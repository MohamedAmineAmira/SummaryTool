import { createRouter, createWebHistory } from "vue-router";
import TableDocuments from '../views/TableDocuments.vue'
import AppLayout from '@/layout/AppLayout.vue';
import TableDataPreprocessor from '../views/TableDataPreprocessor.vue';
import TableTextToolbox from '../views/TableTextToolbox.vue';
import TableTextSummarizer from '../views/TableTextSummarizer.vue';
import Login from '../views/auth/Login.vue';

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/Acount',
            component: AppLayout,
            children: [
                {
                    path: 'documents',
                    component: TableDocuments
                },
                {
                    path: 'dataPreprocessor',
                    component: TableDataPreprocessor
                },
                {
                    path: 'textAnalyticsToolbox',
                    component: TableTextToolbox
                },
                {
                    path: 'textSummarizer',
                    component: TableTextSummarizer
                }
            ]

        },
        {
            path: '/',
            component: Login
        }
    ]
})
export default router 
