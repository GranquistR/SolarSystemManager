import UserRequest from '@/Entities/UserRequest'
import FetchAPIService from './FetchAPIService'
import CreateUserRequest from '@/Entities/CreateUserRequest'
import ChangeUsernameRequest from '@/Entities/ChangeUsernameRequest'
import EncryptedMessage from '@/Entities/encrypted'
import EncryptionModule from '@/services/encryption'
import Keys from '@/Entities/Keys'
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
    alert(encMessage.getMessage());
    alert(encMessage.getKey()); 
    alert(encMessage.getN());
    return FetchAPIService.post('/User/GetSalts', encMessage)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in GetSaltService. Check console for details.')
        console.error('Error in GetSaltService: ', error)
      })
  }


  static async ChangeUsername(userdata: ChangeUsernameRequest): Promise<any> {
    return FetchAPIService.post('/User/ChangeUserName', userdata)
      .then((data) => {
        return JSON.parse(data)
      })
      .catch((error) => {
        alert('Error in LoginService. Check console for details.')
        console.error('Error in ChangeUsername: ', error)
      })
  }

  static async ChangePassword(userdata: ChangeUsernameRequest): Promise<any> {
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

