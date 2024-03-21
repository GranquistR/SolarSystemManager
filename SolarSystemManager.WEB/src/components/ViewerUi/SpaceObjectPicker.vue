<template>
  <div class="frosted p-4 flex flex-column">
    <div
      v-for="(spaceObject, index) in spaceObjects"
      :key="index"
      class="my-4 flex align-items-center"
      @click="SelectId(spaceObject.spaceObjectID)"
    >
      <img src="../../assets/Images/spacebox-logo.png" height="40px" />
      <div class="align-items-center ml-3">{{ spaceObject.objectName }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps({
  solarSystem: {
    type: Object,
    default: () => ({ spaceObjects: [] })
  }
})

//computed is required to make the spaceObjects reactive
const spaceObjects = computed(() => {
  console.log('spaceObjects', props.solarSystem.spaceObjects)
  return props.solarSystem.spaceObjects
})

const emit = defineEmits(['select-id'])
function SelectId(id: number) {
  emit('select-id', id)
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
