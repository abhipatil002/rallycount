import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { LeaderboardService } from '../../services/leaderboard.service';
import { AuthService } from '../../services/auth.service';
import { Match } from '../../models/match.model';
import { Player } from '../../models/player.model';
import { Period } from '../../models/period.model';

@Component({
  selector: 'app-match-history',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './match-history.html',
  styleUrl: './match-history.scss'
})
export class MatchHistoryComponent implements OnInit {
  private matchService = inject(MatchService);
  private lbService = inject(LeaderboardService);
  auth = inject(AuthService);

  matches: Match[] = [];
  players: Player[] = [];
  periods: Period[] = [];
  selectedPeriodId = '';

  async ngOnInit(): Promise<void> {
    [this.players, this.periods] = await Promise.all([
      this.matchService.getPlayers(),
      this.lbService.getPeriods()
    ]);
    const active = this.periods.find(p => p.isActive) ?? this.periods[0];
    if (!active) return;
    this.selectedPeriodId = active.id;
    await this.loadMatches();
  }

  async onPeriodChange(): Promise<void> { await this.loadMatches(); }

  private async loadMatches(): Promise<void> {
    this.matches = await this.matchService.getMatches(this.selectedPeriodId);
  }

  name(id: string): string {
    return this.players.find(p => p.id === id)?.name ?? '?';
  }

  async deleteMatch(id: string): Promise<void> {
    await this.matchService.deleteMatch(id);
    this.matches = this.matches.filter(m => m.id !== id);
  }
}
