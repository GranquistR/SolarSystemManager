<template>
  <Card style="height: 40rem" class="w-25rem align-items-center overflow-hidden pb-5">
    <template #header>
      <img src="../../assets/Images/star-banner.webp" alt="Space Image" height="200px" />
      <ProgressBar v-if="isLoading" mode="indeterminate" style="height: 5px"></ProgressBar>
      <div v-else style="height: 5px"></div>
    </template>
    <template #title>
      <h1>SpaceBox</h1>
    </template>
    <template #subtitle>Welcome to the Universe!</template>
    <template #content>
      <div class="flex flex-column gap-2">
        <label for="username">Username</label>
        <InputText variant="filled" id="username" v-model="username" style="width: 245px" />
        <label for="password">Password</label>
        <Password variant="filled" id="password" v-model="password" />
      </div>
      <Button class="mt-3" @click="Login"> Login </Button>
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
import User from '@/Entities/UserLogin'
import encrypt from '@/services/encryption'
const username = ref('')
const password = ref('')

const isLoading = ref(false)
const hasFailed = ref(false)

function Login() {
  isLoading.value = true

  LoginService.Login(new User(username.value, encrypt(password.value, "saltEX"))).then((response) => {
    if (response == 'Success!') {
      isLoading.value = false
      window.location.href = '/dashboard'
    } else {
      isLoading.value = false
      hasFailed.value = true
      setTimeout(() => {
        hasFailed.value = false
      }, 2000)
    }
  })
}
</script>
