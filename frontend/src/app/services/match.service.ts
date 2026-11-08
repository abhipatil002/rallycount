import { inject, Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Match, CreateMatchDto } from '../models/match.model';
import { Player } from '../models/player.model';

@Injectable({ providedIn: 'root' })
export class MatchService {
  private api = inject(ApiService);

  getMatches(periodId: string): Promise<Match[]> {
    return this.api.get(`/api/matches?periodId=${periodId}`);
  }

  createMatch(dto: CreateMatchDto): Promise<Match> {
    return this.api.post('/api/matches', dto);
  }

  deleteMatch(id: string): Promise<void> {
    return this.api.delete(`/api/matches/${id}`);
  }

  getPlayers(): Promise<Player[]> {
    return this.api.get('/api/players');
  }
}
