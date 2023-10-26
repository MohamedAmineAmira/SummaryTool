<script setup>
import typeLog from '@/models/enums/logEnum.js';
import Dialog from 'primevue/dialog';
import { computed, defineEmits } from 'vue';
import BlockViewer from '@/components/BlockViewer.vue';


const props = defineProps(['visible', 'logText']);
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
            <BlockViewer containerClass="px-2 py-2 " :style="{ width: '50vw', height: '70vh' }">
                <template #original>
                    <div></div>
                </template>

            </BlockViewer>
        </Dialog>
    </div>
</template>

<style scoped></style>
  

  






  

  
