<template>
  <div>
      <div v-for="reference in this.references" :key="reference.getId()" class="reference">
        <ul v-if="reference.projects.length > 0" class="list-group">
          <li class="list-group-item d-flex justify-content-between align-items-center">
            {{reference.name}}
            <span class="badge badge-primary badge-pill">{{reference.projects.length}}</span>
          </li>
          <li class="list-group-item d-flex justify-content-between align-items-center list-item-project" v-for="project in reference.projects" :key="project.getId()">
            <small>{{project.name}}</small>
            <span class="badge badge-secondary">{{project.version}}</span>
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
</style>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { Project } from './Project'
import { Reference } from './Reference'
import axios, { AxiosRequestConfig } from 'axios'
import VueAxios from 'vue-axios'
Vue.use(VueAxios, axios)

@Component
export default class ByReference extends Vue {
  public references: Reference[] = []

  public created () {
    this.load()
  }

  private async load () {
    const config: AxiosRequestConfig = {
      headers: { 'Access-Control-Allow-Origin': '*' }
    }

    Vue.axios.get(`${Vue.prototype.$baseUrl}/api/references`, config).then((r) => {
      this.references = this.mapToReferences(r)
    })
  }

  private mapToReferences (response: any): Reference[] {
    const references: Reference[] = []
    response.data.map(function (value: any) {
      const reference = new Reference(value.reference)
      value.matches.map(function (proj: any) {
        const project = new Project(proj.name, proj.version)
        reference.projects.push(project)
      })
      references.push(reference)
    })
    return references
  }
}
</script>
