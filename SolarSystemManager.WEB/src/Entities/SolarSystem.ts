export default class SolarSystem {
    systemId: number;
    ownerId: number;
    systemName: string;
    systemVisibility: number;

    constructor(systemId: number, ownerId: number, systemName: string, systemVisibility: number) {
      this.systemId = systemId
      this.ownerId = ownerId
      this.systemName = systemName
      this.systemVisibility = systemVisibility
    }
  }