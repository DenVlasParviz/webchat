// store/modules/chats.js

import axios from 'axios'

export default {
  namespaced: true,

  state: () => ({
    chats: [],
  }),

  mutations: {
    initializeChats(state, newData) {
      state.chats = newData;
    },
  },

  getters: {
    getChats(state) {
      return state.chats;
    },
  },

  actions: {
    async loadChats({ commit }) {
      try {
        const response = await axios.get('/api/chats')

        commit("initializeChats", response.data)
      } catch (e) {
        console.error(e)
      }
    }
  }
}
