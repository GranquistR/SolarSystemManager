<template>
  <div
    :style="{
      backdropFilter: 'blur(5px)',
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
      borderBottom: 'solid #27272a 1px',
      opacity: scrollY != 0 || noDocking ? 1 : 0,
      transition: 'opacity 0.3s ease-in-out',
      height: '66px',
      zIndex: 100
    }"
    class="fixed w-full"
  ></div>

  <!-- actual fixed header -->
  <div class="fixed w-full p-2 flex align-items-center" style="z-index: 101">
    <!-- logo -->
    <RouterLink class="router-link-unstyled" to="/">
      <SpaceBoxLogo></SpaceBoxLogo>
    </RouterLink>

    <!-- spacer -->
    <div class="flex-grow-1"></div>

    <!-- navigation -->
    <nav class="mr-5" v-if="!noLinks">
      <!-- logged out links -->
      <div v-if="!isLoggedIn">
        <!-- login link -->
        <RouterLink to="/login">
          <Button
            icon="pi pi-user"
            severity="secondary"
            rounded
            outlined
            label="Login &nbsp;&nbsp;"
            iconPos="right"
          ></Button>
        </RouterLink>
      </div>

      <!-- logged in links -->
      <div v-else class="icon-container">
        <!-- dashboard link -->
        <RouterLink class="mr-2" to="/dashboard" v-if="isLoggedIn">
          <Button
            outlined
            severity="secondary"
            icon="pi pi-home"
            rounded
            iconPos="right"
            label="Dashboard &nbsp;&nbsp;"
          ></Button>
        </RouterLink>
        <!-- user settings link  -->
        <RouterLink to="/settings" v-if="isLoggedIn">
          <Button
            :label="username + '&nbsp;&nbsp;'"
            icon="pi pi-user"
            severity="secondary"
            iconPos="right"
            rounded
            outlined
          ></Button>
        </RouterLink>
      </div>
    </nav>
  </div>
  <!-- spacer -->
  <div v-if="!noDocking" style="height: 66px"></div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Button from 'primevue/button'
import { RouterLink } from 'vue-router'
import SpaceBoxLogo from './SpaceBoxLogo.vue'

const Props = defineProps<{
  noLinks?: boolean
  requireLogin?: boolean
  noDocking?: boolean
}>()

onMounted(() => {
  //if username and password cookies exist
  if (document.cookie.includes('username') && document.cookie.includes('password')) {
    isLoggedIn.value = true
    username.value = document.cookie.split('username=')[1].split(';')[0]
  } else {
    if (Props.requireLogin) {
      window.location.href = '/login'
    }
  }
})

const username = ref('')
const isLoggedIn = ref(false)
const scrollY = ref(0)

//updates scroll position
window.addEventListener('scroll', () => {
  scrollY.value = window.scrollY
})
</script>

<style scoped>
.icon-container {
  display: flex;
  flex-direction: row;
  flex-wrap: nowrap;
}
</style>
