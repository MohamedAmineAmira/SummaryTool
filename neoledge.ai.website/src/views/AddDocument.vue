<script setup>
import { ref, computed, defineEmits } from 'vue';

import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import Textarea from 'primevue/textarea';
import InputNumber from 'primevue/inputnumber';
import FileUpload from 'primevue/fileupload';

import axiosInstance from '@/service/axiosInstance';

const props = defineProps(['visible']);
const emit = defineEmits(['close', 'confirm']);
const dropdownTypes = ref(['txt', 'pdf', 'doc']);
const dropdownLanguages = ref(['English', 'French']);
const document = ref({
    title: null,
    type: null,
    language: null,
    priority: null,
    plainText: null,
    createdDATE: null
})
const titleError = ref(false);
const typeError = ref(false);
const languageError = ref(false);
const priorityError = ref(false);
const plainTextError = ref(false);
const key = ref(0)

const show = computed({
    get() {
        return props.visible;
    },
    set(newValue) {
        emit('close')
    }
})

function resumeText() {
    if (!document.value.title) {
        titleError.value = true;
        return;
    }

    if (!document.value.type) {
        typeError.value = true;
        return;
    }

    if (!document.value.language) {
        languageError.value = true;
        return;
    }

    if (!document.value.priority) {
        priorityError.value = true;
        return;
    }

    if (!document.value.plainText) {
        plainTextError.value = true;
        return;
    }

    // If all fields are filled, proceed with resumeText logic
    document.value.createdDATE = new Date().toLocaleString();
    console.log(document.value);
    axiosInstance.post('api/text', document.value).then(() => {
        resetData();
        emit("confirm");
    });
}

function resetData() {
    document.value.title = null;
    document.value.type = null;
    document.value.language = null;
    document.value.priority = null;
    document.value.plainText = null;

    // Reset error variables
    titleError.value = false;
    typeError.value = false;
    languageError.value = false;
    priorityError.value = false;
    plainTextError.value = false;
}

function onUpload(file) {
    const formData = new FormData();
    formData.append('pdfFile', file.files[0]);
    axiosInstance.post('api/text/uploadFile', formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        }
    }).then((data) => {
        document.value.plainText = data;
        key.value += 1;
    }).catch((error) => {
        console.error('Error uploading file', error);
    });
}

</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header="Summarizer" :style="{ width: '50vw' }">
            <div class="grid formgrid">
                <div class="col-6 col-sm-12">
                    <h5>Title</h5>
                    <InputText v-model="document.title" type="text" :required="true" placeholder="Title of Text"
                        aria-describedby="username-help" :class="{ 'p-invalid': titleError == true }"
                        @input="titleError = false" style="width: 100%;" />
                    <small id="username-help" :style="{ 'visibility': titleError ? 'visible' : 'hidden' }"
                        class="p-error">Title is required.</small>
                </div>
                <div class="col-6 col-sm-12">
                    <h5>Type</h5>
                    <Dropdown v-model="document.type" :options="dropdownTypes" placeholder="-- Select Type --"
                        :class="{ 'p-invalid': typeError == true }" @change="typeError = false" style="width: 100%;"
                        :required="true" />
                    <small id="username-help" :style="{ 'visibility': typeError ? 'visible' : 'hidden' }"
                        class="p-error">Type is required.</small>
                </div>
            </div>
            <br>
            <div class="grid formgrid">
                <div class="col-6 col-sm-12">
                    <h5>Language</h5>
                    <Dropdown v-model="document.language" :options="dropdownLanguages"
                        :class="{ 'p-invalid': languageError == true }" @change="languageError = false"
                        placeholder="-- Select language --" style="width: 100%;" :required="true" />
                    <small id="username-help" :style="{ 'visibility': languageError ? 'visible' : 'hidden' }"
                        class="p-error">Language is required</small>
                </div>
                <div class="col-6 col-sm-12">
                    <h5> Priority </h5>
                    <InputNumber v-model="document.priority" mode="decimal" showButtons :min="0" :max="100"
                        :class="{ 'p-invalid': priorityError == true }" @input="priorityError = false" style="width: 100%;"
                        :required="true" />
                    <small id="username-help" :style="{ 'visibility': priorityError ? 'visible' : 'hidden' }"
                        class="p-error">Priority is required</small>
                </div>
            </div>
            <div class="grid formgrid textarea-block">
                <h5>Text</h5>
                <div class="upload-file-textarea">
                    <FileUpload mode="basic" name="demo" accept=".pdf" @select="onUpload" :auto="false" choose-label=" "
                        choose-icon="pi pi-spin pi-spinner" cancel-icon="pi pi-upload" :key="key" />
                </div>

                <Textarea v-model="document.plainText" class="responsive-textarea" style="font-size: 16px;"
                    :class="{ 'p-invalid': plainTextError == true }" @input="plainTextError = false"
                    placeholder="Text To Resume" :required="true" />
                <small id="username-help" :style="{ 'visibility': plainTextError ? 'visible' : 'hidden' }"
                    class="p-error">Text
                    is required</small>
            </div>
            <template #footer>
                <Button icon="pi pi-delete-left" label="Cancel" class="p-button-secondary"
                    @click="$emit('close'), resetData()"></Button>
                <Button icon="pi pi-align-left" label="Resume" class="p-button mr-3" @click="resumeText"></Button>
            </template>
        </Dialog>
    </div>
</template>

  
<style scoped lang="scss">
.responsive-textarea {
    width: 100%;
    resize: vertical;
    min-height: 200px;
    text-align: justify;
}

.textarea-block {
    position: relative;
}

.upload-file-textarea {
    position: absolute !important;
    top: -5px;
    right: 0px;
}
</style>
  

  






  

  
