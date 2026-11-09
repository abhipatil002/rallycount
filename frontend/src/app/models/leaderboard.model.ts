export interface LeaderboardEntry {
  id: string;
  name: string;
  avatarUrl?: string;
  score: number;
  nrr: number;
  wins: number;
  losses: number;
  matchesPlayed: number;
}
