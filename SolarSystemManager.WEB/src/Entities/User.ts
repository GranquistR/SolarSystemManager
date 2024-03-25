//export a class called UserLogin
export default class User {
  userID: number
  username: string
  role: number

  constructor(userID: number, username: string, role: number) {
    this.userID = userID
    this.username = username
    this.role = role
  }
}
