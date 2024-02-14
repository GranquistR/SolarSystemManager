<template>
  <div>
    <HeaderBar dashboard landing login settings signup editor></HeaderBar>

    <img
      src="../assets/Images/openStars.jpg"
      alt="open space"
      style="width: 100%; height: 100%; position: fixed; object-fit: cover"
    />
    <div
      @mouseover="hover = true"
      style="
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        text-align: center;
      "
    >
      <transition name="fade" mode="out-in">
        <template v-if="hover">
          <RouterLink to="/" style="color: white; text-decoration: none">Take Me Home</RouterLink>
        </template>
        <template v-else>
          <RouterLink to="/" style="color: white; text-decoration: none">You're Lost...</RouterLink>
        </template>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import HeaderBar from '@/components/Header/HeaderBar.vue'

const hover = ref(false)
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: 1s -webkit-filter linear;
}
.fade-enter-from,
.fade-leave-to {
  -webkit-filter: blur(20px);
}
</style>
