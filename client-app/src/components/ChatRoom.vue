<script>
import ChatMessage from './ChatMessage.vue'
import MessageBox from './MessageBox.vue'
import SpeakerInfo from './SpeakerInfo.vue'
import * as signalR from '@microsoft/signalr'
import {parseJwt} from '../Store/Modules/ParseJwt.js'
import { mapGetters,mapActions } from 'vuex'
export default {
  props:{
    conversationId:{
      type:[Number,String],
      required:true
    }
  },
  watch: {
    conversationId: {
      immediate: true,
      handler(id) {
        if (id) this.loadMessages(id)
      }
    }
  },
  components: { SpeakerInfo, MessageBox, ChatMessage },

  data() {
    return {
      connection: null,
      userName: '',
      userMessage: '',
      bsModal: null,
      modalEl: null,
      currentUser: null,
    }
  },
  computed: {
    ...mapGetters('messages', { rawMessages: 'getMessages' }),
    messages() {
      return this.rawMessages(this.conversationId)
    }
  },
  mounted() {
    // 1) Извлекаем currentUser из JWT
    const token = localStorage.getItem('token')
    if (token) {
      const payload = parseJwt(token)
      const nameClaim =
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      this.currentUser = nameClaim ?? payload.sub
    }

    // 2) Настраиваем SignalR
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/chat', {
        accessTokenFactory: () => localStorage.getItem('token'),
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build()

    // 3) Приход новых сообщений — пушим их в Vuex через мутацию
    this.connection.on('ReceiveMessage', (sender, text) => {
      const now = new Date()

      this.$store.commit('messages/pushMessage', {
        chatId: this.conversationId,
        message: {
          id: Date.now(),
          sender,
          text,
          timestamp: now
        }
      })

      this.$store.commit('messages/setLastMessages', {
        chatId: this.conversationId,
        message: {
          sender,
          text,
          timestamp: now
        }
      })
    })

    // 4) Стартуем соединение
    this.connection
      .start()
      .then(() => {
        this.isConnected = true
      })
      .catch(err => {
        console.error('Ошибка подключения SignalR:', err)
        this.error = err.toString()
      })
  },

  methods: {
    ...mapActions('messages', ['loadMessages']),

    // Отправить сообщение через SignalR
    sendMessage() {
      if (!this.isConnected) {
        this.error = 'Нет соединения с сервером'
        return
      }
      if (!this.conversationId) {
        this.error = 'Не выбран чат'
        return
      }

      this.connection
        .invoke('SendMessage', this.userMessage, this.conversationId)
        .then(() => {
          this.userMessage = ''
        })
        .catch(err => {
          console.error('Ошибка отправки сообщения:', err)
          this.error = err.toString()
        })
    }
  },
}
</script>

<template>
  <div class="container">
    <div class="row no-gutters">
      <div class="col-md-4 border-right">
        <div class="settings-tray">
          <img
            class="profile-image"
            src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
            alt="Profile img"
          />
          <span class="settings-tray--right">
<!--            <i class="material-icons">cached</i>-->
<!--            <i class="material-icons">message</i>-->
<!--            <i class="material-icons">menu</i>-->
          </span>
        </div>
        <div class="search-box">
          <div class="input-wrapper">
            <i class="material-icons">search</i>
            <input placeholder="Search here" type="text" />
          </div>
        </div>
        <message-box @open-chat="$emit('open-chat',$event)" />
      </div>
      <div class="col-md-8 chat-wrapper">
        <speaker-info></speaker-info>
        <div class="messages-area">
          <chat-message
            v-for="msg in messages"
            :key="msg.id"
            :message="msg"
            :current-user="currentUser"
          > </chat-message>
        </div>
        <div class="row">
          <div class="col-12">
            <div class="chat-box-tray">
              <input type="text" v-model="userMessage" placeholder="Текст сюда" />
              <div class="btn" @click.prevent="sendMessage">send</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss"></style>
