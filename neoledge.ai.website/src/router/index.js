import { createRouter, createWebHistory } from "vue-router";
import TableView from '../views/TableView.vue'
import AppLayout from '@/layout/AppLayout.vue';


const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes:[
        {
            path:'/',
            component:AppLayout,
            children:[
                {
                    path:'/',
                    component: TableView
                }
            ]

        }
    ]
})
export default router 
