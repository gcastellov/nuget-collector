<template>
  <div class="container">
    <button class="btn btn-primary" @click="sync()">Sync repositories</button>
    <p class="status">{{status}}</p>
    <table class="table">
      <thead>
        <tr>
          <th scope="col">Project name</th>
          <th scope="col">Status</th>
          <th scope="col">Working directory</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="project in projects" :key="project.name">
          <td>{{project.name}}</td>
          <td>{{project.status}}</td>
          <td>
            <span v-if="project.isMapped" class="badge badge-primary badge-success">MAPPED</span>
            <span v-if="!project.isMapped" class="badge badge-primary badge-warning">NOT MAPPED</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style lang="scss">
  .table {
    margin-top: 10px;
    margin-bottom: 30px;
  }
  .container {
    padding-left: 10%;
    padding-right: 10%;
  }
  .status {
    padding: 10px;
    font-style: italic;
  }
</style>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import axios, { AxiosRequestConfig } from 'axios'
import VueAxios from 'vue-axios'
import { Project } from './Project'
Vue.use(VueAxios, axios)

@Component
export default class Settings extends Vue {
  public projects: Project[] = []
  public status: string

  constructor () {
    super()
    this.status = '...'
  }

  public created () {
    this.projects = this.load()
    this.connectSingalr()
  }

  public connectSingalr () {
    Vue.prototype.$codeHub.$on('status-changed', (notification: any) => {
      this.refresh(notification)
    })
    Vue.prototype.$codeHub.$on('message-changed', (message: string) => {
      this.status = message
    })
  }

  public updated () {
    this.projects.forEach(project => {
      if (!project.isConnected) {
        Vue.prototype.$codeHub.connectionOpenned(project.name)
        project.isConnected = true
      }
    })
  }

  public destroyed () {
    this.projects.map(function (project) {
      Vue.prototype.$codeHub.connectionClosed(project.name)
    })
  }

  public sync () {
    this.projects.forEach(project => {
      project.status = 'Starting ...'
    })

    const config: AxiosRequestConfig = {
      headers: { 'Access-Control-Allow-Origin': '*' }
    }
    Vue.axios.post(`${Vue.prototype.$baseUrl}/api/analyze`, config).then((r) => {
      console.log('analyzing...')
    })
  }

  private refresh (notification: any) {
    const index = this.projects.findIndex(p => p.name === notification.name)
    if (index > -1) {
      const project = this.projects[index]
      const newProject = new Project(project.name, project.isMapped)
      newProject.status = notification.message
      newProject.isConnected = project.isConnected
      Vue.set(this.projects, index, newProject)
    }
  }

  private load (): Project[] {
    const projects: Project[] = []
    const config: AxiosRequestConfig = {
      headers: { 'Access-Control-Allow-Origin': '*' }
    }
    Vue.axios.get(`${Vue.prototype.$baseUrl}/api/settings`, config).then((r) => {
      r.data.repositories.map(function (value: any) {
        const project = new Project(value.name, value.isMapped)
        projects.push(project)
      })
    })
    return projects
  }
}
</script>
