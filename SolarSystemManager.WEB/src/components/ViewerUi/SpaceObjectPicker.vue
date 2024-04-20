<template>
  <CustomMessage ref="message"></CustomMessage>
  <DataTable
    scrollable
    scrollHeight="78vh"
    class="datatableNoHeader datatableNoBackground frosted pt-1"
    v-model:selection="selectedObject"
    selectionMode="single"
    :value="
      spaceObjects.sort((a: any, b: any) => {
        return a.objectName.localeCompare(b.objectName)
      })
    "
  >
    <template #header>
      <div class="flex flex-wrap align-items-center justify-content-between gap-2">
        <span class="text-xl text-900 font-bold">
          {{ solarSystem.systemName }}
          <span style="color: rgba(0, 0, 0, 0); font-size: 70%">
            {{ 'ID:' + solarSystem.systemId }}
          </span>
        </span>
      </div>
    </template>
    <Column>
      <template #body="slotProps"
        ><SpaceObjectDisplay
          :size="50"
          :space-object="slotProps.data"
        ></SpaceObjectDisplay> </template
    ></Column>
    <Column field="objectName">
      <template #body="slotProps">
        <div>{{ slotProps.data.objectName }}</div>
      </template>
    </Column>
    <Column>
      <template #body="slotProps">
        <div class="flex">
          <AddSpaceObject
            :system="solarSystem"
            :graphics="graphics"
            :space-object-to-edit="slotProps.data"
            :click-position="clickPosition"
          ></AddSpaceObject>
          <Button
            class="ml-2"
            outlined
            severity="secondary"
            icon="pi pi-trash"
            rounded
            iconPos="right"
            @click="openDeleteDialogId = slotProps.data.spaceObjectID"
          ></Button>

          <Dialog
            v-model:visible="openSesame"
            :closable="false"
            v-if="(slotProps.data.spaceObjectID as number) == (openDeleteDialogId as number)"
          >
            <template #header>
              Are you sure you want to delete {{ slotProps.data.objectName }}
            </template>
            <div class="flex justify-content-center">
              <Button
                class="mr-2"
                label="Cancel"
                icon="pi pi-times"
                @click="openDeleteDialogId = null"
              />
              <Button
                label="Confirm"
                icon="pi pi-check"
                @click="RemoveSpaceObject(slotProps.data.spaceObjectID as number)"
              />
            </div>
          </Dialog>
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup lang="ts">
import { computed, ref, watch, inject } from 'vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SpaceObjectDisplay from '@/components/ViewerUi/SpaceObjectDisplay.vue'
import SolarSystemService from '@/services/SolarSystemService'
import type User from '@/Entities/User'
import CustomMessage from '../CustomMessage.vue'
import Dialog from 'primevue/dialog'
import AddSpaceObject from './AddSpaceObject.vue'
import Graphics from '@/scripts/pixie/DrawSolarSystem'

const message = ref()

const user: User | undefined = inject('currentUser')
const openDeleteDialogId = ref<number | null>(null)
const openSesame = ref(true)

const props = defineProps({
  solarSystem: {
    type: Object,
    default: () => ({ spaceObjects: [] })
  },
  graphics: Graphics
})

const clickPosition = ref<any>()
defineExpose({ selectPosition })
function selectPosition(event: any) {
  clickPosition.value = event
}

//computed is required to make the spaceObjects reactive
const spaceObjects = computed(() => {
  return props.solarSystem.spaceObjects
})

const selectedObject = ref<any>()
const emit = defineEmits(['select-id'])

watch(
  selectedObject,
  (newValue) => {
    emit('select-id', newValue)
  },
  { immediate: true }
)

function RemoveSpaceObject(id: number) {
  if (user) {
    SolarSystemService.RemoveSpaceObject(user, id).then((data) => {
      console.log(data)
      if (data.success) {
        message.value.ShowMessage('Successfully Deleted.')
        //remove object from array
        const index = spaceObjects.value.findIndex((x: any) => x.spaceObjectID === id)
        spaceObjects.value.splice(index, 1)
        if (props.graphics) {
          props.graphics.DrawSolarSystem(props.solarSystem)
        }
      } else {
        message.value.ShowMessage('Failed to delete.', 'error')
      }
    })
  }
}
</script>
<style scoped>
.frosted {
  backdrop-filter: blur(5px);
  background-color: rgba(0, 0, 0, 0.5);
  border: solid #27272a 1px;
  border-radius: 10px;
  transition: opacity 0.3s ease-in-out;
  z-index: 990;
}
</style>

v-model:visible="isDialogVisible"
