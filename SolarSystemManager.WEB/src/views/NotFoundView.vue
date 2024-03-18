<template>
  <HeaderBar></HeaderBar>
  <ErrorPage title="404: Not Found" message="Page not Found"></ErrorPage>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import ErrorPage from '@/components/Error/ErrorPage.vue'
</script>
