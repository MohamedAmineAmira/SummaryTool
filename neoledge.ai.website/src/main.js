import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import App from './App.vue'
import router from './router'
import '@/assets/styles.scss';
import axiosInstance from '@/service/axiosInstance';
import Chart from 'primevue/chart';



const app = createApp(App)
app.use(PrimeVue, { ripple: true });
app.config.globalProperties.$api = { ...axiosInstance }
app.component('Chart', Chart);
app.use(router)

app.mount('#app')
