<template>
  <div>
    <div
      :style="{
        backdropFilter: 'blur(3px)',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
        borderBottom: 'solid #27272a 1px',
        opacity: scrollY != 0 ? 1 : 0,
        transition: 'opacity 0.3s ease-in-out',
        height: '66px'
      }"
      class="fixed w-full"
    ></div>

    <!-- actual fixed header -->
    <div class="fixed w-full p-2 flex align-items-center">
      <h1 class="ml-8 flex">
        <img src="../assets/Images/spacebox.png" alt="SpaceBox Logo" height="50px" />
        <span class="ml-4 m-auto">S P A C E B O X</span>
      </h1>

      <!-- spacer -->
      <div class="flex-grow-1"></div>

      <nav class="ml-5 mr-5">
        TEMPORARY NAVIGATION FOR TESTING:
        <RouterLink to="/dashboard">dashboard</RouterLink>
        |
        <RouterLink to="/">Landing</RouterLink>
        |
        <RouterLink to="/login">login</RouterLink>
        |
        <RouterLink to="/notfound">notfound</RouterLink>
        |
        <RouterLink to="/settings">settings</RouterLink>
        |
        <RouterLink to="/signup">signup</RouterLink>
        |
        <RouterLink to="/editor">editor</RouterLink>
        |
        <RouterLink to="/unauthorized">unauthorized</RouterLink>
      </nav>
    </div>
    <!-- spacer -->
    <div style="height: 66px"></div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'
const scrollY = ref(0)

//updates scroll position
window.addEventListener('scroll', () => {
  scrollY.value = window.scrollY
})
</script>

<style scoped>
@font-face {
  font-family: nasa-font;
  src: url(../assets/Fonts/nasalization-rg.ttf);
}

h1 {
  font-family: nasa-font;
}
</style>
