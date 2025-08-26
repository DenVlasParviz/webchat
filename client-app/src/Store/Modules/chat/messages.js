import axios from 'axios'
import * as signalR from '@microsoft/signalr'
import { createRouterMatcher as Promise } from 'vue-router'


export default {

  namespaced: true,
  state: () => ({
    messagesByChat: {},  // { [chatId]: [ { id, sender, text, timestamp } ] }
    lastMessages: {},
    usersById: {}
  }),

  mutations: {
    setUsers(state, users) {
      const map = {}
      users.forEach(u => {
        map[u.id] = u.userName
      })
      state.usersById = map
    },
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

    pushMessage(state, { chatId, message }) {
      const arr = state.messagesByChat[chatId] || []
      state.messagesByChat = {
        ...state.messagesByChat, [chatId]: [...arr, message]
      }
    }
  },

  getters: {
    getMessages: (state) => (chatId) => state.messagesByChat[chatId] || [],
    getLastMessage: (state) => (chatId) => {
      return state.lastMessages[chatId] || null
    },
    getUserNameById: (state) => (id) => {
      if (!id) return null
      return state.usersById[id] ?? null
    }
  },

  actions: {
    async loadUsers({ commit }, userId = null) {
      const token = localStorage.getItem('token')
      const headers = { Authorization: `Bearer ${token}` }

      if (userId) {
        // Попытаться получить одиночного пользователя
        try {
          const res = await axios.get(`/api/users/${encodeURIComponent(userId)}`, { headers })
          // Ожидаем { id, userName }
          commit('setUser', res.data)
          return res.data
        } catch (err) {
          // Если endpoint /api/users/{id} отсутствует (404) или другой сбой =>
          // fallback: загрузим всех пользователей и выберем нужного
          console.warn('loadUsers(single) failed, fallback to load all users', err)
        }
      }

      // Общий случай: загрузить всех пользователей
      try {
        const res = await axios.get('/api/users', { headers })
        // Ожидаем массив [{ id, userName }]
        commit('setUsers', res.data || [])
        return res.data || []
      } catch (e) {
        console.error('messages/loadUsers error', e)
        return []
      }
    },

    async loadMessages({ commit }, chatId) {
      try {
        const res = await axios.get(
          `/api/chats/${chatId}/messages`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem('token')}`
            }
          }
        )
        commit('setMessages', { chatId, message: res.data })
      } catch (e) {
        console.error('messages/loadMessages error', e)
      }
    },

    async fetchLastMessage({ commit }, chatId) {
      const res = await axios.get(
        `/api/chats/last-messages`,
        { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` } }
      )

      const msg = res.data.find(m => m.conversationId === chatId)
      commit('setLastMessages', { chatId, message: msg })
    }
  }


}
