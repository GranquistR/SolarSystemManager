<template>
  <div>
    <h1>Solar System Manager</h1>
    <nav>
      <RouterLink to="/dashboard">dashboard</RouterLink>
      |
      <RouterLink to="/">Landing</RouterLink>
      |
      <RouterLink to="/login">login</RouterLink>
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
</script>
