import { Component, inject, signal } from '@angular/core';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router, RouterLink } from '@angular/router';

import { Auth } from '../../../core/services/auth';
import { Notification } from '../../../core/services/notification';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './register.html'
})
export class RegisterComponent {

  private fb = inject(NonNullableFormBuilder);

  private auth = inject(Auth);

  private router = inject(Router);
    private notification = inject(Notification);

  loading = signal(false);

  form = this.fb.group({

    fullName: ['', Validators.required],

    email: ['', [Validators.required, Validators.email]],

    password: ['', [Validators.required, Validators.minLength(6)]],

    confirmPassword: ['', Validators.required]

  });

  register() {

    if (this.form.invalid) {

      this.form.markAllAsTouched();

      return;

    }

    if (this.form.value.password !== this.form.value.confirmPassword) {

      alert('Passwords do not match');

      return;

    }

    this.loading.set(true);

    this.auth.register({

      fullName: this.form.getRawValue().fullName,

      email: this.form.getRawValue().email,

      password: this.form.getRawValue().password

    }).subscribe({

      next: () => {

        this.loading.set(false);

        this.notification.success(
  'Success',
  'Account Created Successfully'
);

        this.router.navigate(['/login']);

      },

      error: err => {

        this.loading.set(false);

        this.notification.error(
  'Registration Failed',
  err.error.message
);

      }

    });

  }

}

export { RegisterComponent as Register };
