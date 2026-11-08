import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login/login').then(m => m.LoginComponent) },
  { path: 'leaderboard', loadComponent: () => import('./components/leaderboard/leaderboard').then(m => m.LeaderboardComponent), canActivate: [authGuard] },
  { path: 'match-form', loadComponent: () => import('./components/match-form/match-form').then(m => m.MatchFormComponent), canActivate: [authGuard] },
  { path: 'history', loadComponent: () => import('./components/match-history/match-history').then(m => m.MatchHistoryComponent), canActivate: [authGuard] },
  { path: 'admin', loadComponent: () => import('./components/admin/admin').then(m => m.AdminComponent), canActivate: [authGuard, adminGuard] },
  { path: '', redirectTo: 'leaderboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'leaderboard' }
];
