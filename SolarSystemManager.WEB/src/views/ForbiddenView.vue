<template>
    <HeaderBar></HeaderBar>
    <ErrorPage title="403: Forbidden" message="You do not have proper credentials to view this page"></ErrorPage>
  </template>
  
  <script setup lang="ts">
  import HeaderBar from '@/components/Header/HeaderBar.vue'
  import ErrorPage from '@/components/Error/ErrorPage.vue'
  </script>
  