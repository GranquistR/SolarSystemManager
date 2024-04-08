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
          <RouterLink to="/new">
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

            <!--Delete button-->
            <Column header="Delete">
            <template #body="slotProps">
            <Button icon="pi pi-trash" class="p-button-rounded p-button-danger" @click="confirmDelete(slotProps.data.systemId)" />
            </template>
            </Column>
            <!--End Delete button-->
          </DataTable>

          <!--Delete confirmation dialog-->
          <Dialog v-model:visible="deleteDialogVisible" :closable="false">
          <p>Are you sure you want to delete?</p>
          <template #footer>
          <Button label="Cancel" icon="pi pi-times" @click="deleteDialogVisible = false" />
          <Button label="Yes" icon="pi pi-check" @click="deleteSolarSystem" />
          </template>
          </Dialog>

        </template>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import Card from 'primevue/card'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SolarSystemService from '@/services/SolarSystemService'
import Button from 'primevue/button'
import router from '@/router'
import Dialog from 'primevue/dialog'
import { inject, ref } from 'vue'
import type SolarSystem from '@/Entities/SolarSystem'
import type User from '@/Entities/User'

const user = inject<User | null>('currentUser', null);

const solarSystems = ref<any>([])
const deleteDialogVisible = ref(false)
const systemIdToDelete = ref<number | null>(null);

const confirmDelete = (systemId: number) => {
  systemIdToDelete.value = systemId;
  deleteDialogVisible.value = true;
};

SolarSystemService.GetPublicSolarSystems().then((response) => {
  solarSystems.value = response.data
  solarSystems.value.forEach((solarSystem: any) => {
    solarSystem.systemVisibility = solarSystem.systemVisibility == 0 ? 'Public' : 'Private'
  })
})

function ViewerGoTo(systemId: number) {
  router.push(`viewer/${systemId}`)
}

const deleteSolarSystem = () => {

  //Check if user is logged in and systemIdToDelete is not null
  if (systemIdToDelete.value !== null && user) {
    const userCredentials = {
      username: user.username,
      password: user.password,
    };

    // Call the DeleteSolarSystem function in the SolarSystemService
    SolarSystemService.DeleteSolarSystem(userCredentials, systemIdToDelete.value)
    
      //Handle the response
      //If the deletion is successful, it updates the list of solar systems displayed in the UI or state management library.
      .then(response => {
        if (response.success) {
          solarSystems.value = solarSystems.value.filter((system: SolarSystem) => system.systemId !== systemIdToDelete.value);
      
          //Show success message
          console.log('Solar system deleted successfully');

        } else {
          //Show error message
          console.error('Failed to delete solar system:', response.message);
        }
      })
      .catch(error => {
        //Show error message
        console.error('Error while deleting solar system:', error);
      })
      .finally(() => {
        //Close the dialog
        deleteDialogVisible.value = false;
        systemIdToDelete.value = null;
      });
  }
};

console.log(SolarSystemService);

</script>
