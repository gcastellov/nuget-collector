export class Project {
  constructor (public name: string, public version: string) {
    this.name = name ?? ''
    this.version = version ?? ''
  }

  public getId (): string {
    return 'by-reference' + this.name + this.version
  }
}
