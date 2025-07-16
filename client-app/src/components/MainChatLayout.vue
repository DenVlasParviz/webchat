<template>
  <div class="chat-layout">
    <aside class="sidebar">
      <users-list @chat-started="openChat"/>
      <conversations-list :conversations="conversations" @select="openChat"/>
    </aside>
    <chat-window :conversation-id="activeId" v-if="activeId"/>
  </div>
</template>

<script>
import UsersList from './UsersList.vue'
import ConversationsList from './ConversationList.vue'
import ChatWindow from './ChatRoom.vue'
import axios from 'axios'

export default {
  components: { UsersList, ConversationsList, ChatWindow },
  data() {
    return {
      conversations: [],
      activeId: null
    }
  },
  async mounted() {
    try {
      const res = await axios.get('/api/chats', {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      this.conversations = res.data
    } catch (err) {
      console.error('Ошибка при загрузке чатов:', err)
    }
  },
  methods: {
    openChat(convId) {
      console.log('🏷️ clicked conversation:', convId)
      this.activeId = convId
    }
  }
}
</script>
