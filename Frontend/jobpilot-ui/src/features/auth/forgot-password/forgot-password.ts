import { Component, inject, signal } from '@angular/core';

import { ReactiveFormsModule, NonNullableFormBuilder, Validators } from '@angular/forms';
import { Auth } from '../../../core/services/auth';
import { Notification } from '../../../core/services/notification';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterModule],
  templateUrl: './forgot-password.html',
})
export class ForgotPasswordComponent {
  private fb = inject(NonNullableFormBuilder);
  private auth = inject(Auth);
  private notification = inject(Notification);

  loading = signal(false);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();

      return;
    }

    this.loading.set(true);

    this.auth.forgotPassword(this.form.getRawValue().email).subscribe({
      next: (response) => {
        this.loading.set(false);

        this.notification.success('Email Sent', response.message);
      },

      error: (err) => {
        this.loading.set(false);

        this.notification.error('Error', err.error.message);
      },
    });
  }
}

export { ForgotPasswordComponent as ForgotPassword };
