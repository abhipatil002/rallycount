export interface Period {
  id: string;
  label: string;
  frequency: 'weekly' | 'biweekly' | 'monthly';
  startDate: string;
  endDate?: string;
  isActive: boolean;
}
