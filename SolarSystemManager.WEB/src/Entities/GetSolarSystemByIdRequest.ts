import type UserRequest from './UserRequest'

export default class GetSolarSystemByIdRequest {
  solarSystemID: number
  credentials: UserRequest

  constructor(solarSystemID: number, credentials: any) {
    this.solarSystemID = solarSystemID
    this.credentials = credentials
  }
}
