<script setup>
import { ref } from 'vue';

import AppMenuItem from './AppMenuItem.vue';

const model = ref([
    {
        label: 'Home',
        items: [
            { label: 'Documents', icon: 'pi pi-fw pi-list', to: '/' },
            { label: 'Dashboard', icon: 'pi pi-chart-bar', to: '/dashboard' }
        ]
    },
    {
        label: 'Settings Hub',
        items: [
            { label: 'Data Preprocessor', icon: 'pi pi-eraser', to: '/dataPreprocessor' },
            { label: 'Text Analytics Toolbox', icon: 'pi pi-cog', to: '/textAnalyticsToolbox' },
            { label: 'Text Summarizer', icon: 'pi pi-align-left', to: '/textSummarizer' }
        ]

    }
]);
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
