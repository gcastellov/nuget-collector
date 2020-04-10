import { Project } from './Project'

export class Reference {
    public projects: Project[] = []
    constructor (public name: string) {
      this.name = name ?? ''
    }

    public getId (): string {
      return 'by-reference' + this.name
    }
}
