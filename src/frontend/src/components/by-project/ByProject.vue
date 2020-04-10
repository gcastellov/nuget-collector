<template>
  <div>
      <div v-for="project in this.projects" :key="project.getId()" class="project">
        <ul v-if="project.references.length > 0" class="list-group">
          <li class="list-group-item d-flex justify-content-between align-items-center">
            {{project.name}}
            <span class="badge badge-primary badge-pill">{{project.references.length}}</span>
          </li>
          <li class="list-group-item d-flex justify-content-between align-items-center list-item-reference" v-for="reference in project.references" :key="project.name + reference.getId()">
            <small>{{reference.name}}</small>
            <span class="badge badge-secondary">{{reference.version}}</span>
          </li>
        </ul>
      </div>
  </div>
</template>

<style lang="scss">
  .project {
    margin-top: 10px;
    margin-bottom: 30px;
    padding-left: 10%;
    padding-right: 10%;
  }
  .list-item-reference small {
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
export default class ByProject extends Vue {
  public projects: Project[] = []

  public async created () {
    this.load()
  }

  private mapToProjects (response: any): Project[] {
    const projects: Project[] = []
    response.data.map(function (value: any) {
      const project = new Project(value.name)
      value.references.map(function (ref: any) {
        const reference = new Reference(ref.name, ref.version)
        project.references.push(reference)
      })
      projects.push(project)
    })
    return projects
  }

  private async load () {
    const config: AxiosRequestConfig = {
      headers: { 'Access-Control-Allow-Origin': '*' }
    }

    Vue.axios.get(`${Vue.prototype.$baseUrl}/api/projects`, config).then((r) => {
      this.projects = this.mapToProjects(r)
    })
  }
}
</script>
