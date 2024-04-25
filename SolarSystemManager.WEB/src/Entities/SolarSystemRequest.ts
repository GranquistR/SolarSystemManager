export default class SolarSystemRequest {
    username: string
    password: string
    systemID: number
  
    constructor(username: string, password: string, systemID: number) {
      this.username = username
      this.password = password
      this.systemID = systemID
    }
  }
  