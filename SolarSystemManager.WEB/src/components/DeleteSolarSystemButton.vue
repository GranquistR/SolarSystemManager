<template>
  <Button icon="pi pi-trash" class="p-button-rounded p-button-danger" @click="confirmDelete()" />
  <Dialog v-model:visible="deleteDialogVisible" :closable="false">
    <p>
      Are you sure you want to delete
      <strong style="color: #f44336; font-size: 1.3em"> {{ solarSystem.systemName }}</strong> ?
    </p>
    <template #footer>
      <Button label="Cancel" icon="pi pi-times" @click="deleteDialogVisible = false" />
      <Button label="Yes" icon="pi pi-check" @click="deleteSolarSystem" />
    </template>
  </Dialog>
</template>
<script setup lang="ts">
import { inject, ref } from 'vue'
import Button from 'primevue/button'
import SolarSystemService from '@/services/SolarSystemService'
import type User from '@/Entities/User'
import Dialog from 'primevue/dialog'

const user = inject<User | null>('currentUser')
const deleteDialogVisible = ref(false)

const props = defineProps<{
  solarSystem: any
  removeSolarSystem: Function
  success: Function
  fail: Function
}>()

function confirmDelete() {
  deleteDialogVisible.value = true
}

function deleteSolarSystem() {
  if (user) {
    SolarSystemService.DeleteSolarSystem(props.solarSystem.systemId, user)
      .then((response) => {
        if (response.success) {
          props.success()
          deleteDialogVisible.value = false
          props.removeSolarSystem(props.solarSystem.systemId)
        } else {
          props.fail()
        }
      })
      .catch((error) => {
        props.fail()
        console.error(error)
      })
  }
}
</script>
