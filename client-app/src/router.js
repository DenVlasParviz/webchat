import { createRouter, createWebHistory } from 'vue-router'
import Login from './pages/LoginPage.vue'
import SignUp from './pages/SignUpPage.vue'
const routes=[
  {path:'/login', component:Login},
  {path:'/signup', component:SignUp}

]
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
