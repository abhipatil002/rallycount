import { inject, Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { LeaderboardEntry } from '../models/leaderboard.model';
import { Period } from '../models/period.model';

@Injectable({ providedIn: 'root' })
export class LeaderboardService {
  private api = inject(ApiService);

  getLeaderboard(periodId: string): Promise<LeaderboardEntry[]> {
    return this.api.get(`/api/leaderboard?periodId=${periodId}`);
  }

  getPeriods(): Promise<Period[]> {
    return this.api.get('/api/periods');
  }
}
