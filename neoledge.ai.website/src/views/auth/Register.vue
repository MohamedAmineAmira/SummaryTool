<script setup>
import { ref, computed } from 'vue';

import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';
import axiosInstance from '@/service/axiosInstance';
import router from '../../router';

const register = ref({
    userName: null,
    email: null,
    password: null
})
const errorUserName = ref(false);
const errorEmail = ref(false);
const errorPassword = ref(false);
const error = ref(null);

async function Register() {

    axiosInstance.post('api/Auth/Register', register.value).then((data) => {
        if (data.succeeded) {
            router.push({ path: '/' })
        } else {
            if ((data.errors[0]['code']).includes('UserName')) {
                errorUserName.value = true;
                error.value = data.errors[0]['code'];
            }
            if ((data.errors[0]['code']).includes('Email')) {
                errorEmail.value = true;
                error.value = data.errors[0]['code'];
            }
            if ((data.errors[0]['code']).includes('Password')) {
                errorPassword.value = true;
                error.value = data.errors[0]['description'];
            }
        }
    });

}


const isRegisterDisabled = computed(() => {
    return !register.value.email || !register.value.password || !register.value.userName;
});

</script>

<template>
    <div class="surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="flex flex-column align-items-center justify-content-center">

            <div
                style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                <div class="w-full surface-card py-8 px-5 sm:px-8" style="border-radius: 53px">
                    <div class="text-center mb-6">
                        <div class="text-900 text-3xl font-medium mb-3">Welcome, To Summary Tool</div>
                    </div>
                    <div>
                        <div class="row mb-4">
                            <label class="block text-900 text-xl font-medium mb-2">UserName</label>
                            <InputText type="text" placeholder="Username" class="w-full md:w-30rem mb-2"
                                style="padding: 1rem" v-model="register.userName"
                                :class="{ 'p-invalid': errorUserName == true }" @input="errorUserName = false" />
                            <small :style="{ 'visibility': errorUserName ? 'visible' : 'hidden' }" class="p-error block"> {{
                                error }}
                            </small>

                        </div>
                        <div class="row mb-4">
                            <label class="block text-900 text-xl font-medium mb-2">Email</label>
                            <InputText type="text" placeholder="Email address" class="w-full md:w-30rem mb-2 "
                                style="padding: 1rem" v-model="register.email" :class="{ 'p-invalid': errorEmail == true }"
                                @input="errorEmail = false" />
                            <small :style="{ 'visibility': errorEmail ? 'visible' : 'hidden' }" class="p-error block">{{
                                error }}
                            </small>
                        </div>
                        <div class="row mb-5">
                            <label class="block text-900 font-medium text-xl mb-2">Password</label>
                            <Password v-model="register.password" placeholder="Password" :toggleMask="true" class="w-full"
                                inputClass="w-full mb-2" :inputStyle="{ padding: '1rem' }"
                                :class="{ 'p-invalid': errorPassword == true }" @input="errorPassword = false"></Password>
                            <small :style="{ 'visibility': errorPassword ? 'visible' : 'hidden' }" class="p-error">{{ error
                            }}</small>
                        </div>
                        <Button :disabled="isRegisterDisabled" label="Register" class="w-full p-3 text-xl"
                            @click="Register()"></Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.pi-eye {
    transform: scale(1.6);
    margin-right: 1rem;
}

.pi-eye-slash {
    transform: scale(1.6);
    margin-right: 1rem;
}
</style>
