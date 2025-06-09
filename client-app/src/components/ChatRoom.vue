<script >
import ChatMessage from "./ChatMessage.vue"
import MessageBox from './MessageBox.vue'
import SpeakerInfo from './SpeakerInfo.vue'
import { Modal } from 'bootstrap';
import * as signalR from '@microsoft/signalr'

export default {

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
    fetch(`/chat`).then(response => { response.json().then(data => {
      this.messages = data;

    })})
    this.currentUser=localStorage.getItem('userName');
  },
  data(){
    return{
      connection: null,
      userName: '',
      userMessage: '',
      bsModal: null,
      modalEl: null,
      messages:[],
      currentUser: null
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
    this.connection.on('ReceiveMessage', (name, message) => {
      // Когда приходит новый текст, добавляем в массив
      this.messages.push({ name, message })
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
    this.modalEl = document.getElementById('nameModal');
    this.bsModal = new Modal(this.modalEl, { backdrop: true });
    if(!localStorage.getItem('userName')){
      this.bsModal.show();
    }

  },

  methods:{
onSave(){
 // сохранение имени пользователя при нажатии на кнопку Save
    localStorage.setItem('userName', this.userName);
    this.bsModal.hide();
},
    sendMessage() {
      if (!this.isConnected) {
        this.error = 'Нет соединения с сервером'
        return
      }

      // Вызываем метод хаба SendMessage(user, message)
      this.connection.invoke('SendMessage', this.currentUser, this.userMessage)
        .then(() => {
          // После успешной отправки очищаем поле ввода
          this.userMessage = ''
        })
        .catch(err => {
          console.error('Ошибка при отправке сообщения:', err)
          this.error = err.toString()
        })
    }
  }
}

</script>

<template>
  <div class="modal fade" id="nameModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Modal title</h5>
            </div>
        <div class="modal-body">
          <label for="user-name" class="form-label">Your name here</label>
<input type="text" class="form-control" id="user-name" v-model="userName">
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-dismiss="modal" @click.prevent="onSave" >Save</button>
        </div>
      </div>
    </div>
  </div>
<!--  <div class="container" >
    &lt;!&ndash; Секция отправки &ndash;&gt;
    <div class="margin-bottom: 1rem;">
      <label for="name-input">Name</label>
      <input id="name-input" v-model="userName" placeholder="Ваше имя" />
      <label for="text-input" style="margin-left: 0.5rem;">Text</label>
      <input id="text-input" v-model="newMessage" placeholder="Введите сообщение" />

      <button @click="sendMessage" style="margin-left: 0.5rem;">Send</button>
    </div>

    &lt;!&ndash; Секция получения &ndash;&gt;
    <div>
      <p><em>Received messages:</em></p>
      <p v-for="(item, index) in messages" :key="index">
        <strong>{{ item.user }}:</strong> {{ item.message }}
      </p>
    </div>
    <div>
      <p> Старые сообщения</p>
      <p v-for="(msg,idx) in historyMessages" >
        <strong>{{ msg.name }}:</strong> {{ msg.message }}
        <small>{{ new Date(msg.timestamp).toLocaleTimeString() }}</small>
      </p>
    </div>
  </div>-->
  <div class="container ">
    <div class="row no-gutters">
      <div class="col-md-4 border-right">
        <div class="settings-tray">
          <img class="profile-image" src="https://www.clarity-enhanced.net/wp-content/uploads/2020/06/filip.jpg" alt="Profile img">
          <span class="settings-tray--right">
			<i class="material-icons">cached</i>
			<i class="material-icons">message</i>
			<i class="material-icons">menu</i>
		  </span>
        </div>
        <div class="search-box">
          <div class="input-wrapper">
            <i class="material-icons">search</i>
            <input placeholder="Search here" type="text">
          </div>
        </div>
   <message-box></message-box>
        </div>
      <div class="col-md-8 chat-wrapper">
<speaker-info></speaker-info>
        <div class="messages-area"><chat-message v-for="msg in messages" :key="msg.id" :message="msg" :current-user="currentUser"></chat-message> </div>
        <div class="row">
          <div class="col-12">
            <div class="chat-box-tray">
              <input type="text" v-model="userMessage" placeholder="Текст сюда">
              <div class="btn" @click.prevent="sendMessage">send</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>


</template>

<style lang="scss">


</style>
