<script setup>
import typeLog from '@/models/enums/logEnum';
import Dialog from 'primevue/dialog';
import { computed, defineEmits } from 'vue';
import BlockViewerLog from '@/components/BlockViewerLog.vue';


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
            <BlockViewerLog containerClass="px-2 py-2 " :style="{ width: '50vw', height: '70vh' }">
                <template #original>
                    <div v-for="log in logText">
                        {{ typeLog[log.type] }} : {{ log.value }}
                        <span v-if="log.operationName != null"> By {{ log.operationName }}</span>
                    </div>
                </template>

            </BlockViewerLog>
        </Dialog>
    </div>
</template>

<style scoped></style>


  






  

  
