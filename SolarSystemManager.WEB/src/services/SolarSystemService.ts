import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async testGet(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/TestGet').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
  static async testPost(id: number) {
    return FetchAPIService.post('/SolarSystem/TestPost', id).then((data) => {
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
    return FetchAPIService.get('/SolarSystem/GetAllPublicSolarSystems').then((data) => {
      return JSON.parse(data)
    })
  }
}
