<template>
  <div>
    <div
      :style="{
        backdropFilter: 'blur(3px)',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
        borderBottom: 'solid #27272a 1px',
        opacity: scrollY != 0 ? 1 : 0,
        transition: 'opacity 0.3s ease-in-out',
        height: '66px'
      }"
      class="fixed w-full"
    ></div>

    <!-- actual fixed header -->
    <div class="fixed w-full p-2 flex align-items-center">
      <!-- logo -->
      <RouterLink class="router-link-unstyled" to="/">
        <h1 class="ml-8 flex">
          <img src="../../assets/Images/spacebox.png" alt="SpaceBox Logo" height="50px" />
          <div class="ml-4 m-auto" style="letter-spacing: 0.8rem">SPACEBOX</div>
        </h1>
      </RouterLink>

      <!-- spacer -->
      <div class="flex-grow-1"></div>

      <!-- navigation -->
      <nav class="mr-5" v-if="!noLinks">
        <!-- logged out links -->
        <div v-if="!isLoggedIn">
          <!-- login link -->
          <RouterLink to="/login">
            <Button icon="pi pi-user" severity="secondary" rounded outlined></Button>
          </RouterLink>
        </div>

        <!-- logged in links -->
        <div v-else class="icon-container">
          <!-- dashboard link -->
          <RouterLink class="mr-2" to="/dashboard" v-if="isLoggedIn">
            <Button outlined severity="secondary" icon="pi pi-home" aria-label="Filter"></Button>
          </RouterLink>
          <!-- user settings link  -->
          <RouterLink to="/settings" v-if="isLoggedIn">
            <Button
              :label="username + '&nbsp;&nbsp;'"
              icon="pi pi-user"
              severity="secondary"
              iconPos="right"
              outlined
            ></Button>
          </RouterLink>
        </div>
      </nav>
    </div>
    <!-- spacer -->
    <div style="height: 66px"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Button from 'primevue/button'
import { RouterLink } from 'vue-router'

const Props = defineProps<{
  noLinks?: boolean
  requireLogin?: boolean
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
@font-face {
  font-family: nasa-font;
  src: url(../../assets/Fonts/nasalization-rg.ttf);
}
h1 {
  font-family: nasa-font;
}
.icon-container {
  display: flex;
  flex-direction: row;
  flex-wrap: nowrap;
}
</style>
