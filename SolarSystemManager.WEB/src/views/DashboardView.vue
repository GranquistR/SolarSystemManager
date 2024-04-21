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
        <template #title> Select Your Cosmic Celestial !</template>
        <template #subtitle> Expand or shrink your universe </template>
        <template #content>
          <CustomMessage ref="message"></CustomMessage>
          <DataTable
            selectionMode="single"
            :value="solarSystems"
            @row-click="(row) => ViewerGoTo(row.data.systemId)">
          

            <Column header="Name">
              <template #body="slotProps">
                <span class="highlight-name">{{ slotProps.data.systemName }}</span>
              </template>
            </Column>

            <Column header="Privacy">
              <template #body="slotProps">
                <span v-if="slotProps.data.systemVisibility === 1" class="privacy-icon private">
                  <i class="pi pi-lock" style="color: #f44336;"></i>
                </span>
                <span v-else class="privacy-icon public">
                  <i class="pi pi-globe"></i>
                </span>
              </template>
            </Column>

            <!--Delete button-->
            <Column header="Delete">
            <template #body="slotProps">
            <Button icon="pi pi-trash" class="p-button-rounded p-button-danger" @click="confirmDelete(slotProps.data.systemId)" />
            </template>
            </Column>
            <!--End Delete button-->
          </DataTable>

          <!--Delete system dialog-->
          <Dialog v-model:visible="deleteDialogVisible" :closable="false">
          <p>Are you sure you want to delete "<strong style="color: #f44336; font-size: 1.3em;">
            {{ systemNameToDelete }}</strong>" ?</p>
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

<style scoped>

  .highlight-name {
    font-weight: bold;
    color: white; /*TODO: Choose a better color */
    font-size: 1.2em; /*TODO: Choose a better font size*/
  }

  .privacy-icon {
    font-size: 1.2em;
  }

  .privacy-icon.private {
    color: red;
    /* Additional styles if needed */
  }

  .privacy-icon.public {
    color: rgb(2, 160, 2); /*TODO: Choose a better color */
  }
</style>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import Card from 'primevue/card'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import SolarSystemService from '@/services/SolarSystemService'
import DeleteSolarSystemRequest from '@/Entities/DeleteSolarSystemRequest'
import Button from 'primevue/button'
import router from '@/router'
import Dialog from 'primevue/dialog'
import { inject, ref } from 'vue'
import User from '@/Entities/User'
import SolarSystem from '@/Entities/SolarSystem'
import CustomMessage from '@/components/CustomMessage.vue'

const user = inject<User | null>('currentUser');
const solarSystems = ref<any>([])
const deleteDialogVisible = ref(false)
const systemIdToDelete = ref<number | null>(null);
const message = ref()
const systemNameToDelete = ref('');


const confirmDelete = (systemId: number) => {
  const system = solarSystems.value.find((s: SolarSystem) => s.systemId === systemId);
  systemNameToDelete.value = system ? system.systemName : '';
  systemIdToDelete.value = systemId;
  deleteDialogVisible.value = true;
};

//Shows user owned solar systems
if (user){
  SolarSystemService.GetUserSolarSystems(user)
    .then((response) => {
      if (response.success === false) {
        throw new Error('Failed to load solar systems')
      }
      solarSystems.value = response.data
    })
};

function ViewerGoTo(systemId: number) {
  router.push(`viewer/${systemId}`)
}

const deleteSolarSystem = () => {
  //Check if user is logged in and systemIdToDelete is not null
  if (systemIdToDelete.value !== null && user) {

  //Create user credentials object properties taken from the user object.
  const userCredentials = {
    username: user.username,
    password: user.password,
  };

  //Call the DeleteSolarSystem function in the SolarSystemService
  SolarSystemService.DeleteSolarSystem(new DeleteSolarSystemRequest(userCredentials.username, userCredentials.password, systemIdToDelete.value))
  
    //If the deletion is successful, it updates the list of solar systems displayed in the UI or state management library.
    .then(response => {
      if (response.success) {
        
        //Show success message
        message.value.ShowMessage('Successfully Deleted.')
        solarSystems.value = solarSystems.value.filter((system: SolarSystem) => system.systemId !== systemIdToDelete.value);
        //Show success message
        console.log('Solar system deleted successfully');

      } else {
        //Show error message
        message.value.ShowMessage('Failed to delete.', 'error')
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
</script>