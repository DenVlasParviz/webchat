<script >
import { mapActions } from 'vuex'
import { SIGNIN_ACTIONS } from '/src/Store/Modules/storeConstants.js'
export default {
data(){
  return{
    username:'',
    password:'',
    errors:[],
  }
},
  methods:{
    ...mapActions('auth',{signin:SIGNIN_ACTIONS}),

   async  onLogin(){
try{
  await this.signin({
    username: this.username,
    password: this.password,
  })
  this.$router.push('/');
}
catch(e) {
  console.error("Ошибка входа:" +e.response.data.message )
  this.errors.general = typeof e.response.data === 'string'
    ? e.response.data
    : JSON.stringify(e.response.data);}
  }
  }
}
</script>

<template>
<div>
  <form class="form-group">
    <label for="username-text">Username</label>
<input type="text" class="form-control" v-model="username">
    <div class="error" v-if="errors.username">{{errors.username}}</div>
    <label for="password-text">Password</label>
    <input type="password" class="form-control" id="password-text" v-model="password">
    <div class="error" v-if="errors.password">{{errors.password}}</div>

  </form>
  <div>
    <button class="btn btn-primary" type="submit" @click.prevent="onLogin">Login</button>
  </div>
</div>
</template>

<style scoped>

</style>
