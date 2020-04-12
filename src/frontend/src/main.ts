import Vue from 'vue'
import App from './App.vue'
import router from './router'
import codeHub from './plugins/code.hub'
import '../node_modules/bootstrap/scss/bootstrap.scss'

const hub = new Vue()
Vue.prototype.$codeHub = hub
Vue.prototype.$baseUrl = process.env.VUE_APP_API_URL
Vue.config.productionTip = false

Vue.use(codeHub)

new Vue({
  router,
  render: h => h(App)
}).$mount('#app')
