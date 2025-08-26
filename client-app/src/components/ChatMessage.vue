<script>
export default {
  props: {
    message: Object,
    currentUser: String,
  },
  computed: {
    isMine() {
      const sender = this.message.sender // подстраховка
      const mine = sender === this.currentUser
      return mine
    },
    containerClass() {
      return {
        'col-md-9': true, // <-- Исправлено
        'offset-md-3': this.isMine, // <-- Тоже надо исправить
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
          {{ message.text }}
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
