import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async getAllSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
  static async GetSolarSystemById(id: number) {
    return FetchAPIService.post('/SolarSystem/GetSolarSystemById', id).then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }

  static async AddSpaceObject(type: string) {
    return FetchAPIService.post('/SolarSystem/AddSpaceObject', type).then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }

  static async GetPublicSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetPublicSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
}