import { createRouter, createWebHistory } from 'vue-router'
import Login from './pages/LoginPage.vue'
import SignUp from './pages/SignUpPage.vue'
import Chat from './components/ChatRoom.vue'
import UsersList from './components/MainChatLayout.vue'
const routes=[
  {path:'/login', component:Login},
  {path:'/signup', component:SignUp},
  {path:'/', component:Chat},
  {path:'/users', component:UsersList}

]
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
