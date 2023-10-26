<script setup>
import { ref, computed } from 'vue';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Checkbox from 'primevue/checkbox';
import Button from 'primevue/button';
import axiosInstance from '@/service/axiosInstance';
import router from '../../router';


const login = ref({
    email: null,
    password: null
})
const errorEmail = ref(false);
const errorPassword = ref(false);
const checked = ref(false);

async function Login() {

    console.log(login.value);
    axiosInstance.post('api/Auth/Login', login.value).then((data) => {
        if (data == 'Email Not Exists') {
            errorEmail.value = true;
        } else if (data == 'Password Wrong') {
            errorPassword.value = true;
        } else {
            PutRoleInLocalStorage();
            localStorage.setItem('token', data);
            router.push({ path: 'Acount/documents' })
        }
    });
}

function PutRoleInLocalStorage() {
    axiosInstance.get('api/Auth/role').then((data) => {
        localStorage.setItem('role', data);
    }).then(() => {
        location.reload()
    })
}

function Register() {
    router.push({ path: 'Register' })
}
const isSignInDisabled = computed(() => {
    return !login.value.email || !login.value.password;
});

</script>

<template>
    <div class="surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="flex flex-column align-items-center justify-content-center">

            <div
                style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                <div class="w-full surface-card py-8 px-5 sm:px-8" style="border-radius: 53px">
                    <div class="text-center mb-5">
                        <div class="text-900 text-3xl font-medium mb-3">Welcome, To Summary Tool</div>
                        <span class="text-600 font-medium">Sign in to continue</span>
                    </div>
                    <div>
                        <div class="row mb-4">
                            <label class="block text-900 text-xl font-medium mb-2">Email</label>
                            <InputText type="text" placeholder="Email address" class="w-full md:w-30rem mb-2"
                                style="padding: 1rem" v-model="login.email" :class="{ 'p-invalid': errorEmail == true }"
                                @input="errorEmail = false" />
                            <small :style="{ 'visibility': errorEmail ? 'visible' : 'hidden' }" class="p-error block ">Email
                                Not
                                Exists</small>
                        </div>
                        <div class="row mb-4">
                            <label class="block text-900 font-medium text-xl mb-2">Password</label>
                            <Password v-model="login.password" placeholder="Password" :toggleMask="true" class="w-full mb-2"
                                inputClass="w-full" :inputStyle="{ padding: '1rem' }"
                                :class="{ 'p-invalid': errorPassword == true }" @input="errorPassword = false"
                                :feedback="false"></Password>
                            <small :style="{ 'visibility': errorPassword ? 'visible' : 'hidden' }" class="p-error"> Password
                                Wrong</small>
                        </div>
                        <div class="flex align-items-center justify-content-between mb-5 gap-5">
                            <div class="flex align-items-center">
                                <Checkbox v-model="checked" binary class="mr-2"></Checkbox>
                                <label for="rememberme1">Remember me</label>
                            </div>
                            <a class="font-medium no-underline ml-2 text-right cursor-pointer"
                                style="color: var(--primary-color)" @click="Register()">New Account</a>
                        </div>
                        <Button :disabled="isSignInDisabled" label="Sign In" class="w-full p-3 text-xl"
                            @click="Login()"></Button>
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
