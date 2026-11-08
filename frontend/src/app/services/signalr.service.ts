import { inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { LeaderboardEntry } from '../models/leaderboard.model';

@Injectable({ providedIn: 'root' })
export class SignalrService {
  private auth = inject(AuthService);
  private connection: HubConnection | null = null;

  async connect(periodId: string, onUpdate: (data: LeaderboardEntry[]) => void): Promise<void> {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/badminton`, {
        accessTokenFactory: () => this.auth.getToken() ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on('LeaderboardUpdated', onUpdate);
    await this.connection.start();
    await this.connection.invoke('JoinPeriod', periodId);
  }

  async disconnect(periodId: string): Promise<void> {
    if (!this.connection) return;
    await this.connection.invoke('LeavePeriod', periodId);
    await this.connection.stop();
    this.connection = null;
  }
}
