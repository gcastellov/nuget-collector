<template>
  <div id="app">
    <img src="./assets/logo.svg" aria-hidden="true" alt="Logo"  width="184" height="57"/>
    <div id="nav">
      <router-link to="/">Settings</router-link> |
      <router-link to="/stats">Stats</router-link> |
      <router-link to="/by-project">By Project</router-link> |
      <router-link to="/by-reference">By Reference</router-link> |
      <router-link to="/by-version">By Version</router-link>
    </div>
    <router-view/>
    <div class="footer">
      <a v-bind:href="swagger" target="_blank"> Swagger API </a>
    </div>
  </div>
</template>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  margin-top: 15px;
  height: 100%;
}

#nav {
  padding: 20px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #007bff;
    }
  }
}
</style>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class App extends Vue {
  public swagger: string

  constructor () {
    super()
    this.swagger = `${Vue.prototype.$baseUrl}/swagger`
  }
}
</script>
