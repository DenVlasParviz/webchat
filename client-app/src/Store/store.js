import { createStore } from 'vuex'
import chat from './Modules/chat/chat.js'
import auth from './Modules/auth/index.js'
const store = createStore({
  modules:{
auth,
    chat

  }
})
export default store;
