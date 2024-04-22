<template>
  <CustomMessage ref="message"></CustomMessage>
  <div
    class="flex"
    :style="{
      transform: collapsed ? 'translateX(-90%)' : 'translateX(0)',
      transition: 'transform 0.3s ease'
    }"
  >
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
              v-if="graphics"
              :ref="(x) => AddSpaceObjectRefs.push(x)"
              :system="solarSystem"
              :graphics="graphics"
              :space-object-to-edit="slotProps.data"
              @opened="emit('opened')"
              @closed="emit('closed')"
              :disabled="editingDisabled"
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
    <div>
      <Button
        class="ml-2 mt-2"
        outlined
        rounded
        severity="contrast"
        :icon="collapsed ? 'pi pi-angle-double-right' : 'pi pi-angle-double-left'"
        iconPos="right"
        @click="collapsed = !collapsed"
      ></Button>
    </div>
  </div>
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
import type { transform } from 'typescript'
//props
const props = defineProps({
  solarSystem: {
    type: Object,
    default: () => ({ spaceObjects: [] })
  },
  graphics: Graphics,
  editingDisabled: Boolean
})

//refs
const message = ref()
const user: User | undefined = inject('currentUser')
const openDeleteDialogId = ref<number | null>(null)
const openSesame = ref(true)
const AddSpaceObjectRefs = ref<any[]>([])
const selectedObject = ref<any>()
const collapsed = ref(false)

//expose
defineExpose({ selectPosition })
function selectPosition(event: any) {
  AddSpaceObjectRefs.value.forEach((x) => {
    x.selectPosition(event)
  })
}

//computed
const spaceObjects = computed(() => {
  return props.solarSystem.spaceObjects
})

//emits
const emit = defineEmits(['select-id', 'opened', 'closed'])
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
collapse-animate {
  transition: width 0.3s ease-in-out;
}
</style>

v-model:visible="isDialogVisible"
