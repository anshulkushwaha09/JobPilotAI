import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Api } from './api';
import { ApiEndpoints } from '../constants/api-endpoints';

import { LoginRequest } from '../models/login-request';
import { RegisterRequest } from '../models/register-request';

import { ApiResponse } from '../models/api-response';
import { AuthResponse } from '../models/auth-response';
import { Token } from './token';
import { Storage } from './storage';
import { Router } from '@angular/router';
// import {
// SocialAuthService,
// GoogleLoginProvider
// } from '@abacritt/angularx-social-login';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private api = inject(Api);
  private token = inject(Token);

  private storage = inject(Storage);

  private router = inject(Router);
  // private socialAuth = inject(SocialAuthService);

  login(request: LoginRequest) {
    return this.api.post<ApiResponse<AuthResponse>>(ApiEndpoints.Auth.Login, request);
  }

  register(request: RegisterRequest) {
    return this.api.post<ApiResponse<AuthResponse>>(ApiEndpoints.Auth.Register, request);
  }

  // googleLogin(idToken: string) {
  //   return this.api.post(ApiEndpoints.Auth.Google, {
  //     idToken,
  //   });
  // }

  refreshToken(refreshToken: string) {
    return this.api.post(ApiEndpoints.Auth.Refresh, {
      refreshToken,
    });
  }

  logout(): void {
    this.token.clear();

    this.storage.clearUser();

    this.router.navigate(['/login']);
  }

  forgotPassword(email: string) {

  return this.api.post<ApiResponse<any>>(
    ApiEndpoints.Auth.ForgotPassword,
    {
      email: email
    }
  );

}

// googleLogin() {

//     return this.socialAuth.signIn(
//         GoogleLoginProvider.PROVIDER_ID
//     );

// }

loginWithGoogle(idToken: string) {

  return this.api.post<ApiResponse<AuthResponse>>(

    ApiEndpoints.Auth.GoogleLogin,

    {

      idToken

    }

  );

}

}
