<template>
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  {{ currentSolarSystem }}
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'

const currentSolarSystem = ref<any>([])

SolarSystemService.GetSolarSystemById(1).then((response) => {
  currentSolarSystem.value = response
})
</script>