<template>
  <Button
    type="button"
    icon="pi pi-sliders-h"
    rounded
    outlined
    :label="paramToEdit"
    @click="visible = true"
  ></Button>

  <Dialog v-model:visible="visible" modal header="Edit Profile" :style="{ width: '25rem' }">
    <span class="p-text-secondary block mb-5">Update your information.</span>
    <div class="flex align-items-center gap-3 mb-3">
      <label for="username" class="font-semibold w-6rem">Username</label>
      <InputText id="username" variant="filled" autocomplete="off" v-model="oldUN" />
    </div>
    <div class="flex align-items-center gap-3 mb-5">
      <label for="password" class="font-semibold w-6rem">Password</label>
      <Password variant="filled" id="password" v-model="oldP" :feedback="false" />
    </div>
    <div class="flex align-items-center gap-3 mb-3" v-if:visible="!passVisible">
      <label for="new username" class="font-semibold w-6rem">New Username</label>
      <InputText id="username" variant="filled" autocomplete="off" v-model="newUN" />
    </div>
    <div class="flex align-items-center gap-3 mb-5" v-if:visible="passVisible">
      <label for="new password" class="font-semibold w-6rem">New Password</label>
      <Password variant="filled" id="password" v-model="newUN" :feedback="true" />
    </div>
    <div class="flex justify-content-end gap-2">
      <Button
        type="button"
        rounded
        outlined
        label="Cancel"
        severity="secondary"
        @click="visible = false"
      ></Button>
      <Button type="button" rounded outlined label="Save" @click="changeParam"></Button>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ChangeUsername from '@/services/LoginService'
import LoginService from '@/services/LoginService'
import ChangeCredRequest from '@/Entities/ChangeCredRequest'
import InputText from 'primevue/inputtext'
import encrypt from '@/scripts/Encryption/encryption'
import Password from 'primevue/password'

const newUN = ref('')
const oldUN = ref('')
const oldP = ref('')

const visible = ref(false)
let passVisible = false

onMounted(() => {
  passVisible = props.paramToEdit == 'Change Password'
})

const props = defineProps<{
  paramToEdit: string
}>()

async function changeParam() {
  if (oldUN.value != '' && oldUN.value != '') {
    let salt = ''
    await LoginService.GetSalt(oldUN.value).then((response) => {
      salt = response.data
    })

    // Encrypt password using fetched salt
    const encryptedPassword = encrypt.encrypt(oldP.value, salt)
    if (passVisible) {
      const newEncryptedPassword = encrypt.encrypt(newUN.value, salt)
      LoginService.ChangePassword(
        new ChangeCredRequest(oldUN.value, encryptedPassword, newEncryptedPassword)
      )
    } else {
      LoginService.ChangeUsername(
        new ChangeCredRequest(oldUN.value, encryptedPassword, newUN.value)
      )
    }
    visible.value = false
  } else {
    console.error('Error in LoginService: ', Error)
    alert('Error in LoginService. Check console for details.')
  }
  window.location.pathname = '/login'
}
</script>
@/Entities/ChangeCredRequest
