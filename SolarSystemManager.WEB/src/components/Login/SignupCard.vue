<template>
  <Card style="height: 40rem" class="w-25rem align-items-center overflow-hidden pb-5">
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
          &nbsp;Could not make account. Please try again.
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

const username = ref('')
const password = ref('')

const isLoading = ref(false)
const hasFailed = ref(false)

onMounted(() => {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
})

function Signup() {
  alert('Signup: ' + username.value + ',' + password.value)
}
</script>
