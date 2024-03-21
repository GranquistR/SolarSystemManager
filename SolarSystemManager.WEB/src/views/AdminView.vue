<template>
  <HeaderBar require-login />
  <div v-if="!isLoading">
    <h1>Admin View</h1>
    <p>This is the admin view</p>
  </div>
</template>
<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import { inject, onBeforeMount, ref } from 'vue'
import UserV2 from '@/Entities/UserV2'
import router from '@/router'
import ProgressSpinner from 'primevue/progressspinner'

const isLoading = ref(true)
onBeforeMount(() => {
  const user: UserV2 | undefined = inject('currentUser')
  isLoading.value = true
  if (user == undefined || user == null) {
    router.push('/unauthorized')
  } else if (user.role != 1) {
    router.push('/forbidden')
  } else {
    isLoading.value = false
  }
})
</script>
m
