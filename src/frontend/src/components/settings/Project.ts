export class Project {
  public status: string
  public isConnected: boolean

  constructor (public name: string, public isMapped: boolean) {
    this.name = name ?? ''
    this.isMapped = isMapped
    this.status = ''
    this.isConnected = false
  }
}
