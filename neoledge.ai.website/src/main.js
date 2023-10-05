import { createApp } from 'vue'
import PrimeVue from 'primevue/config';
import App from './App.vue'
import router from './router'
import '@/assets/styles.scss';
import axiosInstance from '@/service/axiosInstance';



const app = createApp(App)
app.use(PrimeVue, { ripple: true });
app.config.globalProperties.$api = { ...axiosInstance }

// note
const test = { a: 2, b: 9 }

const test1 = test;



app.use(router)

app.mount('#app')
