import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { LeaderboardService } from '../../services/leaderboard.service';
import { MatchService } from '../../services/match.service';
import { Period } from '../../models/period.model';
import { Player } from '../../models/player.model';

@Component({
  selector: 'app-admin',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class AdminComponent implements OnInit {
  private api = inject(ApiService);
  private lbService = inject(LeaderboardService);
  private matchService = inject(MatchService);
  private cdr = inject(ChangeDetectorRef);

  periods: Period[] = [];
  players: Player[] = [];

  newPeriod = { label: '', frequency: 'weekly', startDate: '' };
  periodError = '';

  async ngOnInit(): Promise<void> {
    await this.refresh();
  }

  private async refresh(): Promise<void> {
    [this.periods, this.players] = await Promise.all([
      this.lbService.getPeriods(),
      this.matchService.getPlayers()
    ]);
    this.cdr.detectChanges();
  }

  async createPeriod(): Promise<void> {
    this.periodError = '';
    if (!this.newPeriod.label) { this.periodError = 'Label is required.'; return; }
    if (!this.newPeriod.startDate) { this.periodError = 'Start date is required.'; return; }
    await this.api.post('/api/admin/periods', this.newPeriod);
    this.newPeriod = { label: '', frequency: 'weekly', startDate: '' };
    this.periods = await this.lbService.getPeriods();
    this.cdr.detectChanges();
  }

  async activatePeriod(id: string): Promise<void> {
    await this.api.put(`/api/admin/periods/${id}/activate`);
    this.periods = await this.lbService.getPeriods();
    this.cdr.detectChanges();
  }

  async toggleAdmin(player: Player): Promise<void> {
    await this.api.put(`/api/admin/players/${player.id}/toggle-admin`);
    player.isAdmin = !player.isAdmin;
  }
}
