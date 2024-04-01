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
  </Card>
</template>
<script setup lang="ts">
import Button from 'primevue/button'
import HeaderBar from '@/components/Header/HeaderBar.vue'
import ToggleButton from 'primevue/togglebutton'
import Card from 'primevue/card'
import InputText from 'primevue/inputtext'
import { ref } from 'vue'

const name = ref('')
const isPrivate = ref(false)

function Create() {
    
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
