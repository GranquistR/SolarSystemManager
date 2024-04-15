export default class SolarSystem {
    systemId: number;
    ownerId: number;
    systemName: string;
    systemVisibility: string;

    constructor(systemId: number, ownerId: number, systemName: string, systemVisibility: string) {
      this.systemId = systemId
      this.ownerId = ownerId
      this.systemName = systemName
      this.systemVisibility = systemVisibility
    }
  }