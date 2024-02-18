import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async getAllSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/TestGet').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
  static async getSolarSystemById(id: number) {
    return FetchAPIService.post('/SolarSystem/TestPost', id).then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }

  static async GetPublicSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllPublicSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
}
