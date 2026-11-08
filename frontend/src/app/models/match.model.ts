export interface Match {
  id: string;
  periodId: string;
  team1P1: string;
  team1P2: string;
  team2P1: string;
  team2P2: string;
  team1Score: number;
  team2Score: number;
  playedAt: string;
  submittedBy: string;
}

export interface CreateMatchDto {
  periodId: string;
  team1P1: string;
  team1P2: string;
  team2P1: string;
  team2P2: string;
  team1Score: number;
  team2Score: number;
}
