<script>
import {mapGetters,mapActions} from 'vuex'
export default{
  emits: ["open-chat"],
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
  <div v-for="chat in getChats" :key="chat.id" class="friend-drawer friend-drawer--onhover"
       @click="$emit('open-chat', chat.id)"
  >
    <img
      class="profile-image"
      src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
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
