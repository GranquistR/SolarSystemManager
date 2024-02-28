<template>
  <div>
    <HeaderBar no-links></HeaderBar>
    <div style="height: 90vh" class="flex align-items-center justify-content-center">
      <SignupCard></SignupCard>
    </div>
  </div>
</template>

<script setup lang="ts">
import SignupCard from '@/components/Login/SignupCard.vue'
import HeaderBar from '@/components/Header/HeaderBar.vue'
</script>
