export default class SolarSystemRequest {
    username: string
    password: string
    dSolarSystemID: number
  
    constructor(username: string, password: string, dSolarSystemID: number) {
      this.username = username
      this.password = password
      this.dSolarSystemID = dSolarSystemID
    }
  }