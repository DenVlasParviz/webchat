<template>
  <div class="user-list">
    <h2>найти користувача</h2>
    <ul>
      <li v-for="u in users" :key="u.id" @click="startChat(u)">
        {{ u.userName }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      users: []
    };
  },

  async mounted() {
    const res = await axios.get('/api/users', {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    });
    this.users = res.data;
  },

  methods: {
    async startChat(user) {
      const convRes = await axios.post(
        '/api/chats',
        JSON.stringify(user.userName),
        {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`
          }
        }
      );
      const conv = convRes.data;

      await axios.post(`/api/chats/${conv.id}/users/${user.id}`, null, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });

      const meId = this.parseToken().sub;
      await axios.post(`/api/chats/${conv.id}/users/${meId}`, null, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });

      this.$emit('chat-started', conv.id);
    },

    parseToken() {
      const token = localStorage.getItem('token');
      if (!token) return {};

      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload;
    }
  }
};
</script>

<style scoped>
/* Я не знаю, что тут писать, так что пусть будет пусто. */
</style>
