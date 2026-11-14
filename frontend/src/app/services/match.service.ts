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

  updateMatch(id: string, team1Score: number, team2Score: number): Promise<void> {
    return this.api.put(`/api/matches/${id}`, { team1Score, team2Score });
  }

  deleteMatch(id: string): Promise<void> {
    return this.api.delete(`/api/matches/${id}`);
  }

  getPlayers(): Promise<Player[]> {
    return this.api.get('/api/players');
  }

  getMatchPointConfigs(): Promise<number[]> {
    return this.api.get('/api/match-point-configs');
  }
}
