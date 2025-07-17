<script>
import {mapGetters,mapActions} from 'vuex'
export default{
  computed:{
    ...mapGetters('chat',['getChats']),

  },
  methods:{
    ...mapActions('chat',['loadChats'])

  },
  mounted() {
    this.loadChats()
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
      <p class="text-muted">test</p>
    </div>
    <span class="time text-muted small">13:21</span>
  </div>
  <hr />
</template>

<style scoped></style>
