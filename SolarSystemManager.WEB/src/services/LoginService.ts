import UserRequest from '@/Entities/UserRequest'
import FetchAPIService from './FetchAPIService'
import CreateUserRequest from '@/Entities/CreateUserRequest'
import ChangeUsernameRequest from '@/Entities/ChangeCredRequest'
import ChangeCredRequest from '@/Entities/ChangeCredRequest'
import EncryptedMessage from '@/Entities/encrypted'
import EncryptionModule from '@/services/encryption'
export default class LoginService {
  static async Login(user: UserRequest) {
    const message = EncryptionModule.eRSA(JSON.stringify(user));
    const eMessage: EncryptedMessage = new EncryptedMessage(message.coded, message.privateKey, message.n); 
    return FetchAPIService.post('/User/Login', eMessage)
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
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetUserCount(): Promise<any> {
    return FetchAPIService.get('/User/GetUserCount')
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in LoginService: ', error)
      })
  }

  static async GetSalt(username: string): Promise<any> {
    const message = EncryptionModule.eRSA(username);
    const encMessage = new EncryptedMessage(message.coded, message.privateKey, message.n);
    return FetchAPIService.post('/User/GetSalts', encMessage)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in GetSaltService. Check console for details.')
        console.error('Error in GetSaltService: ', error)
      })
  }


  static async ChangeUsername(userdata: ChangeCredRequest): Promise<any> {
    return FetchAPIService.post('/User/ChangeUserName', userdata)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in ChangeUsername: ', error)
      })
  }

  static async ChangePassword(userdata: ChangeCredRequest): Promise<any> {
    return FetchAPIService.post('/User/ChangePassword', userdata)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in ChangeUsername: ', error)
      })
  }
}

