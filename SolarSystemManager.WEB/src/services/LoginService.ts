import type User from '@/Entities/UserLogin'
import FetchAPIService from './FetchAPIService'

export default class LoginService {
  static async Login(user: User) {
    return FetchAPIService.post('/User/login', user)
      .then((data) => {
        return data
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetUserSettings(user: User): Promise<any> {
    return FetchAPIService.post('/User/GetUserSettings', user)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async CreateAccount(user: User): Promise<any> {
    return FetchAPIService.post('/User/CreateAccount', user)
      .then((data) => {
        return data
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetUserCount(): Promise<any> {
    return FetchAPIService.get('/User/GetUserCount')
      .then((data) => {
        return data
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }
}
