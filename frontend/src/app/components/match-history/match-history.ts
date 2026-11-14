import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
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
  private cdr = inject(ChangeDetectorRef);
  auth = inject(AuthService);

  matches: Match[] = [];
  players: Player[] = [];
  periods: Period[] = [];
  selectedPeriodId = '';
  editingId: string | null = null;
  editScores = { team1Score: 0, team2Score: 0 };

  async ngOnInit(): Promise<void> {
    [this.players, this.periods] = await Promise.all([
      this.matchService.getPlayers(),
      this.lbService.getPeriods()
    ]);
    const active = this.periods.find(p => p.isActive) ?? this.periods[0];
    if (!active) return;
    this.selectedPeriodId = active.id;
    await this.loadMatches();
    this.cdr.detectChanges();
  }

  async onPeriodChange(): Promise<void> {
    await this.loadMatches();
    this.cdr.detectChanges();
  }

  private async loadMatches(): Promise<void> {
    this.matches = await this.matchService.getMatches(this.selectedPeriodId);
  }

  name(id: string): string {
    return this.players.find(p => p.id === id)?.name ?? '?';
  }

  startEdit(m: Match): void {
    this.editingId = m.id;
    this.editScores = { team1Score: m.team1Score, team2Score: m.team2Score };
  }

  cancelEdit(): void {
    this.editingId = null;
  }

  async saveEdit(m: Match): Promise<void> {
    await this.matchService.updateMatch(m.id, this.editScores.team1Score, this.editScores.team2Score);
    this.editingId = null;
    await this.loadMatches();
    this.cdr.detectChanges();
  }

  async deleteMatch(id: string): Promise<void> {
    if (!confirm('Delete this match? It will remain visible but won\'t count towards scores.')) return;
    await this.matchService.deleteMatch(id);
    await this.loadMatches();
    this.cdr.detectChanges();
  }
}
