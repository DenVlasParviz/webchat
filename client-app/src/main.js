import './assets/main.scss'
import { createApp } from 'vue'
import router from './router'
import App from './App.vue'
import store from './Store/store'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'

createApp(App).use(router)
  .use(store).mount('#app')

