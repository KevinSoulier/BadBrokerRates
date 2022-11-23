<template>
  <!--<img alt="Vue logo" src="./assets/logo.png">-->
  <!--<HelloWorld msg="Hola que hace perri"/>-->
  <BestRates/>
</template>

<script>
//import HelloWorld from './components/HelloWorld.vue'
import BestRates from './components/BestRates.vue'

export default {
  name: 'App',
  components: {
    //HelloWorld,
    BestRates
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  /*text-align: center;*/
  color: #2c3e50;
  /*margin-top: 60px;*/
}
</style>
