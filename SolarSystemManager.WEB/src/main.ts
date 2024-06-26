//global styles
import './assets/main.css'

//importing vue and router
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

//importing primevue and children
import PrimeVue from 'primevue/config'
import 'primevue/resources/themes/aura-dark-green/theme.css'
import '/node_modules/primeflex/primeflex.css'
import 'primeicons/primeicons.css'
import Tooltip from 'primevue/tooltip'
import EncryptionModule from './services/encryption'
const app = createApp(App)

//directives
app.directive('tooltip', Tooltip)

//begin app
app.use(PrimeVue)
app.use(router)

//login dependency injection
//mounts app after checking if user is logged in
import LoginService from './services/LoginService'
import User from './Entities/User'
import LoginRequest from './Entities/UserRequest'
if (document.cookie.includes('username') && document.cookie.includes('password')) {
  const user = document.cookie.split('username=')[1].split(';')[0]
  const pass = document.cookie.split('password=')[1].split(';')[0]
  LoginService.Login(new LoginRequest(user, pass)).then((response) => {
    if (response.success) {
      response.data = JSON.parse(EncryptionModule.dRSA(response.data.message, response.data.key, response.data.n));
      app.provide(
        'currentUser',
        new User(
          response.data.userID,
          response.data.username,
          response.data.role,
          response.data.password
        )
      )
    } else {
      document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
      document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
    }
  }).finally(() => {
      app.mount('#app')
  })
} else {
  document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
  app.mount('#app')
}
