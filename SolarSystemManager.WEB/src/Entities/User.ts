//export a class called UserLogin
export default class User {
  userID: number
  username: string
  role: number
  password: string

  constructor(userID: number, username: string, role: number, password: string) {
    this.userID = userID
    this.username = username
    this.role = role
    this.password = password
  }
}
