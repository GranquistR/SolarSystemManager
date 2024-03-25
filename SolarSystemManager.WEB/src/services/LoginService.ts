import UserRequest from '@/Entities/UserRequest'
import FetchAPIService from './FetchAPIService'
import CreateUserRequest from '@/Entities/CreateUserRequest'

export default class LoginService {
  static async Login(user: UserRequest) {
    return FetchAPIService.post('/User/Login', user)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetUserSettings(user: UserRequest): Promise<any> {
    return FetchAPIService.post('/User/GetUserSettings', user)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async CreateAccount(user: CreateUserRequest): Promise<any> {
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

  static async GetSalt(username: string) {
    return FetchAPIService.post('/User/GetSalts', username)
      .then((data) => {
        console.log(data)
        return data
      })
      .catch((error) => {
        alert('Error in GetSaltService. Check console for details.')
        console.error('Error in GetSaltService: ', error)
      })
  }
}
