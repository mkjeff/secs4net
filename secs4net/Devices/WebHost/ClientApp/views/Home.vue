<template>
  <div class="home">
    <!-- <img src="../assets/logo.png"> -->
    <Demo />
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Demo from '@/components/HsmsDemo.vue'; // @ is an alias to /src

export default Vue.extend({
  name: 'home',
  components: {
    Demo,
  },
});
</script>
