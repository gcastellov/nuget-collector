export class Reference {
  public projects: string[] = []

  constructor (public name: string, public version: string, public counter: number) {
    this.name = name ?? ''
    this.version = version ?? ''
  }

  public getId (): string {
    return `by-version-${this.name}-${this.version}`
  }
}
