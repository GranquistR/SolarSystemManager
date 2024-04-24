import type UserRequest from './UserRequest'

export default class AddSpaceObjectRequest {
  credentials: UserRequest
  spaceObject: any

  constructor(credentials: UserRequest, spaceObject: any) {
    this.credentials = credentials
    this.spaceObject = spaceObject
  }
}
