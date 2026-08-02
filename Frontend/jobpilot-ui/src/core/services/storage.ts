import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Storage {

  private readonly USER_KEY = 'current_user';

  setUser(user: any): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  getUser() {
    const data = localStorage.getItem(this.USER_KEY);
    return data ? JSON.parse(data) : null;
  }

  clearUser(): void {
    localStorage.removeItem(this.USER_KEY);
  }
}