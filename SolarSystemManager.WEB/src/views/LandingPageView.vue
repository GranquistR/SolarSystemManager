<template>
  <div>
    <HeaderBar></HeaderBar>
  </div>
  <div class="overflow-hidden">
    <div class="bg-cover bg-center border-round h-screen w-full" style="background-image: url('https://images3.alphacoders.com/130/1300409.jpg');">

   
    <h1>landing</h1>
    <p>Home page content</p>
    {{ allSolarSystems }}

    
    </div>
  </div>

  <!-- Background image -->
  <!-- <div class="flex flex-wrap align-items-center justify-content-center">
    <div class="border-round bg-primary-100 w-full h-screen p-3 m-3">
     
    </div>
  </div> -->
  <!-- <div class="overflow-hidden">
    <div class="bg-cover bg-center border-round h-screen w-full" style="background-image: url('https://images3.alphacoders.com/130/1300409.jpg');">
      
    </div>
  </div> -->
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
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
