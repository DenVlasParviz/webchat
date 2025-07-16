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
      <router-link to="/users" class="navbar-brand">Users</router-link>
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

<style>
/* Можно оставить пустым или добавить несколько базовых стилей */
</style>
