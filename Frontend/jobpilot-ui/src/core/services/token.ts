import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Token {

  private readonly ACCESS = 'access_token';
  private readonly REFRESH = 'refresh_token';

  saveTokens(access: string, refresh: string): void {
    localStorage.setItem(this.ACCESS, access);
    localStorage.setItem(this.REFRESH, refresh);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH);
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  clear(): void {
    localStorage.removeItem(this.ACCESS);
    localStorage.removeItem(this.REFRESH);
  }
}