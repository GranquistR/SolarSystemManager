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
import { provide } from 'vue'
import LoginService from './services/LoginService'
import User from './Entities/UserLogin'
import UserV2 from './Entities/UserV2'

if (document.cookie.includes('username') && document.cookie.includes('password')) {
  const user = document.cookie.split('username=')[1].split(';')[0]
  const pass = document.cookie.split('password=')[1].split(';')[0]
  LoginService.LoginV2(new User(user, pass)).then((response) => {
    console.log(response)
    if (response.success) {
      app.provide(
        'currentUser',
        new UserV2(
          response.data.userID,
          response.data.username,
          response.data.password,
          response.data.role
        )
      )
    }
  })
}

//begin app
app.use(PrimeVue)
app.use(router)
app.mount('#app')
