import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = environment.apiUrl;

  get<T>(path: string): Promise<T> {
    return firstValueFrom(this.http.get<T>(`${this.base}${path}`));
  }

  post<T>(path: string, body: unknown): Promise<T> {
    return firstValueFrom(this.http.post<T>(`${this.base}${path}`, body));
  }

  put(path: string, body: unknown = {}): Promise<void> {
    return firstValueFrom(
      this.http.put(`${this.base}${path}`, body, { observe: 'response', responseType: 'text' })
    ).then(() => undefined);
  }

  delete<T>(path: string): Promise<T> {
    return firstValueFrom(this.http.delete<T>(`${this.base}${path}`));
  }
}
