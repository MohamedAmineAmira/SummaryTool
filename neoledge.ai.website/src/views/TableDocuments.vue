<script setup>
import { ref, onBeforeMount, Teleport } from 'vue';
import { FilterMatchMode } from 'primevue/api';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import status from '@/models/enums/textStateEnum';
import axiosInstance from '@/service/axiosInstance';
import AddDocument from '@/views/AddDocument.vue';
import ShowDocument from '@/views/ShowDocument.vue';
import ShowLog from '@/views/ShowLog.vue';
import 'primeicons/primeicons.css';

const texts = ref([]);
const loading = ref(true);
const visible = ref(false);
const showTextModalVisible = ref(false);
const showLogTextModalVisible = ref(false);
const selectedText = ref({});
const selectedLogText = ref([]);

const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  title: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  type: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  state: { value: null, matchMode: FilterMatchMode.EQUALS },
  priority: { value: null, matchMode: FilterMatchMode.EQUALS }
});

const role = localStorage.getItem('role');
onBeforeMount(() => {
  getDocument()
  visible.value = false
});

function getDocument() {
  if (role == "Admin") {
    getAll()
  } else {
    getDocumentByUser()
  }
}

function getDocumentByUser() {
  loading.value = true;
  axiosInstance.get('api/text/getByUser').then((data) => {
    texts.value = data;
  }).finally(() => {
    loading.value = false;
  })
}

function getAll() {
  loading.value = true;
  axiosInstance.get('api/text').then((data) => {
    texts.value = data;
  }).finally(() => {
    loading.value = false;
  })
}

const getStatus = (index) => {
  return status[index];
}

const getSeverity = (status) => {
  switch (status) {
    case 'Done':
      return 'success';
    case 'Created':
      return 'secondary';
    case 'Error':
      return 'danger';
    default:
      return null;
  }
};

const DisplayModal = () => {
  visible.value = !visible.value;
}

const showText = (id) => {
  selectedText.value = texts.value.find((t) => t.id == id);
  showTextModalVisible.value = true;
}

const showLogText = (id) => {
  axiosInstance.get('api/log/' + id).then((data) => {
    selectedLogText.value = data;
  });
  showLogTextModalVisible.value = true;
}
</script>
<template>
  <div class="card" :style="{ minHeight: '100%', width: '100%' }">
    <h5>Document List</h5>
    <DataTable v-model:filters="filters" :value="texts" :paginator="true" :rows="10" dataKey="id" filterDisplay="menu"
      :loading="loading" :globalFilterFields="['title', 'type', 'state', 'priority']">
      <template #header>
        <div class="flex justify-content-between flex-column sm:flex-row">
          <Button type="button" icon="pi pi-plus" label="Add Document" class="p-button-outlined mb-2"
            @click="DisplayModal()" />
          <span class="p-input-icon-left mb-2">
            <i class="pi pi-search" />
            <InputText v-model="filters['global'].value" placeholder="Keyword Search" style="width: 100%" />
          </span>
        </div>
      </template>
      <template #empty> No customers found. </template>
      <template #loading> Loading customers data. Please wait. </template>
      <Column field="title" header="Title" style="min-width: 10rem" />
      <Column field="type" header="Type" style="min-width: 10rem" />
      <Column field="language" header="Language" style="min-width: 10rem" />

      <Column field="status" header="Status" :showFilterMenu="false" :filterMenuStyle="{ width: '10rem' }"
        style="min-width: 12rem">
        <template #body="{ data }">
          <Tag :value="getStatus(data.state)" :severity="getSeverity(data.state)" />
        </template>
      </Column>
      <Column header="Log" style="min-width: 9rem">
        <template #body="{ data }">
          <div class="col-3">
            <i title="log" class="pi pi-history" style="cursor: pointer; font-size: 1.25rem;"
              @click="showLogText(data.id)" />
          </div>
        </template>
      </Column>
      <Column field="priority" header="Priority" filterField="priority" dataType="numeric" style="min-width: 9rem" />
      <Column header="Options" style="min-width: 9rem">
        <template #body="{ data }">
          <div class="grid formgrid">
            <div class="col-3">
              <i title="show" class="pi pi-eye" style="cursor: pointer; font-size: 1.25rem;" @click="showText(data.id)" />
            </div>
            <div class="col-3">
              <i title="edit" class="pi pi-file-edit" style="cursor: pointer; font-size: 1.25rem;"></i>
            </div>
            <div class="col-3">
              <i title="delete" class="pi pi-trash" style="cursor: pointer; font-size: 1.25rem;"></i>
            </div>
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
  <Teleport to="body">
    <AddDocument :visible="visible" @close="visible = false" @confirm="visible = false; getDocument()" />
    <ShowDocument :text="selectedText" :visible="showTextModalVisible" @close="showTextModalVisible = false" />
    <ShowLog :logText="selectedLogText" :visible="showLogTextModalVisible" @close="showLogTextModalVisible = false" />
  </Teleport>
</template>



<style>
.p-datatable-wrapper {
  height: 80%;
}
</style>