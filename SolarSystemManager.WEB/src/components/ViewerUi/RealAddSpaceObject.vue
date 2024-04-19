<template>
  <AddSpaceObject
    ref="addSpaceObject"
    :system="system"
    :graphics="graphics"
    :submit="RealAddSpaceObject"
  ></AddSpaceObject>
</template>
<script setup lang="ts">
import Graphics from '@/scripts/pixie/DrawSolarSystem'
import AddSpaceObject from './AddSpaceObject.vue'
import type SpaceObject from '@/Entities/SpaceObject'
import { ref } from 'vue'
import SolarSystemService from '@/services/SolarSystemService'

const addSpaceObject = ref()

const props = defineProps<{
  system: any
  graphics: Graphics
}>()

defineExpose({ selectPosition })

function selectPosition(event: any) {
  addSpaceObject.value.selectPosition(event)
}

function RealAddSpaceObject(SpaceObject: SpaceObject, success: Function, failed: Function) {
  SolarSystemService.AddSpaceObject(SpaceObject).then((data) => {
    if (data.success) {
      success()
    } else {
      failed()
    }
  })
}
</script>
