<script setup>
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import { ref, computed, defineEmits } from 'vue';
import axiosInstance from '@/service/axiosInstance';

const props = defineProps(['visible', 'dataPreprocessor']);
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

function EditDataPreprocessor() {

    if (!props.dataPreprocessor.name) {
        nameError.value = true;
        return;
    }

    if (!props.dataPreprocessor.language) {
        languageError.value = true;
        return;
    }

    if (!props.dataPreprocessor.url) {
        urlError.value = true;
        return;
    }
    dataPreprocessor.value.name = props.dataPreprocessor.name;
    dataPreprocessor.value.isActive = props.dataPreprocessor.isActive;
    dataPreprocessor.value.url = props.dataPreprocessor.url;
    dataPreprocessor.value.language = props.dataPreprocessor.language;

    axiosInstance.put('api/dataPreprocessor/' + props.dataPreprocessor.id, dataPreprocessor.value).then(() => {
        emit("confirm");
    });
}


</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header="Data Preprocessor" :style="{ width: '30vw' }">
            <h5>Name</h5>
            <InputText v-model="props.dataPreprocessor.name" type="text" :required="true"
                placeholder="Name of Data Preprocessor" aria-describedby="username-help"
                :class="{ 'p-invalid': nameError == true }" @input="nameError = false" style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': nameError ? 'visible' : 'hidden' }" class="p-error">Name is
                required.</small>
            <br>
            <h5>Language</h5>
            <Dropdown v-model="props.dataPreprocessor.language" :options="dropdownLanguages"
                :class="{ 'p-invalid': languageError == true }" @change="languageError = false"
                placeholder="-- Select language --" style="width: 100%;" :required="true" />
            <small id="username-help" :style="{ 'visibility': languageError ? 'visible' : 'hidden' }"
                class="p-error">Language is required</small>
            <h5>Url</h5>
            <InputText v-model="props.dataPreprocessor.url" type="text" :required="true"
                placeholder="Url of Data Preprocessor" aria-describedby="username-help"
                :class="{ 'p-invalid': urlError == true }" @input="urlError = false" style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': urlError ? 'visible' : 'hidden' }" class="p-error">Title is
                required.</small>
            <template #footer>

                <Button label="Save & Submit" class="p-button mr-3" @click="EditDataPreprocessor"></Button>
                <Button label="Cancel" class="p-button-secondary" @click="$emit('close')"></Button>
            </template>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
