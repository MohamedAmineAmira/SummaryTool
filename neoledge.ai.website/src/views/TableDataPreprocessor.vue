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
import AddDataPreprocessor from './AddDataPreprocessor.vue';

const dataPreprocessors = ref([]);
const loading = ref(true);
const visible = ref(false);
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
  axiosInstance.get('/api/dataPreprocessor').then((data) => {
    dataPreprocessors.value = data;
  }).finally(() => {
    loading.value = false;
  })
}
function editDataPreprocessor(dataPreprocessor) {
  axiosInstance.put('api/dataPreprocessor/' + dataPreprocessor.id, dataPreprocessor)
    .then((response) => {
      console.log(response.data);
    })
    .catch((error) => {
      console.error('Error editing data preprocessor:', error);
    });
}
function changeIsActive(row) {
  editDataPreprocessor(row);
  if (row.isActive) {
    dataPreprocessors.value.forEach(element => {
      if (element.language == row.language && element.id != row.id) {
        element.isActive = false;
        editDataPreprocessor(element);
      }
    });
  }
}
const DisplayModal = () => {
  visible.value = !visible.value;
}
</script>
<template>
  <div class="card" :style="{ minHeight: '100%', width: '100%' }">
    <h5>List of Data Preprocessing Tools</h5>
    <DataTable v-model:filters="filters" :value="dataPreprocessors" :paginator="true" :rows="10" dataKey="id"
      filterDisplay="menu" :loading="loading" :globalFilterFields="['name', 'language', 'url']">
      <template #header>
        <div class="flex justify-content-between flex-column sm:flex-row">
          <Button type="button" icon="pi pi-plus" label="Add Data Preprocessor Tool" class="p-button-outlined mb-2"
            @click="DisplayModal()" />
          <span class="p-input-icon-left mb-2">
            <i class="pi pi-search" />
            <InputText v-model="filters['global'].value" placeholder="Keyword Search" style="width: 100%" />
          </span>
        </div>
      </template>
      <template #empty> No customers found. </template>
      <template #loading> Loading customers data. Please wait. </template>
      <Column field="name" header="Name" style="min-width: 7rem" />
      <Column field="language" header="Language" style="min-width: 8rem" />
      <Column field="url" header="Url" style="min-width: 10rem" />
      <Column header="IsActive" style="min-width: 10rem">
        <template #body="{ data }">
          <InputSwitch v-model="data.isActive" @change="changeIsActive(data)" />
        </template>
      </Column>
      <Column header="Options" style="min-width: 9rem">
        <template #body="{ data }">
          <div class="grid formgrid">
            <div class="col-3">
              <i title="edit" class="pi pi-pencil" style="cursor: pointer; font-size: 1.25rem;" />
            </div>
            <div class="col-3">
              <i title="delete" class="pi pi-trash" style="cursor: pointer; font-size: 1.25rem;" />
            </div>
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
  <Teleport to="body">
    <AddDataPreprocessor :visible="visible" @close="visible = false" @confirm="visible = false; getAll()" />
  </Teleport>
</template>
<style>
.p-datatable-wrapper {
  height: 80%;
}
</style>
