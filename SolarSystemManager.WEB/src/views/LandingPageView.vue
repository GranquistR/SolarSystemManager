<template>
  <div>
    <HeaderBar></HeaderBar>
    <h1>landing</h1>
    <p>Home page content</p>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'

const allSolarSystems = ref<string>('')

SolarSystemService.getAllSolarSystems().then((response) => {
  allSolarSystems.value = response
})
</script>

<style scoped>
/* 
style done here
the scoped tag mean it will only style elements in this component
*/
</style>
