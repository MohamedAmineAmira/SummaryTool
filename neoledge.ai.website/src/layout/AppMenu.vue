<script setup>
import { ref, computed } from 'vue';

import AppMenuItem from './AppMenuItem.vue';

const role = localStorage.getItem('role');
console.log(role)

const modelAdmin = ref([
    {
        label: 'Home',
        items: [
            { label: 'Documents', icon: 'pi pi-fw pi-list', to: '/Acount/documents' },
            { label: 'Dashboard', icon: 'pi pi-chart-bar', to: '/Acount/dashboard' }
        ]
    },
    {
        label: 'Settings Hub',
        items: [
            { label: 'Data Preprocessor', icon: 'pi pi-eraser', to: '/Acount/dataPreprocessor' },
            { label: 'Text Analytics Toolbox', icon: 'pi pi-cog', to: '/Acount/textAnalyticsToolbox' },
            { label: 'Text Summarizer', icon: 'pi pi-align-left', to: '/Acount/textSummarizer' }
        ]
    }
]);

const modelUser = ref([
    {
        label: 'Home',
        items: [
            { label: 'Documents', icon: 'pi pi-fw pi-list', to: '/Acount/documents' },
            { label: 'Dashboard', icon: 'pi pi-chart-bar', to: '/Acount/dashboard' }
        ]
    }
]);
const model = computed(() => role === "Admin" ? modelAdmin.value : modelUser.value)
</script>

<template>
    <ul class="layout-menu">
        <template v-for="(item, i) in model" :key="item">
            <app-menu-item v-if="!item.separator" :item="item" :index="i"></app-menu-item>
            <li v-if="item.separator" class="menu-separator"></li>
        </template>
    </ul>
</template>

<style lang="scss" scoped></style>
