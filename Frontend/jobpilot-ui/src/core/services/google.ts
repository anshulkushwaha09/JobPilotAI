import { Injectable } from '@angular/core';

import { GoogleConfig } from '../config/google.config';

declare const google: any;

@Injectable({
  providedIn: 'root'
})
export class Google {

  initialize(callback: (token: string) => void): boolean {
    const googleApi = (globalThis as any).google;

    if (!googleApi?.accounts?.id) {
      return false;
    }

    googleApi.accounts.id.initialize({

      client_id: GoogleConfig.ClientId,

      callback: (response: any) => {

        callback(response.credential);

      }

    });

    return true;

  }

  renderButton(element: HTMLElement): boolean {
    const googleApi = (globalThis as any).google;

    if (!googleApi?.accounts?.id) {
      return false;
    }

    googleApi.accounts.id.renderButton(

      element,

      {

        theme: 'outline',

        size: 'large',

        shape: 'pill',

        width: 350

      }

    );

    return true;

  }

}
