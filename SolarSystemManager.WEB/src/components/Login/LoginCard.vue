<template>
  <Card style="height: fit-content" class="w-25rem align-items-center overflow-hidden">
    <template #header>
      <img src="../../assets/Images/star-banner.webp" alt="Space Image" height="200px" />
      <ProgressBar v-if="isLoading" mode="indeterminate" style="height: 5px"></ProgressBar>
      <div v-else style="height: 5px"></div>
    </template>
    <template #title>
      <h1>Login</h1>
    </template>
    <template #subtitle>Welcome back, Traveler!</template>
    <template #content>
      <div class="flex flex-column gap-2">
        <label for="username">Username</label>
        <InputText variant="filled" id="username" v-model="username" style="width: 245px" />
        <label for="password">Password</label>
        <Password variant="filled" id="password" v-model="password" :feedback="false" />
      </div>
      <div class="flex mt-3">
        <Button @click="Login"> Login </Button>
        <RouterLink to="/signup" style="margin: auto">Create Account</RouterLink>
      </div>
    </template>
    <template #footer>
      <transition-group name="p-message" tag="div">
        <Message severity="error" :closable="false" v-if="hasFailed">
          &nbsp;Invalid Credentials
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

import { ref } from 'vue'
import LoginService from '@/services/LoginService'
import encrypt from '@/scripts/Encryption/encryption'
import UserRequest from '@/Entities/UserRequest'

const username = ref('')
const password = ref('')
const isLoading = ref(false)
const hasFailed = ref(false)

async function Login() {
  isLoading.value = true
  if (username.value == '' || password.value == '') {
    failedLogin()
    return
  }

  try {
    // Fetch salt
    let salt = ''
    await LoginService.GetSalt(username.value).then((response) => {
      salt = response.data
    })

    // Encrypt password using fetched salt
    const encryptedPassword = encrypt.encrypt(password.value, salt)

    // Attempt login
    await LoginService.Login(new UserRequest(username.value, encryptedPassword)).then(
      (response) => {
        if (response.success) {
          isLoading.value = false
          const date = new Date()
          date.setTime(date.getTime() + 24 * 60 * 60 * 1000)
          document.cookie = `username=${username.value}; expires=${date}`
          document.cookie = `password=${encryptedPassword}; expires=${date}`
          window.location.href = '/dashboard'
        } else {
          failedLogin()
        }
      }
    )
  } catch (error) {
    console.error('Error in LoginService: ', error)
    alert('Error in LoginService. Check console for details.')
    failedLogin()
  }
}

function failedLogin() {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  isLoading.value = false
  hasFailed.value = true
  setTimeout(() => {
    hasFailed.value = false
  }, 2000)
}
</script>
