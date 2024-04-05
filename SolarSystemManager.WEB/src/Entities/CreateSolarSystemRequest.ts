import type User from './User'

export default class CreateSolarSystemRequest {
  solarSystem: {
    systemName: string
    systemVisibility: number
  }
  credentials: {
    username: string
    password: string
  }

  constructor(systemName: string, systemVisibility: number, user: User) {
    this.solarSystem = {
      systemName,
      systemVisibility
    }
    this.credentials = {
      username: user.username,
      password: user.password
    }
  }
}
