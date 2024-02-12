<template>
  <Card style="height: 40rem" class="w-25rem align-items-center overflow-hidden pb-5">
    <template #header>
      <img src="../../assets/Images/star-banner.webp" alt="Space Image" height="200px" />
      <ProgressBar v-if="isLoading" mode="indeterminate" style="height: 5px"></ProgressBar>
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
    </template>
    <template #footer>
      <Button @click="Login"> Login </Button>
    </template>
  </Card>
</template>

<script setup lang="ts">
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import Card from 'primevue/card'
import ProgressBar from 'primevue/progressbar'
import { ref } from 'vue'

const username = ref('')
const password = ref('')

const isLoading = ref(false)

function Login() {
  isLoading.value = true
  //simulate login wait.
  //replace with call to login service
  setTimeout(() => {
    window.location.href = '/dashboard'
    isLoading.value = false
  }, 3000)
}
</script>
