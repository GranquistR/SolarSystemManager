<template>
    <Button label="paramToEdit" class="mt-3" @click="visible = true"></Button>

    <Dialog v-model:visible="visible" modal header="Edit Profile" :style="{ width: '25rem' }">
        <span class="p-text-secondary block mb-5">Update your information.</span>
        <div class="flex align-items-center gap-3 mb-3">
            <label for="username" class="font-semibold w-6rem">Username</label>
            <InputText id="username" class="flex-auto" autocomplete="off" v-model="newUN"/>
        </div>
        <div class="flex align-items-center gap-3 mb-5">
            <label for="email" class="font-semibold w-6rem">Email</label>
            <InputText id="email" class="flex-auto" autocomplete="off" />
        </div>
        <div class="flex justify-content-end gap-2">
            <Button type="button" label="Cancel" severity="secondary" @click="visible = false"></Button>
            <Button type="button" label="Save" @click="visible = false"></Button>
        </div>
    </Dialog>
</template>

<script setup lang="ts">
import { ref, onMounted, inject } from "vue";
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import User from '@/Entities/User'
import ChangeUsername from '@/services/LoginService'
import LoginService from "@/services/LoginService";
import UserRequest from '@/Entities/UserRequest'

const newUN = ref('')
let user: User | undefined = inject('currentUser')

const visible = ref(false)

  const props = defineProps< {
    paramToEdit: string
 }>()

 function changeParam(){
    if (user?.username != undefined && newUN.value != '') {
        LoginService.ChangeUsername( new UserRequest(user?.username, user?.password), newUN.value)
    }
 }

</script>