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
const visible = ref(false);
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
});

onBeforeMount(() => {
    getAll()
    visible.value = false
});

function getAll() {
    loading.value = true;
    axiosInstance.get('/api/textAnalyticsToolbox').then((data) => {
        textAnalytics.value = data;
        console.log(textAnalytics.value);
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
</script>
<template>
    <div class="card" :style="{ minHeight: '100%', width: '100%' }">
        <h5>List of Text Analytices Tools</h5>
        <DataTable v-model:filters="filters" :value="textAnalytics" :paginator="true" :rows="10" dataKey="id"
            filterDisplay="menu" :loading="loading" :globalFilterFields="['name']">
            <template #header>
                <div class="flex justify-content-between flex-column sm:flex-row">
                    <Button type="button" icon="pi pi-plus" label="Add Text Analytice Tool" class="p-button-outlined mb-2"
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
