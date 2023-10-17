import { createRouter, createWebHistory } from "vue-router";
import TableDocuments from '../views/TableDocuments.vue'
import AppLayout from '@/layout/AppLayout.vue';
import TableDataPreprocessor from '../views/TableDataPreprocessor.vue';
import TableTextToolbox from '../views/TableTextToolbox.vue';
import TableTextSummarizer from '../views/TableTextSummarizer.vue';


const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            component: AppLayout,
            children: [
                {
                    path: '/',
                    component: TableDocuments
                },
                {
                    path: '/dataPreprocessor',
                    component: TableDataPreprocessor
                },
                {
                    path: '/textAnalyticsToolbox',
                    component: TableTextToolbox
                },
                {
                    path: '/textSummarizer',
                    component: TableTextSummarizer   
                }
            ]

        }
    ]
})
export default router 
