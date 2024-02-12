<template>
  <div>
    <h1>Home</h1>
    <p>Home page content</p>
    <p>{{ allSolarSystems }}</p>

    <div class="overflow-hidden">
    <div class="bg-contain bg-center bg-no-repeat bg-primary border-round h-20rem w-full" style="background-image: url('images/product-placeholder-green.svg');"></div>
    </div>
    
  </div>
</template>

<script setup lang="ts">
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
