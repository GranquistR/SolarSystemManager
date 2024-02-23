<template>
  <h1 class="ml-8 flex">
    <img
      src="../../assets/Images/spacebox-logo.png"
      alt="SpaceBox Logo"
      :height="50 * sizeMultiplier + 'px'"
    />
    <div
      class="my-auto"
      :style="{
        letterSpacing: 0.8 * sizeMultiplier + 'rem',
        fontSize: 100 * sizeMultiplier + '%',
        marginLeft: 1.5 * sizeMultiplier + 'rem'
      }"
    >
      SPACEBOX
    </div>
  </h1>
</template>
<script setup lang="ts">
const Props = defineProps<{
  sizeMultiplier?: number
}>()
const sizeMultiplier = Props.sizeMultiplier || 1
</script>
<style scoped>
@font-face {
  font-family: nasa-font;
  src: url(../../assets/Fonts/nasalization-rg.ttf);
}
h1 {
  font-family: nasa-font;
}
</style>
