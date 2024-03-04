import FetchAPIService from './FetchAPIService'

export default class SolarSystemService {
  static async testGet(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/TestGet')
      .then((data) => {
        console.log('SolarSystemService: ', data)
        return data
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }
  static async testPost(id: number) {
    return FetchAPIService.post('/SolarSystem/TestPost', id)
      .then((data) => {
        console.log('SolarSystemService: ', data)
        return data
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetPublicSolarSystems(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetAllPublicSolarSystems').then((data) => {
      console.log('SolarSystemService: ', data)
      return data
    })
    }

    static async GetSpaceObjects(): Promise<any> {
        return FetchAPIService.get('/SolarSystem/GetSpaceObjects').then((data) => {
            console.log('SolarSystemService: ', data)
            return data
        })
    }
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
        return data
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetSpaceObjectCount(): Promise<any> {
    return FetchAPIService.get('/SolarSystem/GetSpaceObjectCount')
      .then((data) => {
        return data
      })
      .catch((error) => {
        alert('Error in SolarSystemService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }
}
