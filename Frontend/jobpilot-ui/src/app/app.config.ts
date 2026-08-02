import { ApplicationConfig } from '@angular/core';

import { provideRouter } from '@angular/router';

import {
  provideHttpClient,
  withInterceptors
} from '@angular/common/http';

import { provideAnimations } from '@angular/platform-browser/animations';

import { providePrimeNG } from 'primeng/config';

import Aura from '@primeuix/themes/aura';

import { MessageService } from 'primeng/api';

import { routes } from './app.routes';

import { authInterceptor } from '../core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [

    provideRouter(routes),

    provideAnimations(),

    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),

    provideHttpClient(
      withInterceptors([
        authInterceptor
      ])
    ),

    MessageService

  ]
};