import { Routes } from '@angular/router';

export const dashboardRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layout/app-layout/app-layout')
        .then(m => m.AppLayoutComponent),

    children: [
      {
        path: '',
        loadComponent: () =>
          import('./dashboard-home/dashboard-home')
            .then(m => m.DashboardHomeComponent)
      }
    ]
  }
];