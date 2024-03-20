<template>
  <HeaderBar require-login></HeaderBar>

  <!-- <div class="flex justify-content-end flex-wrap">
    <router-link to="/">
      <Button label="Log Out"></Button>
    </router-link>
  </div> -->
  <div class="flex justify-content-center">
    <div class="flex-column flex w-9">
      <Card class="mb-4">
        <template #title> Make your own Solar System</template>
        <template #subtitle> The universe is yours to explore </template>
        <template #content>
          <RouterLink to="/editor">
            <Button> Create System </Button>
          </RouterLink>
        </template>
      </Card>
      <Card>
        <template #title> Check out our user-made Solar Systems! </template>
        <template #subtitle> Set your Solar Systems to public to be viewable here. </template>
        <template #content>
          <DataTable
            selectionMode="single"
            :value="solarSystems"
            @row-click="(row) => ViewerGoTo(row.data.systemId)"
          >
            <Column field="systemId" header="ID"></Column>
            <Column field="ownerId" header="Owner ID"></Column>
            <Column field="systemName" header="Name"></Column>
            <Column field="systemVisibility" header="Privacy"></Column>
          </DataTable>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'
import Card from 'primevue/card'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SolarSystemService from '@/services/SolarSystemService'
import { ref } from 'vue'
import Button from 'primevue/button'

import router from '@/router'

const solarSystems = ref<any>([])

console.log('code here')

//const currentSolarSystem = ref<string>('')

SolarSystemService.GetPublicSolarSystems().then((response) => {
  solarSystems.value = response
  solarSystems.value.forEach((solarSystem: any) => {
    solarSystem.systemVisibility = solarSystem.systemVisibility == 0 ? 'Public' : 'Private'
  })
})

function ViewerGoTo(systemId: number) {
  router.push(`viewer/${systemId}`)
}
</script>
