
import type User from '@/Entities/UserLogin'
import FetchAPIService from './FetchAPIService'

export default class SettingsService {
    static async getUserSettings(user: User): Promise<any> {
        return FetchAPIService.post('/GetSettings', user).then((data) => {
            console.log('SettingsService: ', data)
            return data
          })
          .catch((error) => {
            alert('Error in LoginService. Check console for details.')
            console.error('Error in LoginService: ', error)
          })
      }
}