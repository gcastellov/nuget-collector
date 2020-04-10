import Vue from 'vue'
import VueRouter from 'vue-router'
import ByProject from '../components/by-project/ByProject.vue'
import ByReference from '../components/by-reference/ByReference.vue'
import ByVersion from '../components/by-version/ByVersion.vue'
import Settings from '../components/settings/Settings.vue'
import Stats from '../components/stats/Stats.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Settings',
    component: Settings
  },
  {
    path: '/stats',
    name: 'Stats',
    component: Stats
  },
  {
    path: '/by-project',
    name: 'ByProject',
    component: ByProject
  },
  {
    path: '/by-reference',
    name: 'ByReference',
    component: ByReference
  },
  {
    path: '/by-version',
    name: 'ByVersion',
    component: ByVersion
  }
]

const router = new VueRouter({
  routes
})

export default router
