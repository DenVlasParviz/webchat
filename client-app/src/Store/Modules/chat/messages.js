import axios from 'axios'
import * as signalR from '@microsoft/signalr'
import { createRouterMatcher as Promise } from 'vue-router'


export default{

  namespaced:true,
  state: () => ({
    messagesByChat: {},  // { [chatId]: [ { id, sender, text, timestamp } ] }
    lastMessages: {}
  }),

  mutations:{
    setMessages(state, { chatId, message }) {
      state.messagesByChat = {
        ...state.messagesByChat,
        [chatId]: message
      }
    },
    setLastMessages(state, { chatId, message }) {
      state.lastMessages = {
        ...state.lastMessages,
        [chatId]: message
      }
    },

    pushMessage(state,{chatId, message}){
      const arr = state.messagesByChat[chatId] || []
      state.messagesByChat={
        ...state.messagesByChat,[chatId] : [...arr,message]
      }
    }
  },

  getters:{
   getMessages:(state)=>(chatId)=>state.messagesByChat[chatId] || [],
    getLastMessage: (state) => (chatId) => {
      return state.lastMessages[chatId] || null
    }   },

  actions: {
    async loadMessages({ commit }, chatId) {
      try {
        // 1) используем backticks и chatId
        const res = await axios.get(
          `/api/chats/${chatId}/messages`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem('token')}`
            }
          }
        )
        // 2) коммитим именно setMessages
        commit('setMessages', { chatId, message: res.data })
      } catch (e) {
        console.error('messages/loadMessages error', e)
      }
    },
    // используется чтобы вывести в messagebox сообщение последнее
    async fetchLastMessage({ commit }, chatId) {
      const res = await axios.get(
        `/api/chats/last-messages`,
        { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` } }
      );
      // Отфильтруем локально нужное:
      const msg = res.data.find(m => m.conversationId === chatId);
      commit('setLastMessages', { chatId, message: msg });
    }
  }


}
