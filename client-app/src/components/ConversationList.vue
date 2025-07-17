<template>
  <div class="conversations-list">
    <h2>Мои чаты</h2>
    <ul>
      <li
        v-for="c in conversations"
        :key="c.id"
        @click="$emit('select', c.id)"
        class="conversation-item"
      >
        {{ c.name }} (ID: {{ c.id }})
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  props: {
    conversations: { type: Array, required: true},

  }
}
</script>

<style scoped>
.conversation-item {
  cursor: pointer;           /* подсказка, что по элементу можно кликать */
  padding: 0.5rem;
}
.conversation-item:hover {
  background: #f0f0f0;
}
</style>
