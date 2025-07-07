<script>
export default {
  props: {
    message: Object,
    currentUser: String,
  },
  computed: {
    isMine() {
      return this.message.name === this.currentUser
    },
    containerClass() {
      return {
        'col-md-3': true,
        'offset-md-9': this.isMine,
      }
    },
    bubbleClass() {
      return [
        'chat-bubble',
        this.isMine ? 'chat-bubble--right chat-bubble--blue' : 'chat-bubble--left',
      ]
    },
  },
}
</script>

<template>
  <div class="chat-panel">
    <div class="row no-gutters">
      <div :class="containerClass">
        <div :class="bubbleClass">
          {{ message.message }}
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
