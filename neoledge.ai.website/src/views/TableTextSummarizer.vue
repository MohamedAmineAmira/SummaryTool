<script setup>
import { ref, onBeforeMount, Teleport } from 'vue';
import { FilterMatchMode } from 'primevue/api';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import InputSwitch from 'primevue/inputswitch';
import axiosInstance from '@/service/axiosInstance';
import 'primeicons/primeicons.css';
import AddTextSummarizer from './AddTextSummarizer.vue';
import DeleteTextSummarizer from './DeleteTextSummarizer.vue';
import EditTextSummarizer from './EditTextSummarizer.vue';

const textProcessors = ref([]);
const loading = ref(true);
const visible = ref(false);
const visibleDeleteModal = ref(false);
const textProcessor = ref(null);
const visibleEditModal = ref(false);
const idDeletedTextSummarizer = ref(null);
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
    language: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
    url: { value: null, matchMode: FilterMatchMode.EQUALS },
});

onBeforeMount(() => {
    getAll()
    visible.value = false
});

function getAll() {
    loading.value = true;
    axiosInstance.get('/api/textProcessor').then((data) => {
        textProcessors.value = data;
    }).finally(() => {
        loading.value = false;
    })
}
function editTextProcessor(id, textProcessor) {
    axiosInstance.put('/api/textProcessor/' + id, textProcessor)
        .then((response) => {
            console.log(response.data);
        })
        .catch((error) => {
            console.error('Error editing data preprocessor:', error);
        });
}
function prepareTextProcessor(element) {
    return {
        "name": element.Name,
        "language": element.Language,
        "isActive": element.IsActive,
        "url": element.Url,
        "toolboxId": element.Toolbox.Id
    }
}
function changeIsActive(row) {
    editTextProcessor(row.Id, prepareTextProcessor(row));
    if (row.IsActive) {
        textProcessors.value.forEach(element => {
            if (element.Language == row.Language && element.Id != row.Id) {
                element.IsActive = false;
                editTextProcessor(element.Id, prepareTextProcessor(element));
            }
        });
    }
}
const DisplayModal = () => {
    visible.value = !visible.value;
}
function Delete(id) {
    idDeletedTextSummarizer.value = id;
    visibleDeleteModal.value = true;
}
function Edit(row) {
    textProcessor.value = row;
    visibleEditModal.value = true;
}
</script>
<template>
    <div class="card" :style="{ minHeight: '100%', width: '100%' }">
        <h5>List of Summarizer Tools</h5>
        <DataTable v-model:filters="filters" :value="textProcessors" :paginator="true" :rows="10" dataKey="id"
            filterDisplay="menu" :loading="loading" :globalFilterFields="['name', 'language', 'url']">
            <template #header>
                <div class="flex justify-content-between flex-column sm:flex-row">
                    <Button type="button" icon="pi pi-plus" label="Add Summarizer Tool" class="p-button-outlined mb-2"
                        @click="DisplayModal()" />
                    <span class="p-input-icon-left mb-2">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" style="width: 100%" />
                    </span>
                </div>
            </template>
            <template #empty> No customers found. </template>
            <template #loading> Loading customers data. Please wait. </template>
            <Column field="Name" header="Name" style="min-width: 7rem" />
            <Column field="Language" header="Language" style="min-width: 8rem" />
            <Column field="Url" header="Url" style="min-width: 10rem" />
            <Column header="IsActive" style="min-width: 10rem">
                <template #body="{ data }">
                    <InputSwitch v-model="data.IsActive" @change="changeIsActive(data)" />
                </template>
            </Column>
            <Column header="Options" style="min-width: 9rem">
                <template #body="{ data }">
                    <div class="grid formgrid">
                        <div class="col-3">
                            <i title="edit" class="pi pi-file-edit" style="cursor: pointer; font-size: 1.25rem;"
                                @click="Edit(data)" />
                        </div>
                        <div class="col-3">
                            <i title="delete" class="pi pi-trash" style="cursor: pointer; font-size: 1.25rem;"
                                @click="Delete(data.Id)" />
                        </div>
                    </div>
                </template>
            </Column>
        </DataTable>
    </div>
    <Teleport to="body">
        <AddTextSummarizer :visible="visible" @close="visible = false" @confirm="visible = false; getAll()" />
        <EditTextSummarizer :visible="visibleEditModal" :textProcessor="textProcessor" @close="visibleEditModal = false"
            @confirm="visibleEditModal = false; getAll()" />
        <DeleteTextSummarizer :visible="visibleDeleteModal" :id="idDeletedTextSummarizer"
            @close="visibleDeleteModal = false" @confirm="visibleDeleteModal = false; getAll()" />
    </Teleport>
</template>
<style>
.p-datatable-wrapper {
    height: 80%;
}
</style>
