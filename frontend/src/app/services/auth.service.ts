import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';

const TOKEN_KEY = 'badminton_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  async loginWithGoogle(idToken: string): Promise<void> {
    const res = await firstValueFrom(
      this.http.post<{ token: string }>(`${environment.apiUrl}/api/auth/google`, { idToken })
    );
    localStorage.setItem(TOKEN_KEY, res.token);
    this.router.navigate(['/leaderboard']);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['is_admin'] === 'true';
    } catch {
      return false;
    }
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    this.router.navigate(['/login']);
  }
}
