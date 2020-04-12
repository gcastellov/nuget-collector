<template>
  <div>
    <ul class="list-group stats">
      <li class="list-group-item d-flex justify-content-between align-items-center">
        <span>Number of projects</span>
        <span class="badge badge-primary badge-pill">{{data.projectsCount}}</span>
      </li>
      <li class="list-group-item d-flex justify-content-between align-items-center">
        <span>Number of all references on projects</span>
        <span class="badge badge-primary badge-pill">{{data.allReferenceCount}}</span>
      </li>
      <li class="list-group-item d-flex justify-content-between align-items-center">
        <span>Number of unique references (by name &amp; version)</span>
        <span class="badge badge-primary badge-pill">{{data.uniqueReferenceCount}}</span>
      </li>
      <li class="list-group-item d-flex justify-content-between align-items-center">
        <span>Number of references (by name)</span>
        <span class="badge badge-primary badge-pill">{{data.differentReferenceCount}}</span>
      </li>
    </ul>
  </div>
</template>

<style lang="scss">
  .stats {
    margin-top: 10px;
    margin-bottom: 30px;
    padding-left: 10%;
    padding-right: 10%;
  }
</style>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import axios, { AxiosRequestConfig } from 'axios'
import VueAxios from 'vue-axios'
import { Data } from './Data'

@Component
export default class Stats extends Vue {
    public data: Data = new Data(0, 0, 0, 0)

    public created () {
      const config: AxiosRequestConfig = {
        headers: { 'Access-Control-Allow-Origin': '*' }
      }
      Vue.axios.get(`${Vue.prototype.$baseUrl}/api/stats`, config).then((r) => {
        this.data = new Data(r.data.projectsCount, r.data.allReferenceCount, r.data.uniqueReferenceCount, r.data.differentReferenceCount)
      })
    }
}
</script>
