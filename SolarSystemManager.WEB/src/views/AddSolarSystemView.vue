<template>
  <HeaderBar no-docking require-login></HeaderBar>
  <div class="bg-cover bg-center w-full image-background" style="height: 100vh"></div>
  <Card style="height: fit-content" class="w-25rem align-items-center overflow-hidden center-card">
    <template #header>
      <img src="../assets/Images/galaxy_banner.jpg" alt="Space Image" height="200px" />
    </template>
    <template #title>
      <h2>Create Solar System</h2>
    </template>
    <template #content>
      <div class="flex flex-column gap-2">
        <label for="Solar system name">Solar System Name</label>
        <InputText variant="filled" id="username" v-model="name" style="width: 245px" />
        <label for="password">Privacy</label>
        <ToggleButton
          v-tooltip.top="'Visibility to others'"
          v-model="isPrivate"
          onLabel="Private"
          offLabel="Public"
          onIcon="pi pi-lock"
          offIcon="pi pi-lock-open"
          class="w-9rem"
        />
        <div class="flex mt-3">
          <Button @click="Create" :disabled="name == ''"> Create </Button>
        </div>
      </div>
    </template>
    <template #footer>
      <transition-group name="p-message" tag="div">
        <Message severity="error" :closable="false" v-if="hasFailed">
          Failed to create solar system, Try again later!
        </Message>
      </transition-group>
    </template>
  </Card>
</template>
<script setup lang="ts">
//components
import HeaderBar from '@/components/Header/HeaderBar.vue'
import Button from 'primevue/button'
import ToggleButton from 'primevue/togglebutton'
import Card from 'primevue/card'
import InputText from 'primevue/inputtext'
import Message from 'primevue/message'
//vue
import { inject, ref } from 'vue'
import router from '@/router'
//services
import SolarSystemService from '@/services/SolarSystemService'
//types
import type User from '@/Entities/User'

const name = ref('')
const isPrivate = ref(false)
const hasFailed = ref(false)

let user: User | undefined = inject('currentUser')

function Create() {
  if (user == undefined) {
    router.push('/login')
  } else {
    SolarSystemService.CreateSolarSystem(name.value, isPrivate.value, user)
      .then((response) => {
        console.log(response)
        if (response.success === false) {
          hasFailed.value = true
          setTimeout(() => {
            hasFailed.value = false
          }, 2000)
        } else {
          router.push('/viewer/' + response.data)
        }
      })
      .catch((error) => {
        console.error(error)
      })
  }
}
</script>
<style scoped>
.center-card {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
.image-background {
  position: relative;
}
.image-background::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: url(../assets/Images/stars.webp) center center / cover no-repeat;
  z-index: -1;
}
</style>
