<script>
import ChatMessage from './ChatMessage.vue'
import MessageBox from './MessageBox.vue'
import SpeakerInfo from './SpeakerInfo.vue'
import * as signalR from '@microsoft/signalr'
import {parseJwt} from '../Store/Modules/ParseJwt.js'
import axios from 'axios'
export default {
  props:{
    conversationId:{
      type:[Number,String],
      required:true
    }
  },
  watch:{
    conversationId:{
      immediate:true,
      handler(id){
        if(!id){
          this.messages=[];
          return;
        }
        axios.get(`/api/chats/${id}/messages`,{
          headers:{
            Authorization: `Bearer ${localStorage.getItem('token')}`
          }
        }).then(res=>
        {
          this.messages=res.data
        }).catch(err=>{
          console.error("повідомлення не завантажились",err)
          this.error=err.toString()
        })
      }
    }
  },
  components: { SpeakerInfo, MessageBox, ChatMessage },
  /*  name: 'ChatRoom',
    data() {
      return {
        connection: null,    // здесь будет наш HubConnection
        userName: '',        // поле для имени пользователя
        newMessage: '',      // текущее сообщение, которое вводят
        messages: [],
        historyMessages:[],// массив всех пришедших сообщений
        isConnected: false,  // флаг, что соединение установлено
        error: null          // ошибка, если не получилось подключиться
      }
    },
    created() {
      fetch(`/chat`).then(response => { response.json().then(data => {
        this.historyMessages = data;

      })})
    },
    methods: {
      sendMessage() {
        // Не отправляем пустое:
        if (!this.userName || !this.newMessage) return

        // Меняем флаг на случай, если соединение оборвалось:
        if (!this.isConnected) {
          this.error = 'Нет соединения с сервером'
          return
        }

        // Вызываем метод хаба SendMessage(user, message)
        this.connection.invoke('SendMessage', this.userName, this.newMessage)
          .then(() => {
            // После успешной отправки очищаем поле ввода
            this.newMessage = ''
          })
          .catch(err => {
            console.error('Ошибка при отправке сообщения:', err)
            this.error = err.toString()
          })
      }
    },
    mounted() {
      // 1. Создаём объект подключения
      this.connection = new signalR.HubConnectionBuilder()
        // Используем относительный URL, потому что Vite-прокси перенаправит его на ваш ASP.NET
        .withUrl('/hubs/chat')
        .withAutomaticReconnect()  // чтобы сам пытаcь переподключаться при обрыве
        .build()

      // 2. Подписываемся на событие, которое придёт с сервера
      //    'ReceiveMessage' — это имя, которое мы объявили в хабе (Clients.All.SendAsync("ReceiveMessage", ...))
      this.connection.on('ReceiveMessage', (user, message) => {
        // Когда приходит новый текст, добавляем в массив
        this.messages.push({ user, message })
        // Можно при желании: скроллить окно чата вниз или показать уведомление
      })

      // 3. Запускаем соединение
      this.connection.start()
        .then(() => {
          this.isConnected = true  // флаг, что всё в порядке
        })
        .catch(err => {
          // если что-то пошло не так — сохраним текст ошибки
          this.error = err.toString()
          console.error('Ошибка подключения SignalR:', err)
        })
    }*/
  created() {
    fetch(`/api/chats/${this.conversationId}/messages`, {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    })
      .then(response => {
        if (!response.ok) throw new Error(response.statusText);
        return response.json();
      })
      .then(data => { this.messages = data })
      .catch(err => console.error('Не удалось получить историю сообщений:', err));

  },
  data() {
    return {
      connection: null,
      userName: '',
      userMessage: '',
      bsModal: null,
      modalEl: null,
      messages: [],
      currentUser: null,

    }
  },
  mounted() {
    console.log('🐣 Chatroom mounted!')

    // 1. Создаём объект подключения
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/chat', {
        accessTokenFactory: () => {
          const token = localStorage.getItem('token')
          console.log('🔑 Отправляется токен:', token)
          return token
        },
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets  // 👈 Это ключ!
      })
      .withAutomaticReconnect()
      .build();

    const token = localStorage.getItem('token')
    if (token) {
      let payload
      try {
        payload = parseJwt(token)
      } catch (e) {
        console.error('❌ parseJwt error:', e)
      }
      console.log('📦 JWT payload:', payload)
      this.currentUser=payload.unique_name;
      // теперь посмотрите в консоли, какие у payload есть поля
    }
    // 2. Подписываемся на событие, которое придёт с сервера
    this.connection.on('ReceiveMessage', (name, message) => {
      this.messages.push({ name, message })
      // Можно при желании: скроллить окно чата вниз или показать уведомление
    })

    // 3. Запускаем соединение
    this.connection
      .start()
      .then(() => {
        this.isConnected = true // флаг, что всё в порядке
      })
      .catch((err) => {
        // если что-то пошло не так — сохраним текст ошибки
        this.error = err.toString()
        console.error('Ошибка подключения SignalR:', err)
      })

  },

  methods: {
    setConversation(id) {
      this.activeConversationId = id
      console.log('📌 Активный чат:', id)
    },
    sendMessage() {
      if (!this.isConnected) {
        this.error = 'Нет соединения с сервером'
        return
      }

      if (!this.conversationId) {
        console.error('❌ Нет выбранного чата (conversationId)')
        return
      }


      this.connection
        .invoke('SendMessage',  this.userMessage, this.conversationId)
        .then(() => {
          // После успешной отправки очищаем поле ввода
          this.userMessage = ''
        })
        .catch((err) => {
          console.error('Ошибка при отправке сообщения:', err)
          this.error = err.toString()
        })
    },
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
            src="https://www.clarity-enhanced.net/wp-content/uploads/2020/06/filip.jpg"
            alt="Profile img"
          />
          <span class="settings-tray--right">
            <i class="material-icons">cached</i>
            <i class="material-icons">message</i>
            <i class="material-icons">menu</i>
          </span>
        </div>
        <div class="search-box">
          <div class="input-wrapper">
            <i class="material-icons">search</i>
            <input placeholder="Search here" type="text" />
          </div>
        </div>
        <message-box></message-box>
      </div>
      <div class="col-md-8 chat-wrapper">
        <speaker-info></speaker-info>
        <div class="messages-area">
          <chat-message
            v-for="msg in messages"
            :key="msg.id"
            :message="msg"
            :current-user="currentUser"
          ></chat-message>
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
