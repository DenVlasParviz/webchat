<script >
import signUpValidation from '/src/Services/SignUpValidation'
export default {
data(){
  return{
    email:'',
    password:'',
    errors:[],
  }
},
  methods:{
  onLogin(){
    //check the validations
let validations = new signUpValidation(this.email,this.password);
this.errors =validations.checkValidations();
if(this.errors.length){
return false;
}
//continue signup registration

  }
  }
}
</script>

<template>
<div>
  <form class="form-group">
    <label for="email-text">Email</label>
<input type="text" class="form-control" v-model="email" >
    <div class="error" v-if="errors.email">{{errors.email}}</div>
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
