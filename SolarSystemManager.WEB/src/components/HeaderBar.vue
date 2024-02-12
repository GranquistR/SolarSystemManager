<template>
  <div>
    <nav class="flex justify-content-end flex-wrap">
      <router-link to="/login">
        <Button label="Log In"></Button>
      </router-link>
       <!-- Put Sign up vue prime button here -->
    </nav>

    <h1>Solar System Manager</h1>
    
    <nav>
      <RouterLink to="/dashboard">dashboard</RouterLink>
      |
      <RouterLink to="/">Landing</RouterLink>
      |
      <RouterLink to="/notfound">notfound</RouterLink>
      |
      <RouterLink to="/settings">settings</RouterLink>
      |
      <RouterLink to="/signup">signup</RouterLink>
      |
      <RouterLink to="/editor">editor</RouterLink>
      |
      <RouterLink to="/unauthorized">unauthorized</RouterLink>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import Button from 'primevue/button'
</script>

<style scoped>
  .right {
    float: right;
  }
</style>
