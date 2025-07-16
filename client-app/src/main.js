import './assets/main.scss'
import { createApp } from 'vue'
import router from './router'
import App from './App.vue'
import axios from 'axios'
import store from './Store/store'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'


// получение JWT-токена при заходе
const token = localStorage.getItem('token');
if (token) {
  store.commit('auth/SET_TOKEN', token);
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

createApp(App).use(router)
  .use(store).mount('#app')

