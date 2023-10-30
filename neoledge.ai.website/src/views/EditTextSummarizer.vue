<script setup>
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import { ref, computed, defineEmits } from 'vue';
import axiosInstance from '@/service/axiosInstance';

const props = defineProps(['visible', 'textProcessor']);
const emit = defineEmits(['close', 'confirm']);
const dropdownLanguages = ref(['English', 'French']);
const textProcessor = ref({
    Name: null,
    Language: null,
    Url: null,
    IsActive: false,
    ToolboxId: null
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

function EditTextProcessor() {

    if (!props.textProcessor.Name) {
        nameError.value = true;
        return;
    }

    if (!props.textProcessor.Language) {
        languageError.value = true;
        return;
    }

    if (!props.textProcessor.Url) {
        urlError.value = true;
        return;
    }

    textProcessor.value.Name = props.textProcessor.Name;
    textProcessor.value.IsActive = props.textProcessor.IsActive;
    textProcessor.value.ToolboxId = props.textProcessor.Toolbox.Id;
    textProcessor.value.Url = props.textProcessor.Url;
    textProcessor.value.Language = props.textProcessor.Language;
    console.log(textProcessor.value)
    axiosInstance.put('api/textProcessor/' + props.textProcessor.Id, textProcessor.value).then(() => {
        emit("confirm");
    });
}

</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header="Text Processor" :style="{ width: '30vw' }">
            <h5>Name</h5>
            <InputText v-model="props.textProcessor.Name" type="text" :required="true" placeholder="Name of text Processor"
                aria-describedby="username-help" :class="{ 'p-invalid': nameError == true }" @input="nameError = false"
                style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': nameError ? 'visible' : 'hidden' }" class="p-error">Name is
                required.</small>
            <br>
            <h5>Language</h5>
            <Dropdown v-model="props.textProcessor.Language" :options="dropdownLanguages"
                :class="{ 'p-invalid': languageError == true }" @change="languageError = false"
                placeholder="-- Select language --" style="width: 100%;" :required="true" />
            <small id="username-help" :style="{ 'visibility': languageError ? 'visible' : 'hidden' }"
                class="p-error">Language is required</small>
            <h5>Url</h5>
            <InputText v-model="props.textProcessor.Url" type="text" :required="true" placeholder="Url of Data Preprocessor"
                aria-describedby="username-help" :class="{ 'p-invalid': urlError == true }" @input="urlError = false"
                style="width: 100%;" />
            <small id="username-help" :style="{ 'visibility': urlError ? 'visible' : 'hidden' }" class="p-error">Url is
                required.</small>
            <template #footer>

                <Button label="Save & Submit" class="p-button mr-3" @click="EditTextProcessor"></Button>
                <Button label="Cancel" class="p-button-secondary" @click="$emit('close')"></Button>
            </template>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
