import type SpaceObject from './SpaceObject'

export default class SolarSystem {
  systemId: number
  ownerId: number
  systemName: string
  systemVisibility: number
  spaceObjects: SpaceObject[] = []

  constructor(systemId: number, ownerId: number, systemName: string, systemVisibility: number) {
    this.systemId = systemId
    this.ownerId = ownerId
    this.systemName = systemName
    this.systemVisibility = systemVisibility
  }
}
