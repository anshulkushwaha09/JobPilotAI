import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject, signal, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { Auth } from '../../../core/services/auth';
import { Storage } from '../../../core/services/storage';
import { Token } from '../../../core/services/token';
import { Notification } from '../../../core/services/notification';
import { Google } from '../../../core/services/google';
import { CommonModule } from '@angular/common';
import { AuthLayoutComponent } from '../layout/auth-layout/auth-layout';
import { AuthCardComponent } from '../components/auth-card/auth-card';

@Component({
  selector: 'app-login',

  standalone: true,

  imports: [CommonModule, ReactiveFormsModule, RouterModule],

  templateUrl: './login.html',
})
export class LoginComponent implements AfterViewInit {
  @ViewChild('googleButton', { static: true })
  googleButton!: ElementRef;
  private fb = inject(NonNullableFormBuilder);

  private auth = inject(Auth);

  private router = inject(Router);
  private notification = inject(Notification);
  // private social = inject(SocialAuthService);
  private storage = inject(Storage);
  private token = inject(Token);
  private google = inject(Google);

  ngAfterViewInit() {
    const googleReady = this.google.initialize((idToken) => {
      this.loading.set(true);

      this.auth.loginWithGoogle(idToken).subscribe({
        next: (response) => {
          this.loading.set(false);

          this.token.saveTokens(response.data.accessToken, response.data.refreshToken);

          this.storage.setUser(response.data);

          this.notification.success('Welcome', response.message);

          this.router.navigate(['/dashboard']);
        },

        error: (err) => {
          this.loading.set(false);

          this.notification.error('Login Failed', err.error?.message ?? 'Google login failed.');
        },
      });
    });

    if (!googleReady) {
      return;
    }

    this.google.renderButton(this.googleButton.nativeElement);
  }

  loading = signal(false);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],

    password: ['', Validators.required],
  });

  login() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();

      return;
    }

    this.loading.set(true);

    this.auth.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        this.loading.set(false);

        this.token.saveTokens(
          response.data.accessToken,

          response.data.refreshToken,
        );

        this.storage.setUser(response.data);

        this.notification.success(
          'Success',

          'Welcome Back',
        );

        this.router.navigate(['/dashboard']);
      },

      error: (err) => {
        this.loading.set(false);

        this.notification.error(
          'Login Failed',

          err.error.message,
        );
      },
    });
  }

  //   loginWithGoogle() {
  //     this.social
  //       .signIn(GoogleLoginProvider.PROVIDER_ID)
  //       .then((user) => {
  //         if (!user?.idToken) {
  //           this.notification.error('Google Login', 'Unable to retrieve Google token.');
  //           return;
  //         }

  //         this.auth.loginWithGoogle(user.idToken).subscribe({
  //           next: (response) => {
  //             this.token.saveTokens(response.data.accessToken, response.data.refreshToken);

  //             this.storage.setUser(response.data);

  //             this.notification.success('Welcome', response.message);

  //             this.router.navigate(['/dashboard']);
  //           },

  //           error: (err) => {
  //             this.notification.error('Login Failed', err.error?.message ?? 'Google login failed.');
  //           },
  //         });
  //       })
  //       .catch((err) => {

  //   console.log(err);

  //   this.notification.error(
  //     'Google Login',
  //     JSON.stringify(err)
  //   );

  // });
  //   }
}

export { LoginComponent as Login };
