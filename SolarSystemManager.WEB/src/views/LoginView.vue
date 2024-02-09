<template>
  <Card>
    <template #title>Login</template>
    <template #content>
      <div class="flex flex-column gap-2">
        <label for="username">Username</label>
        <InputText id="username" v-model="username" style="width: 250px" />
        <label for="password">Password</label>
        <Password id="password" v-model="password" />
      </div>
    </template>
  </Card>

  <Button>Hello</Button>

  {{ password }}
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

<style scoped>
/* css here */
</style>
