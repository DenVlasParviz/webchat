<template>
  <div class="chat-layout">
    <aside class="sidebar">
<!--
      <users-list @chat-started="openChat"/>
-->
    </aside>
    <chat-window :conversation-id="activeId"  @open-chat="openChat" />
  </div>
</template>

<script>
import ChatWindow from './ChatRoom.vue'
import axios from 'axios'

export default {

  components: {  ChatWindow },
  props:{
    conversationId:{
      type:[Number,String],
      default:null
    }
  },
  data() {
    return {
      conversations: [],

      activeId: this.conversationId,
    }
  },
  watch:{
    conversationId:{
      immediate:true,
      handler(val){
        this.activeId=val;
      }
    }
  },

  async mounted() {
    try {
      const res = await axios.get('/api/chats', {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      })
      this.conversations = res.data

      // если при заходе URL указывал несуществующий чат — почистим activeId
      if (this.activeId && !this.conversations.find(c => c.id === +this.activeId)) {
        this.activeId = null
        if (this.conversationId && this.conversations.some(c => c.id === +this.conversationId)){
          this.openChat(this.conversationId)
        }
      }
    } catch (err) {
      console.error('Ошибка при загрузке чатов:', err)
    }
  },
  methods: {
    openChat(convId) {
      // 4) пишем в локалку, чтобы моментально отрендерить окно
      this.activeId = convId
      //    и меняем URL
      this.$router.push({ path: `/users/${convId}` })
    }
  }
}
</script>
