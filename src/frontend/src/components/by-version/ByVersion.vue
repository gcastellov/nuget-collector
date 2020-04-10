<template>
  <div>
      <div v-for="reference in this.references" :key="reference.getId()" class="reference">
        <ul v-if="reference.projects.length > 0" class="list-group">
          <li class="list-group-item d-flex justify-content-between align-items-center">
            {{reference.name}}
            <div class="reference-summary">
              <span v-if="reference.counter > 1" class="badge badge-secondary badge-danger">{{reference.counter}}</span>
              <span class="badge badge-secondary">{{reference.version}}</span>
              <span class="badge badge-primary badge-pill">{{reference.projects.length}}</span>
            </div>
          </li>
          <li class="list-group-item d-flex justify-content-between align-items-center list-item-project" v-for="project in reference.projects" :key="project">
            <small>{{project}}</small>
          </li>
        </ul>
      </div>
  </div>
</template>

<style lang="scss">
  .reference {
    margin-top: 10px;
    margin-bottom: 30px;
    padding-left: 10%;
    padding-right: 10%;
  }
  .list-item-project small {
    padding-left: 15px;
  }
  .reference-summary > .badge-secondary {
    margin-right: 10px;
  }
</style>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { Reference } from './Reference'
import axios, { AxiosRequestConfig } from 'axios'
import VueAxios from 'vue-axios'
Vue.use(VueAxios, axios)

@Component
export default class ByVersion extends Vue {
  public references: Reference[] = []

  public created () {
    this.load()
  }

  private async load () {
    const config: AxiosRequestConfig = {
      headers: { 'Access-Control-Allow-Origin': '*' }
    }

    Vue.axios.get(`${Vue.prototype.$baseUrl}/api/versions`, config).then((r) => {
      this.references = this.mapToReferences(r)
    })
  }

  private mapToReferences (response: any): Reference[] {
    const references: Reference[] = []
    response.data.map(function (value: any) {
      value.matches.map(function (match: any) {
        const ref = new Reference(value.reference, match.version, value.matches.length)
        match.matches.map(function (project: any) {
          ref.projects.push(project)
        })
        references.push(ref)
      })
    })
    return references
  }
}
</script>
