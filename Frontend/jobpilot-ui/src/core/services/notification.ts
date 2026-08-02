import { Injectable, inject } from '@angular/core';

import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class Notification {

  private message = inject(MessageService, { optional: true });

  success(summary: string, detail: string) {

    this.message?.add({
      severity: 'success',
      summary,
      detail,
      life: 3000
    });

  }

  error(summary: string, detail: string) {

    this.message?.add({
      severity: 'error',
      summary,
      detail,
      life: 4000
    });

  }

  warning(summary: string, detail: string) {

    this.message?.add({
      severity: 'warn',
      summary,
      detail,
      life: 4000
    });

  }

  info(summary: string, detail: string) {

    this.message?.add({
      severity: 'info',
      summary,
      detail,
      life: 3000
    });

  }

}
