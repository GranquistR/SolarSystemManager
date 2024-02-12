<template>
  <Card class="w-25rem align-items-center overflow-hidden pb-5">
    <template #header>
      <img src="../../assets/Images/star-banner.webp" alt="Space Image" height="200px" />
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
      <Button>Login</Button>
    </template>
  </Card>
</template>

<script setup lang="ts">
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import Card from 'primevue/card'

import { ref } from 'vue'

console.log('code here')
const username = ref('')
const password = ref('')
</script>
