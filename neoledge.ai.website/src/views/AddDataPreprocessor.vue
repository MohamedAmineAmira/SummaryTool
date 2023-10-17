<script setup>
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import { ref, computed, defineEmits } from 'vue';
import axiosInstance from '@/service/axiosInstance';

const props = defineProps(['visible']);
const emit = defineEmits(['close', 'confirm']);
const dropdownLanguages = ref(['English', 'French']);
const dataPreprocessor = ref({
    name: null,
    language: null,
    url: null,
    isActive: false
})

const nameError = ref(false);
const languageError = ref(false);
const urlError = ref(false);

const show = computed({
    get() {
        return props.visible;
    },
    set(newValue) {
        emit('close')
    }
})

function addDataPreprocessor() {

    if (!dataPreprocessor.value.name) {
        nameError.value = true;
        return;
    }

    if (!dataPreprocessor.value.language) {
        languageError.value = true;
        return;
    }

    if (!dataPreprocessor.value.url) {
        urlError.value = true;
        return;
    }
    console.log(dataPreprocessor.value);
    // If all fields are filled, proceed with resumeText logic
    axiosInstance.post('api/dataPreprocessor', dataPreprocessor.value).then(() => {
        resetData();
        emit("confirm");
    });
}

function resetData() {
    dataPreprocessor.value.name = null;
    dataPreprocessor.value.language = null;
    dataPreprocessor.value.url = null;

    // Reset error variables
    nameError.value = false;
    languageError.value = false;
    urlError.value = false;
}
</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header="Data Preprocessor" :style="{ width: '30vw' }">
            <h5>Name</h5>
            <InputText v-model="dataPreprocessor.name" type="text" :required="true" placeholder="Name of Data Preprocessor"
                aria-describedby="username-help" :class="{ 'p-invalid': nameError == true }" @input="nameError = false"
                style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': nameError ? 'visible' : 'hidden' }" class="p-error">Name is
                required.</small>
            <br>
            <h5>Language</h5>
            <Dropdown v-model="dataPreprocessor.language" :options="dropdownLanguages"
                :class="{ 'p-invalid': languageError == true }" @change="languageError = false"
                placeholder="-- Select language --" style="width: 100%;" :required="true" />
            <small id="username-help" :style="{ 'visibility': languageError ? 'visible' : 'hidden' }"
                class="p-error">Language is required</small>
            <h5>Url</h5>
            <InputText v-model="dataPreprocessor.url" type="text" :required="true" placeholder="Url of Data Preprocessor"
                aria-describedby="username-help" :class="{ 'p-invalid': urlError == true }" @input="urlError = false"
                style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': urlError ? 'visible' : 'hidden' }" class="p-error">Title is
                required.</small>
            <template #footer>
                <Button icon="pi pi-plus" label=" Add " class="p-button mr-3" @click="addDataPreprocessor"></Button>
                <Button icon="pi pi-delete-left" label="Cancel" class="p-button-secondary"
                    @click="$emit('close'), resetData()"></Button>
            </template>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
