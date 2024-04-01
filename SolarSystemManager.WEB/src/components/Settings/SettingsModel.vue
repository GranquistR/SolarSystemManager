<template>
    <Button label="paramToEdit" class="mt-3" @click="visible = true"></Button>

    <Dialog v-model:visible="visible" modal header="Edit Profile" :style="{ width: '25rem' }">
        <span class="p-text-secondary block mb-5">Update your information.</span>
        <div class="flex align-items-center gap-3 mb-3">
            <label for="username" class="font-semibold w-6rem">Username</label>
            <InputText variant="filled" id="username" style="width: 245px" v-model="oldUN"/>
        </div>
        <div class="flex align-items-center gap-3 mb-5">
            <label for="password" class="font-semibold w-6rem">Password</label>
            <InputText id="password" class="flex-auto" autocomplete="off" v-model="oldP" />
        </div>
        <div class="flex align-items-center gap-3 mb-3">
            <label for="new username" class="font-semibold w-6rem">New Username</label>
            <InputText id="username" class="flex-auto" autocomplete="off" v-model="newUN"/>
        </div>
        <div class="flex align-items-center gap-3 mb-5">
            <label for="new password" class="font-semibold w-6rem">New Password</label>
            <InputText id="new password" class="flex-auto" autocomplete="off" />
        </div>
        <div class="flex justify-content-end gap-2">
            <Button type="button" label="Cancel" severity="secondary" @click="visible = false"></Button>
            <Button type="button" label="Save" @click="changeParam"></Button>
        </div>
    </Dialog>
</template>

<script setup lang="ts">
import { ref, onMounted, inject } from "vue";
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ChangeUsername from '@/services/LoginService'
import LoginService from "@/services/LoginService";
import ChangeUsernameRequest from '@/Entities/ChangeUsernameRequest'
import InputText from 'primevue/inputtext'
import encrypt from '@/scripts/Encryption/encryption'

const newUN = ref('')
const oldUN = ref('')
const oldP = ref('')

const visible = ref(false)

  const props = defineProps< {
    paramToEdit: string
 }>()

 async function changeParam(){
    if (oldUN.value != '' && oldUN.value != '' && newUN.value != '') {
        let salt = ''
        await LoginService.GetSalt(oldUN.value).then((response) => {
            salt = response.data
        })

        // Encrypt password using fetched salt
        const encryptedPassword = encrypt.encrypt(oldP.value, salt)
        console.log(encryptedPassword)
        LoginService.ChangeUsername( new ChangeUsernameRequest(oldUN.value, encryptedPassword, newUN.value))
    }
    else {
        console.error('Error in LoginService: ', Error)
        alert('Error in LoginService. Check console for details.')
    }
 }

</script>