export class Data {
  constructor (public projectsCount: number, public allReferenceCount: number, public uniqueReferenceCount: number, public differentReferenceCount: number) {
    this.projectsCount = projectsCount ?? 0
    this.allReferenceCount = allReferenceCount ?? 0
    this.uniqueReferenceCount = uniqueReferenceCount ?? 0
    this.differentReferenceCount = differentReferenceCount ?? 0
  }
}
