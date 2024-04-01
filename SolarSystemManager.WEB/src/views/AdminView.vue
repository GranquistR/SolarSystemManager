<template>
  <HeaderBar require-login />
  <div v-if="!isLoading">
    <Card class="m-4">
      <template #title> All your solar system are belong to us </template>
      <template #subtitle> </template>
      <template #content>
        <DataTable
          selectionMode="single"
          :value="solarSystems"
          @row-click="(row) => ViewerGoTo(row.data.systemId)"
        >
          <Column field="systemId" header="ID"></Column>
          <Column field="ownerId" header="Owner ID"></Column>
          <Column field="systemName" header="Name"></Column>
          <Column field="systemVisibility" header="Privacy"></Column>
        </DataTable>
      </template>
    </Card>
  </div>
</template>
<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import { inject, onBeforeMount, onMounted, ref } from 'vue'
import User from '@/Entities/User'
import router from '@/router'
import SolarSystemService from '@/services/SolarSystemService'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Card from 'primevue/card'

const isLoading = ref(true)
const user: User | undefined = inject('currentUser')
const solarSystems = ref<any>([])

onBeforeMount(() => {
  isLoading.value = true
  if (user == undefined || user == null) {
    router.push('/unauthorized')
  } else if (user.role != 1) {
    router.push('/forbidden')
  } else {
    SolarSystemService.GetAllSolarSystemsAdmin(user).then((response) => {
      solarSystems.value = response.data
      solarSystems.value.forEach((solarSystem: any) => {
        solarSystem.systemVisibility = solarSystem.systemVisibility == 0 ? 'Public' : 'Private'
      })
    })

    isLoading.value = false
  }
})

function ViewerGoTo(systemId: number) {
  router.push(`viewer/${systemId}`)
}
</script>
m
