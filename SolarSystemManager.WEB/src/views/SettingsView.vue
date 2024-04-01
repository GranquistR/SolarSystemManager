<template>
  <HeaderBar require-login></HeaderBar>
  <div class="pt-5 flex flex-column align-items-center justify-content-center">
    <Card class="m-2 text-center w-30rem">
      <template #header> </template>
      <template #title>Hello, {{ username }} </template>
      <template #subtitle>User type: {{ userType }}</template>
      <template #content> <Button label="Logout" class="mt-3" @click="logout"></Button> </template>
      <template #footer> </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import LoginService from '@/services/LoginService'
import UserRequest from '@/Entities/UserRequest'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { ref, onMounted } from 'vue'

const username = ref<string>('')
const userType = ref<string>('')

onMounted(() => {
  if (document.cookie.includes('username=')) {
    const _username = document.cookie.split('username=')[1].split(';')[0]
    const _password = document.cookie.split('password=')[1].split(';')[0]

    LoginService.GetUserSettings(new UserRequest(_username, _password))
      .then((result) => {
        username.value = result.username
        userType.value = result.role === 1 ? 'Administrator' : 'Member'
      })
      .catch((error: any) => {
        console.error('Error:', error)
      })
  }
})

function logout() {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  window.location.href = '/'
}
</script>