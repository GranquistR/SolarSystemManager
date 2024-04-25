export default class ChangeCredRequest {
    username: string
    password: string
    newUsername: string
  
    constructor(username: string, password: string, newUsername: string) {
      this.username = username
      this.password = password
      this.newUsername = newUsername
    }
  }
  