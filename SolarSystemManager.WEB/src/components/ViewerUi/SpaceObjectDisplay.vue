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
import { Application, Sprite } from 'pixi.js'
import { onMounted } from 'vue'

const props = defineProps<{
  spaceObject: any
  size: number
}>()
</script>
<style scoped>
img {
  filter: opacity(0.7) drop-shadow(0 0 0px #c60532) drop-shadow(0 0 0px #c60532) contrast(1.3);
}
</style>
