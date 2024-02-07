import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async getAllSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
  }
}
