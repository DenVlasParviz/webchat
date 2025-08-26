<script >
import { mapGetters } from 'vuex'
export default{

  ...mapGetters('userName',['getUserNameById']),

  data(){
return{
  displayName:''
}
}



}

</script>

<template>
  <div class="friend-drawer no-gutters friend-drawer--grey">
    <img
      class="profile-image"
      src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
      alt="Friend Image"
    />
    <div class="text">
      <h6>ChatName</h6>
      <p>Заглушка</p>
    </div>
  </div>
</template>

<style scoped></style>
