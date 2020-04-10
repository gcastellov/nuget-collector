import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
import { VueConstructor } from 'vue'

export default {
  install (Vue: VueConstructor) {
    let isConnected = false
    const groups: string[] = []
    const connection = new HubConnectionBuilder()
      .withUrl(`${Vue.prototype.$baseUrl}/code-hub`)
      .configureLogging(LogLevel.Information)
      .build()

    connection.start().then(() => {
      console.log('ws connected')
      isConnected = true
      groups.forEach(group => {
        Vue.prototype.$codeHub.connectionOpenned(group)
      })
    }).catch(e => {
      console.error('error while connecting to ws', e)
    })

    connection.on('SendStatus', (args: any) => {
      Vue.prototype.$codeHub.$emit('status-changed', args)
    })

    connection.on('SendMessage', (args: any) => {
      Vue.prototype.$codeHub.$emit('message-changed', args)
    })

    Vue.prototype.$codeHub.connectionOpenned = (name: string) => {
      if (!isConnected) {
        groups.push(name)
        return
      }
      console.log('joining ' + name)
      return connection.invoke('JoinGroup', name)
    }

    Vue.prototype.$codeHub.connectionClosed = (name: string) => {
      console.log('leaving ' + name)
      return connection.invoke('LeaveGroup', name)
    }
  }
}
