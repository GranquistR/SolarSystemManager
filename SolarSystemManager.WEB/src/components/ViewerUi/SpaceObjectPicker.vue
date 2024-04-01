<template>
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
        <div>
          {{ slotProps.data.objectName }}
        </div>
      </template>
    </Column>
    <Column>
      <template #body>
        <div class="flex">
          <Button
            outlined
            severity="secondary"
            icon="pi pi-pencil"
            rounded
            iconPos="right"
          ></Button>
          <Button
            class="ml-2"
            outlined
            severity="secondary"
            icon="pi pi-trash"
            rounded
            iconPos="right"
          ></Button>
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SpaceObjectDisplay from '@/components/ViewerUi/SpaceObjectDisplay.vue'

const props = defineProps({
  solarSystem: {
    type: Object,
    default: () => ({ spaceObjects: [] })
  }
})

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
