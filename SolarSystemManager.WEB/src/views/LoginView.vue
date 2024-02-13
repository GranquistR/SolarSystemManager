<template>
  <div>
    <LightHeaderBar></LightHeaderBar>
    <div style="height: 90vh" class="flex align-items-center justify-content-center">
      <LoginCard></LoginCard>
    </div>
  </div>
</template>

<script setup lang="ts">
import LoginCard from '@/components/Login/LoginCard.vue'
import LightHeaderBar from '@/components/LightHeaderBar.vue'
</script>
