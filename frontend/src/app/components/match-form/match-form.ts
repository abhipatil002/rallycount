import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { LeaderboardService } from '../../services/leaderboard.service';
import { Player } from '../../models/player.model';

@Component({
  selector: 'app-match-form',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './match-form.html',
  styleUrl: './match-form.scss'
})
export class MatchFormComponent implements OnInit {
  private matchService = inject(MatchService);
  private lbService = inject(LeaderboardService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  players: Player[] = [];
  activePeriodId = '';
  error = '';

  form = { team1P1: '', team1P2: '', team2P1: '', team2P2: '', team1Score: 0, team2Score: 0 };

  async ngOnInit(): Promise<void> {
    [this.players] = await Promise.all([this.matchService.getPlayers()]);
    const periods = await this.lbService.getPeriods();
    this.activePeriodId = periods.find(p => p.isActive)?.id ?? '';
    this.cdr.detectChanges();
  }

  get selectedIds(): string[] {
    return [this.form.team1P1, this.form.team1P2, this.form.team2P1, this.form.team2P2].filter(Boolean);
  }

  available(exclude: string[]): Player[] {
    return this.players.filter(p => !exclude.includes(p.id));
  }

  async submit(): Promise<void> {
    const { team1P1, team1P2, team2P1, team2P2, team1Score, team2Score } = this.form;
    const ids = [team1P1, team1P2, team2P1, team2P2];
    if (ids.some(id => !id) || new Set(ids).size < 4) {
      this.error = 'Select 4 distinct players.';
      this.cdr.detectChanges();
      return;
    }
    if (!this.activePeriodId) {
      this.error = 'No active period. Ask admin to create one.';
      this.cdr.detectChanges();
      return;
    }
    await this.matchService.createMatch({ periodId: this.activePeriodId, team1P1, team1P2, team2P1, team2P2, team1Score, team2Score });
    this.router.navigate(['/leaderboard']);
  }
}
