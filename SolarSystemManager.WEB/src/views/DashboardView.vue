<template>
  <HeaderBar require-login></HeaderBar>

  <!-- <div class="flex justify-content-end flex-wrap">
    <router-link to="/">
      <Button label="Log Out"></Button>
    </router-link>
  </div> -->
  <div class="flex justify-content-center">
    <Card class="w-9">
      <template #title> Check out our user-made Solar Systems! </template>
      <template #subtitle> Set your Solar Systems to public to be viewable here. </template>
      <template #content>
        <DataTable :value="solarSystems">
          <Column field="systemId" header="ID"></Column>
          <column field="ownerId" header="Owner ID"></column>
          <Column field="systemName" header="Name"></Column>
          <Column field="systemVisibility" header="Privacy"></Column>
        </DataTable>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import Card from 'primevue/card'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'

const solarSystems = ref<any>([])

console.log('code here')

//const currentSolarSystem = ref<string>('')

SolarSystemService.GetPublicSolarSystems().then((response) => {
  solarSystems.value = JSON.parse(response)
  solarSystems.value.forEach((solarSystem: any) => {
    solarSystem.systemVisibility = solarSystem.systemVisibility == 0 ? 'Public' : 'Private'
  })
})
</script>

<style scoped>
/* css here */
</style>
