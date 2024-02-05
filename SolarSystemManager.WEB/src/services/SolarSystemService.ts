import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static GetAllSolarSystems(): any {
    return FetchAPIService.get('/SolarSystem/GetAllSolarSystems')
  }
}
