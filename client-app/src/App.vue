
<template>
<!--  <chat-room></chat-room>-->



<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container">
    <li class="nav-item">
      <router-link to="/login" class="navbar-brand">Login</router-link>
    </li>
    <li class="nav-item">
      <router-link to="/signup" class="navbar-brand">Sign Up</router-link>
    </li>
    <li class="nav-item">
      <router-link to="/users-list" class="navbar-brand">Users</router-link>
    </li>
    <li class="nav-item">
      <router-link to="/" class="navbar-brand">Home</router-link>
    </li>


  </div>
</nav>
 <router-view>
    </router-view>


</template>

<script>

export default {
components:{
}
  /*  components: { ChatRoom },*/
}
</script>

<style src="./assets/base.css"></style>
