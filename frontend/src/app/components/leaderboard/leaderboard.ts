import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LeaderboardService } from '../../services/leaderboard.service';
import { SignalrService } from '../../services/signalr.service';
import { AuthService } from '../../services/auth.service';
import { LeaderboardEntry } from '../../models/leaderboard.model';
import { Period } from '../../models/period.model';

@Component({
  selector: 'app-leaderboard',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './leaderboard.html',
  styleUrl: './leaderboard.scss'
})
export class LeaderboardComponent implements OnInit, OnDestroy {
  private lbService = inject(LeaderboardService);
  private signalr = inject(SignalrService);
  private cdr = inject(ChangeDetectorRef);
  auth = inject(AuthService);

  periods: Period[] = [];
  selectedPeriodId = '';
  entries: LeaderboardEntry[] = [];

  async ngOnInit(): Promise<void> {
    this.periods = await this.lbService.getPeriods();
    const active = this.periods.find(p => p.isActive) ?? this.periods[0];
    if (!active) return;
    this.selectedPeriodId = active.id;
    await this.loadLeaderboard();
    this.cdr.detectChanges();
    await this.signalr.connect(this.selectedPeriodId, (data) => { this.entries = data; this.cdr.detectChanges(); });
  }

  async ngOnDestroy(): Promise<void> {
    if (this.selectedPeriodId) await this.signalr.disconnect(this.selectedPeriodId);
  }

  async onPeriodChange(): Promise<void> {
    await this.loadLeaderboard();
  }

  private async loadLeaderboard(): Promise<void> {
    this.entries = await this.lbService.getLeaderboard(this.selectedPeriodId);
  }

  get loser(): LeaderboardEntry | null {
    return this.entries.length ? this.entries[this.entries.length - 1] : null;
  }

  rowClass(i: number, entry: LeaderboardEntry): string {
    if (entry === this.loser) return 'loser-row';
    if (i === 0) return 'rank-1';
    if (i === 1) return 'rank-2';
    if (i === 2) return 'rank-3';
    return '';
  }
}
