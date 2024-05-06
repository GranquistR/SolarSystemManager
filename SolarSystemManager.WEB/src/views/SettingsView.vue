<template>
  <div class="lol" v-if="lightMode">LIGHT MODE SUCKS</div>
  <HeaderBar require-login></HeaderBar>
  <div class="pt-5 flex flex-column align-items-center justify-content-center">
    <Card class="m-2 text-center w-30rem">
      <template #header> </template>
      <template #title>Hello, {{ username }} </template>
      <template #subtitle>User type: {{ userType }}</template>
      <template #content>
        <div class="flex flex-column align-items-center gap-2">
          <Button
            label="Logout"
            icon="pi pi-sign-out"
            rounded
            outlined
            class="mt-3"
            @click="logout"
          ></Button>
          <SettingsModel :paramToEdit="'Change Username'"></SettingsModel>
          <SettingsModel :paramToEdit="'Change Password'"></SettingsModel>
          <ToggleButton
            style="z-index: 10000"
            v-model="lightMode"
            onLabel="Extra Light Mode"
            onIcon="pi pi-sun"
            offLabel="Dark Mode"
            offIcon="pi pi-moon"
          />
        </div>
      </template>
      <template #footer></template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { ref, onMounted } from 'vue'
import SettingsModel from '@/components/Settings/SettingsModel.vue'
import User from '@/Entities/User'
import { inject } from 'vue'
import ToggleButton from 'primevue/togglebutton'

const username = ref<string>('')
const userType = ref<string>('')
const lightMode = ref<boolean>(false)

onMounted(() => {
  let user: User | undefined = inject('currentUser')

  if (user != undefined) {
    username.value = user.username
    userType.value = user.role == 1 ? 'Administrator' : 'Member'
  }
})

function logout() {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  window.location.href = '/'
}
</script>
<style scoped>
.lol {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: #ffffff;
  z-index: 1000;
  color: rgb(248, 248, 248);
  font-size: 10rem;
  text-align: center;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
