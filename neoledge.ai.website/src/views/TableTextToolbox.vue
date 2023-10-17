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

const textAnalytics = ref([]);
const loading = ref(true);
const tool = ref({
    name: null,
    IsActive: false
});
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
});

onBeforeMount(() => {
    getAll()
});

function getAll() {
    loading.value = true;
    axiosInstance.get('/api/textAnalyticsToolbox').then((data) => {
        textAnalytics.value = data;
    }).finally(() => {
        loading.value = false;
    })
}
function editTextAnalytic(textAnalytic) {
    axiosInstance.put('/api/textAnalyticsToolbox/' + textAnalytic.Id, textAnalytic)
        .then((response) => {
            console.log(response.data);
        })
        .catch((error) => {
            console.error('Error editing data preprocessor:', error);
        });
}
function changeIsActive(row) {
    editTextAnalytic(row);
    if (row.IsActive) {
        textAnalytics.value.forEach(element => {
            if (element.Id != row.Id) {
                element.IsActive = false;
                editTextAnalytic(element);
            }
        });
    }
}
function AddTool() {
    axiosInstance.post('api/textAnalyticsToolbox', tool.value).then(() => {
        tool.value.name = null;
        getAll();
    });
}
</script>
<template>
    <div class="card" :style="{ minHeight: '100%', width: '100%' }">
        <h5>List of Text Analytices Tools</h5>
        <DataTable v-model:filters="filters" :value="textAnalytics" :paginator="true" :rows="10" dataKey="id"
            filterDisplay="menu" :loading="loading" :globalFilterFields="['name']">
            <template #header>
                <div class="flex justify-content-between flex-column sm:flex-row">
                    <div class="p-inputgroup  mb-2" style="width: 20vw;">
                        <Button icon="pi pi-plus" @click="AddTool()" />
                        <InputText v-model="tool.name" placeholder="Add New Tool" />
                    </div>
                    <span class="p-input-icon-left mb-2">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Keyword Search" style="width: 100%" />
                    </span>
                </div>
            </template>
            <template #empty> No customers found. </template>
            <template #loading> Loading customers data. Please wait. </template>
            <Column field="Name" header="Name" style="min-width: 7rem" />
            <Column header="IsActive" style="min-width: 10rem">
                <template #body="{ data }">
                    <InputSwitch v-model="data.IsActive" @change="changeIsActive(data)" />
                </template>
            </Column>
            <Column header="Options" style="min-width: 9rem">
                <template #body="{ data }">
                    <div class="grid formgrid">
                        <div class="col-1">
                            <i title="edit" class="pi pi-pencil" style="cursor: pointer; font-size: 1.25rem;" />
                        </div>
                        <div class="col-1">
                            <i title="delete" class="pi pi-trash" style="cursor: pointer; font-size: 1.25rem;" />
                        </div>
                    </div>
                </template>
            </Column>
        </DataTable>
    </div>
</template>
<style>
.p-datatable-wrapper {
    height: 80%;
}
</style>
