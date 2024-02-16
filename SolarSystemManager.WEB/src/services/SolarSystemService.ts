import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async getAllSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
  static async getSolarSystemById(id: number) {
    return FetchAPIService.post('/SolarSystem/GetSolarSystemById', id).then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }

  static async SolarSystemInfoTest(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/SolarSystemInfoTest').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
}