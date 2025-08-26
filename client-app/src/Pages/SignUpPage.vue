<script>
import { mapActions } from 'vuex'
import { SIGNUP_ACTIONS } from '/src/Store/Modules/storeConstants.js'
import Validations from  '/src/Services/Validations.js'
export default {
  data(){
    return{
      username:'',
      password:'',
      confirmPassword:'',
      errors:{
        username:null,
        password:null,
        confirmPassword:null,
        general:null
      }
    }
  },
  methods: {
    ...mapActions('auth', {
      signup: SIGNUP_ACTIONS,
    }),
    validate() {
      this.errors = { username: null, password: null, confirmPassword: null }
      if (!this.username) {
        this.errors.username = 'Имя пользователя обязательно'
      }
      if (!Validations.minLength(this.password, 6)) {
        this.errors.password = 'Пароль должен быть не менее 6 символов'
      }
      if (this.password !== this.confirmPassword) {
        this.errors.confirmPassword = 'Пароли не совпадают'
      }
      // вернуть true, если нет ошибок
      return !this.errors.username && !this.errors.password && !this.errors.confirmPassword
    },
    async onSignup() {
      //check the validations
      if (!this.validate()) {
        return
      }
      try {
        await this.signup({
          username: this.username,
          password: this.password,
        })
        this.$router.push('/login')
      } catch (e) {
        console.error("Ошибка входа:" + e.response.data.message)
        this.errors.general = typeof e.response.data === 'string'
          ? e.response.data
          : JSON.stringify(e.response.data);
      }
    }
  }
}
</script>

<template>
  <div class="container"
       style="max-width: 400px; margin: 50px auto; background: white; padding: 40px; border-radius: 20px; box-shadow: 0 20px 40px rgba(0,0,0,0.1);">
    <form class="form-group">
      <label for="username-text">Username</label>
      <input type="text" class="form-control" v-model="username">
      <div class="error" v-if="errors.username">{{ errors.username }}</div>
      <label for="password-text">Password</label>
      <input type="password" class="form-control" id="password-text" v-model="password">
      <div class="error" v-if="errors.password">{{ errors.password }}</div>
      <label for="password-text">Confirm password</label>
      <input type="password" class="form-control" id="confirmPassword-text"
             v-model="confirmPassword">
      <div class="error" v-if="errors.confirmPassword">{{ errors.confirmPassword }}</div>
    </form>
    <div>
      <button class="btn btn-primary" type="submit" @click.prevent="onSignup">SignUp</button>
    </div>
  </div>
</template>

<style scoped></style>
