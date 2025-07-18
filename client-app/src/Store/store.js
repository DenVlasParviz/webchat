import { createStore } from 'vuex'
import chat from './Modules/chat/chat.js'
import auth from './Modules/auth/index.js'
import messages from './Modules/chat/messages.js'
const store = createStore({
  modules:{
auth,
    chat,
    messages

  }
})
export default store;
