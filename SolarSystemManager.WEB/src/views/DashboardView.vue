<template>
  <HeaderBar require-login></HeaderBar>
  <CustomMessage ref="message"></CustomMessage>

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
            @row-click="(row) => ViewerGoTo(row.data.systemId)"
          >
            <Column header="Name">
              <template #body="slotProps">
                <span class="highlight-name">{{ slotProps.data.systemName }}</span>
              </template>
            </Column>

            <!--New Column: Total bodies in each system.-->
            <Column header="# of Objects">
              <template #body="slotProps">
                <div>
                  <span>Celestial Objects:</span>
                  {{ slotProps.data.spaceObjects.length }}
                </div>
              </template>
            </Column>

            <Column header="Privacy">
              <template #body="slotProps">
                <span
                  v-tooltip.top="'Private'"
                  v-if="slotProps.data.systemVisibility === 1"
                  class="privacy-icon private"
                >
                  <i class="pi pi-lock"></i>
                </span>
                <span v-tooltip.top="'Public'" v-else class="privacy-icon public">
                  <i class="pi pi-globe"></i>
                </span>
              </template>
            </Column>

            <!--Delete button-->
            <Column header="Delete">
              <template #body="slotProps">
                <DeleteSolarSystemButton
                  :solarSystem="slotProps.data"
                  :removeSolarSystem="removeSolarSystem"
                  :success="success"
                  :fail="fail"
                />
              </template>
            </Column>
            <!--End Delete button-->
          </DataTable>
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
import Button from 'primevue/button'
import router from '@/router'
import { inject, ref } from 'vue'
import User from '@/Entities/User'
import CustomMessage from '@/components/CustomMessage.vue'
import DeleteSolarSystemButton from '@/components/DeleteSolarSystemButton.vue'

const user = inject<User | null>('currentUser')
const solarSystems = ref<any>([])
const message = ref()

function removeSolarSystem(systemId: number) {
  solarSystems.value = solarSystems.value.filter(
    (solarSystem: any) => solarSystem.systemId !== systemId
  )
}

function success() {
  message.value.ShowMessage('Successfully Deleted.')
}

function fail() {
  message.value.ShowMessage('Failed to delete. Please try again.')
}

//Shows user owned solar systems
if (user) {
  SolarSystemService.GetUserSolarSystems(user).then((response) => {
    if (response.success === false) {
      throw new Error('Failed to load solar systems')
    }
    solarSystems.value = response.data
  })
}

function ViewerGoTo(systemId: number) {
  router.push(`viewer/${systemId}`)
}
</script>
