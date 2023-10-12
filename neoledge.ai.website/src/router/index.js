import { createRouter, createWebHistory } from "vue-router";
import TableDocuments from '../views/TableDocuments.vue'
import AppLayout from '@/layout/AppLayout.vue';
import TableDataPreprocessor from '../views/TableDataPreprocessor.vue';

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
                }
            ]

        }
    ]
})
export default router 
