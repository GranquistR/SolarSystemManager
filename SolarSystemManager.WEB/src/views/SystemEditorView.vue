<template>
  <HeaderBar require-login></HeaderBar>
  <div>Editor</div>
  {{ currentSolarSystem.systemName }} <!--JSON output :(-->
  <!--I know it doesn't look pretty I just want it to work-->
  <div class="flex justify-content-center">
    <div class="flex-column flex w-9">
      <Card class="mb-4">
        <template #title> Add or remove stars and planets to your solar system </template>
        <template #content>
            <Button> Add space object </Button>
            <Button> Remove space object </Button>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'
import Card from 'primevue/card'
import Button from 'primevue/button'

const currentSolarSystem = ref<any>([])

SolarSystemService.GetSolarSystem(22).then((response) => {
  currentSolarSystem.value = response
})

const test = ref<string>("")

SolarSystemService.AddSpaceObject(1).then((response) => {
  test.value = response
})
</script>