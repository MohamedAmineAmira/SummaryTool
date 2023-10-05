<script setup>
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import { ref, computed, defineEmits } from 'vue';
import BlockViewer from '@/components/BlockViewer.vue';


const props = defineProps(['visible', 'text']);
const emit = defineEmits(['close', 'confirm']);

const show = computed({
    get() {
        return props.visible;
    },
    set(newValue) {
        emit('close')
    }
})


</script>
<template>
    <div class="card flex justify-content-center" v-if="show">
        <Dialog v-model:visible="show" modal header=" ">
            <BlockViewer :header="props.text.title" :text="props.text.plainText" :result="props.text.processText"
                containerClass="px-2 py-2 " :style="{ width: '50vw', height: '70vh' }">
                <template #original>
                    <div>{{ props.text.plainText }}</div>
                </template>
                <template #result>
                    <div>{{ props.text.processText }}</div>
                </template>
            </BlockViewer>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
