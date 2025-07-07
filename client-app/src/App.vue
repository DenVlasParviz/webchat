<template>
<!--  <chat-room></chat-room>-->

<sign-up-page></sign-up-page>
 <router-view>
    </router-view>


</template>

<script>

import SignUpPage from './Pages/SignUpPage.vue'
export default {
components:{
  SignUpPage,
}
  /*  components: { ChatRoom },*/
}
</script>

<style>
/* Можно оставить пустым или добавить несколько базовых стилей */
</style>
