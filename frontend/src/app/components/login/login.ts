import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';

declare const google: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent implements OnInit {
  private auth = inject(AuthService);

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: environment.googleClientId,
      callback: (res: any) => this.auth.loginWithGoogle(res.credential)
    });
    google.accounts.id.renderButton(
      document.getElementById('google-btn'),
      { theme: 'outline', size: 'large', text: 'signin_with' }
    );
  }
}
