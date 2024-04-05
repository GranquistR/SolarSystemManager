import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'landingpage',
      component: () => import('../views/LandingPageView.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/signup',
      name: 'signup',
      component: () => import('../views/SignUpView.vue')
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: () => import('../views/DashboardView.vue')
    },
    {
      path: '/systemeditor',
      name: 'systemeditor',
      component: () => import('../views/SystemEditorView.vue')
    },
    {
      path: '/settings',
      name: 'settings',
      component: () => import('../views/SettingsView.vue')
    },
    {
      path: '/unauthorized',
      name: 'unauthorized',
      component: () => import('../views/UnauthorizedView.vue')
    },
    {
      path: '/editor',
      name: 'editor',
      component: () => import('../views/SystemEditorView.vue')
    },
    {
      path: '/viewer/:id',
      name: 'solarsystemviewer',
      component: () => import('../views/SolarSystemViewerView.vue')
    },
    {
      path: '/new',
      name: 'newsolarsystem',
      component: () => import('../views/AddSolarSystemView.vue')
    },
    {
      path: '/notfound',
      name: 'notfound',
      component: () => import('../views/NotFoundView.vue')
    },
    {
      path: '/forbidden',
      name: 'forbidden',
      component: () => import('../views/ForbiddenView.vue')
    },
    {
      path: '/admin',
      name: 'admin',
      component: () => import('../views/AdminView.vue')
    },
    {
      path: '/:catchAll(.*)',
      redirect: '/notfound'
    }
  ]
})
export default router
