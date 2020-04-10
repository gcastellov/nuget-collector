import { Reference } from './Reference'

export class Project {
    references: Reference[] = []
    constructor (public name: string) {
      this.name = name ?? ''
    }

    public getId (): string {
      return `by-project-${this.name}`
    }
}
