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

const app = createApp(App)

//dependency injection

///login dependency injection
import LoginService from './services/LoginService'
import User from './Entities/User'
import LoginRequest from './Entities/UserRequest'

if (document.cookie.includes('username') && document.cookie.includes('password')) {
  const user = document.cookie.split('username=')[1].split(';')[0]
  const pass = document.cookie.split('password=')[1].split(';')[0]
  LoginService.Login(new LoginRequest(user, pass)).then((response) => {
    if (response.success) {
      app.provide(
        'currentUser',
        new User(response.data.userID, response.data.username, response.data.role)
      )
    } else {
      document.cookie = `username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
      document.cookie = `password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`
    }
  })
}

//begin app
app.use(PrimeVue)
app.use(router)
app.mount('#app')
