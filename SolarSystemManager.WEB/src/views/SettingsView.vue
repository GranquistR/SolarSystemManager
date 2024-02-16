<template>
  <HeaderBar require-login></HeaderBar>
  <div class="pt-5 flex flex-column align-items-center justify-content-center">
    <Card class="m-2 text-center w-30rem">
      <template #header> </template>
      <template #title>Hello, {{ username }} </template>
      <template #subtitle>User type here</template>
      <template #content> <Button label="Logout" class="mt-3" @click="logout"></Button> </template>
      <template #footer> </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { ref, onMounted } from 'vue'

const username = ref('')
onMounted(() => {
  const cookies = document.cookie.split(';')
  for (let i = 0; i < cookies.length; i++) {
    const cookie = cookies[i].split('=')
    if (cookie[0].trim() === 'username') {
      username.value = cookie[1]
    }
  }
})

function logout() {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  window.location.href = '/'
}
</script>
