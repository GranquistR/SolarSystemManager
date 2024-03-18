export default class UserV2 {
  userID: number
  username: string
  password: string
  role: number

  constructor(userID: number, username: string, password: string, role: number) {
    this.userID = userID
    this.username = username
    this.password = password
    this.role = role
  }
}
