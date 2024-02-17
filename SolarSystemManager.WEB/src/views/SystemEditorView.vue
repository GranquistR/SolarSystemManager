<template>
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  {{ currentSolarSystem }}
  

</template>

<script setup lang="ts">
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'
import HeaderBar from '@/components/Header/HeaderBar.vue'

const currentSolarSystem = ref<any>([])

//will eventually get solar system from data base by id
SolarSystemService.GetSolarSystemById(1).then((response) => {
  currentSolarSystem.value = response
})

SolarSystemService.AddSpaceObject("Star").then((response) => {
  currentSolarSystem.value = response
})

</script>