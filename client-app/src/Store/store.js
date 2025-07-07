import { createStore } from 'vuex'
import auth from './Modules/auth/index.js'
const store = createStore({
  modules:{
auth,

  }
})
export default store;
