import { createRouter, createWebHistory } from "vue-router";
import TableDocuments from '../views/TableDocuments.vue'
import AppLayout from '@/layout/AppLayout.vue';
import TableDataPreprocessor from '../views/TableDataPreprocessor.vue';
import TableTextToolbox from '../views/TableTextToolbox.vue';
import TableTextSummarizer from '../views/TableTextSummarizer.vue';
import Login from '../views/auth/Login.vue';
import Register from '../views/auth/Register.vue';
import Dashboard from '../views/dashboard.vue';

const isAuthenticated = () => {
    return localStorage.getItem('token') !== null;
};

const isAdmin = () => {
    return localStorage.getItem('role') === "Admin"; // Use strict comparison (===)
};

const requireAuth = (to, from, next) => {
    if (!isAuthenticated()) {
        next('/'); // Redirect to login page if not authenticated
    } else {
        next();
    }
};

const requireAuthAdmin = (to, from, next) => {
    if (!isAuthenticated() || !isAdmin()) {
        next('/'); // Redirect to login page if not authenticated or not an admin
    } else {
        next();
    }
}

const routes = [
    {
        path: '/Acount',
        component: AppLayout,
        beforeEnter: requireAuth,
        children: [
            {
                path: 'documents',
                component: TableDocuments,
                beforeEnter: requireAuth // Apply the requireAuth guard
            },
            {
                path: 'dashboard',
                component: Dashboard,

            },
            {
                path: 'dataPreprocessor',
                component: TableDataPreprocessor,
                beforeEnter: requireAuthAdmin
            },
            {
                path: 'textAnalyticsToolbox',
                component: TableTextToolbox,
                beforeEnter: requireAuthAdmin
            },
            {
                path: 'textSummarizer',
                component: TableTextSummarizer,
                beforeEnter: requireAuthAdmin
            }
        ]
    },
    {
        path: '/',
        component: Login
    },
    {
        path: '/Register',
        component: Register
    }
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
});

export default router;


