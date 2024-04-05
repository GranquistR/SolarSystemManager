import CreateSolarSystemRequest from '@/Entities/CreateSolarSystemRequest'
import FetchAPIService from './FetchAPIService'
import SpaceObject from '@/Entities/SpaceObject'
import type User from '@/Entities/User'

export default class SolarSystemService {
  static async GetPublicSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllPublicSolarSystems')
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetSolarSystemCount(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetSolarSystemCount')
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetSpaceObjectCount(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetSpaceObjectCount')
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetSolarSystemByID(id: number): Promise<any> {
    return FetchAPIService.get(`/SolarSystem/GetSolarSystemByID?id=${id}`).then((data) => {
      return JSON.parse(data)
    })
  }

  static async AddSpaceObject(object: SpaceObject): Promise<any> {
      return FetchAPIService.post(`/SolarSystem/AddSpaceObject`, object).then((data) => {
          return JSON.parse(data);
      })
      .catch((error) => {
          alert('Error in AddSpaceObject. Check console for details.')
          console.error('Error in AddSpaceObject: ', error)
      })
  }

  static async RemoveSpaceObject(id: number): Promise<any> {
    return FetchAPIService.get(`/SolarSystem/RemoveSpaceObject?id=${id}`).then((data) => {
      return JSON.parse(data)
    })
  }

  static async CreateSolarSystem(systemName: string, isPrivate: boolean, user: User): Promise<any> {
    let visibility = 0
    if (isPrivate) {
      visibility = 1
    }
    const data = new CreateSolarSystemRequest(systemName, visibility, user)
    return FetchAPIService.post('/SolarSystem/CreateSolarSystem', data).then((data) => {
      return JSON.parse(data)
    })
  }

  static async GetAllSolarSystemsAdmin(user: User): Promise<any> {
    return FetchAPIService.post('/SolarSystem/GetAllSolarSystemsAdmin', user).then((data) => {
      return JSON.parse(data)
    })
  }
}
