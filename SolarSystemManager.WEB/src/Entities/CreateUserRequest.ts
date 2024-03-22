//export a class called UserLogin
export default class CreateUserRequest {
  username: string
  password: string
  salt: string

  constructor(username: string, password: string, salt: string) {
    this.username = username
    this.password = password
    this.salt = salt
  }
}
