<template>
  <HeaderBar></HeaderBar>
  <ErrorPage
    title="401: Unauthorized"
    message="You are not authorized to view this page"
  ></ErrorPage>
</template>

<script setup lang="ts">
import HeaderBar from '@/components/Header/HeaderBar.vue'
import ErrorPage from '@/components/Error/ErrorPage.vue'
</script>
