import { Routes } from '@angular/router';
import { authGuard } from '../core/guards/auth-guard';
import { loginGuard } from '../core/guards/login.guard';


export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    canActivate: [loginGuard],
    loadComponent: () =>
      import('../features/auth/login/login')
        .then(m => m.LoginComponent)
  },

  {
    path: 'register',
    loadComponent: () =>
      import('../features/auth/register/register')
        .then(m => m.RegisterComponent)
  },

  {
    path: 'forgot-password',
    loadComponent: () =>
      import('../features/auth/forgot-password/forgot-password')
        .then(m => m.ForgotPasswordComponent)
  },

  {
    path: 'reset-password',
    loadComponent: () =>
      import('../features/auth/reset-password/reset-password')
        .then(m => m.ResetPassword)
  },

  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadChildren: () =>
      import('../features/dashboard/dashboard.routes')
        .then(m => m.dashboardRoutes)
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];
