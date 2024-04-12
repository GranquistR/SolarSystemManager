<template>
  <Message class="alertMessage" :severity="severity" :closable="false" v-if="message != ''">{{
    message
  }}</Message>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import Message from 'primevue/message'

const message = ref('')
const severity = ref('')

defineExpose({ ShowMessage })
function ShowMessage(_message: string, _severity: string = 'success') {
  message.value = _message
  severity.value = _severity

  setTimeout(() => {
    message.value = ''
    severity.value = ''
  }, 5000)
}
</script>
<style scoped>
.alertMessage {
  /* appear in top middle of page fixed */
  position: fixed;
  top: 4rem;
  left: 50%;
  transform: translate(-50%, 0);
  z-index: 1000;
}
</style>
