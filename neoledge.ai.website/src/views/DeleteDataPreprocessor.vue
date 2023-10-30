<script setup>
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import { ref, computed, defineEmits } from 'vue';
import axiosInstance from '@/service/axiosInstance';

const props = defineProps(['id', 'visible']);
const emit = defineEmits(['close', 'confirm']);


const show = computed({
    get() {
        return props.visible;
    },
    set(newValue) {
        emit('close')
    }
})
function deleteText() {
    axiosInstance.delete('api/dataPreprocessor/' + props.id).then(() => {
        emit("confirm");
    });

}



</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header="Delete Data Preprocessor Tool ?" :style="{ width: '30vw' }">
            <h6>
                Are you sure you want to delete this tool ?
            </h6>
            <template #footer>
                <Button label="Delete" class="p-button-danger mr-3" @click="deleteText"></Button>
                <Button label="Cancel" class="p-button-secondary" @click="$emit('close')"></Button>
            </template>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
