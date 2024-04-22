import type SpaceObject from './SpaceObject'
import type UserRequest from './UserRequest'

export default class AddSpaceObjectRequest {
  credentials: UserRequest
  spaceObject: SpaceObject

  constructor(credentials: UserRequest, spaceObject: any) {
    this.credentials = credentials
    this.spaceObject = spaceObject
  }
}
