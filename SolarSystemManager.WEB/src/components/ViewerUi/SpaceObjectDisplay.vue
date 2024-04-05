<template>
  <img
    src="../../assets/Images/sprites/gasPlanet.png"
    :width="size"
    :height="size"
    :style="{
      filter:
        'opacity(0.7) drop-shadow(0 0 0px ' +
        spaceObject.objectColor +
        ') drop-shadow(0 0 0px ' +
        spaceObject.objectColor +
        ') contrast(1.3)'
    }"
  />
</template>
<script setup lang="ts">
defineProps<{
  spaceObject: any
  size: number
}>()
</script>
<style scoped>
</style>
