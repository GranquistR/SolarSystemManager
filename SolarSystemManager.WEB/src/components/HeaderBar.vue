<template>
  <div>
    <h1>Solar System Manager</h1>
    <nav>
      <RouterLink to="/">Home</RouterLink> |
      <RouterLink to="/login">login</RouterLink>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>
