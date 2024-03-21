<template>
  <Card style="height: fit-content" class="w-25rem align-items-center overflow-hidden">
    <template #header>
      <img src="../../assets/Images/galaxy_banner.jpg" alt="Space Image" height="200px" />
      <ProgressBar v-if="isLoading" mode="indeterminate" style="height: 5px"></ProgressBar>
      <div v-else style="height: 5px"></div>
    </template>
    <template #title>
      <h1>Create Account</h1>
    </template>
    <template #subtitle>Welcome to the Universe!</template>
    <template #content>
      <div class="flex flex-column gap-2">
        <label for="username">Username</label>
        <InputText variant="filled" id="username" v-model="username" style="width: 245px" />
        <label for="password">Password</label>
        <Password variant="filled" id="password" v-model="password" />
      </div>
      <div class="flex mt-3">
        <Button @click="Signup"> Create Account </Button>
      </div>
    </template>
    <template #footer>
      <transition-group name="p-message" tag="div">
        <Message severity="error" :closable="false" v-if="hasFailed">
          {{ errorMessages }}
        </Message>
      </transition-group>
    </template>
  </Card>
</template>

<script setup lang="ts">
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import Card from 'primevue/card'
import ProgressBar from 'primevue/progressbar'
import Message from 'primevue/message'
import { onMounted, ref } from 'vue'
import LoginService from '@/services/LoginService'
import UserRequest from '@/Entities/UserRequest'

const username = ref('')
const password = ref('')

const isLoading = ref(false)
const hasFailed = ref(false)
const errorMessages = ref('')

onMounted(() => {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
})

function Signup() {
  LoginService.CreateAccount(new UserRequest(username.value, password.value))
    .then((result) => {
      if (result == 'Success!') {
        isLoading.value = false
        let date = new Date()
        date.setTime(date.getTime() + 24 * 60 * 60 * 1000)
        document.cookie = `username=${username.value}; expires=${date}`
        document.cookie = `password=${password.value}; expires=${date}`
        window.location.href = '/dashboard'
      } else {
        errorMessages.value = result
        failed()
      }
    })
    .catch((error: any) => {
      console.error('Error:', error)
      hasFailed.value = true
    })
}

function failed() {
  isLoading.value = false
  hasFailed.value = true
  setTimeout(() => {
    hasFailed.value = false
  }, 2000)
}
</script>
@/Entities/User
