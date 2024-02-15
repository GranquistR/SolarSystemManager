<template>
  <img
    src="../../assets/images/spaceman.webp"
    alt="astronaut on a space walk"
    style="width: 100%; height: 100%; position: fixed; object-fit: cover"
  />
  <div>
    <div
      style="
        position: fixed;
        top: 63%;
        left: 50%;
        transform: translate(-50%, -50%);
        text-align: center;
      "
    >
      <h1 style="color: white; font-size: 3rem">{{ title }}</h1>
      <p class="mb-2" style="color: white; font-size: 1.5rem">{{ message }}</p>
      <RouterLink to="/">Go Home</RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
defineProps({
  title: String,
  message: String
})
</script>
