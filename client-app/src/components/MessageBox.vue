<script>
import {mapGetters,mapActions} from 'vuex'
export default{
  computed:{
    ...mapGetters('chat',['getChats']),
...mapGetters('messages',['getLastMessage']),

  },
  methods:{
    ...mapActions('chat',['loadChats']),
      ...mapActions('messages',['fetchLastMessage']),



  },
  mounted() {
    console.log('typeof getLastMessages', typeof this.getLastMessage)
    console.log('getLastMessages', this.getLastMessage)

    this.loadChats().then(() => {
      this.getChats.forEach(chat => {
        this.fetchLastMessage(chat.id)
      })
    })
  }
}



</script>

<template>
  <div v-for="chat in getChats" :key="chat.id" class="friend-drawer friend-drawer--onhover">

    <img
      class="profile-image"
      src="https://www.clarity-enhanced.net/wp-content/uploads/2020/06/robocop.jpg"
      alt=""
    />
    <div class="text">
      <h6>{{ chat.name }}</h6>
      <p v-if="getLastMessage(chat.id)">
        {{ getLastMessage(chat.id).text }}
      </p>
    </div>
    <span class="time text-muted small">13:21</span>
  </div>
  <hr />
</template>

<style scoped></style>
