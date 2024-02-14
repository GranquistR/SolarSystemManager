<template>
  <div>
    <HeaderBar></HeaderBar>
    <div style="height: 90vh" class="flex align-items-center justify-content-center">
      <LoginCard></LoginCard>
    </div>
  </div>
</template>

<script setup lang="ts">
import LoginCard from '@/components/Login/LoginCard.vue'
import HeaderBar from '@/components/Header/HeaderBar.vue'
</script>
