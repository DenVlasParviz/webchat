import { createRouter, createWebHistory } from 'vue-router'
import Login from './pages/LoginPage.vue'
import SignUp from './pages/SignUpPage.vue'
import Users from './components/UsersList.vue'
import mainChatLayout from './components/MainChatLayout.vue'
const routes=[
  {path:'/login', component:Login},
  {path:'/signup', component:SignUp},
  { path: '/', redirect: '/users' },
  {path:'/users-list', component:Users},
  {path:'/users/:conversationId?', component:mainChatLayout,
  props: route=>({
    conversationId: route.params.conversationId? Number(route.params.conversationId): null
  })},
  { path: '/:catchAll(.*)', redirect: '/users' }

]
const router = createRouter({
  history: createWebHistory(),
  routes,
});
export default router;
