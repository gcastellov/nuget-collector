export class Reference {
  constructor (public name: string, public version: string) {
    this.name = name ?? ''
    this.version = version ?? ''
  }

  public getId (): string {
    return `by-project-${this.name}-${this.version}`
  }
}
