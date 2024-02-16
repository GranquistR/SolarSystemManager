<template>
  <HeaderBar require-login></HeaderBar>
  <div>Dash</div>

  <!-- <div class="flex justify-content-end flex-wrap">
    <router-link to="/">
      <Button label="Log Out"></Button>
    </router-link>
  </div> -->
 
  <DataTable :value="JSON.parse(solarSystems)" >
    <Column field="solarSystemID" header="ID"></Column>
    <column field="ownerID" header="Owner ID"></column>
    <Column field="systemName" header="Name"></Column>
    <Column field="systemIsPrivate" header="Privacy"></Column>
  </DataTable>
      

</template>



<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Row from 'primevue/row'
import InputText from 'primevue/inputtext'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'

const solarSystems = ref<any>([])

console.log('code here')

//const currentSolarSystem = ref<string>('')

SolarSystemService.GetPublicSolarSystems().then((response) => {
  solarSystems.value = response
})
</script>

<style scoped>
/* css here */
</style>
