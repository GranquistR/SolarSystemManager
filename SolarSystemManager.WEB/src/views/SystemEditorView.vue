<template>
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  {{ allSolarSystems }}
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'

const allSolarSystems = ref<string>('')

SolarSystemService.SolarSystemInfoTest().then((response) => {
  allSolarSystems.value = response
})
</script>