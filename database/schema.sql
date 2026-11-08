CREATE EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE players (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    google_id VARCHAR(255) UNIQUE NOT NULL,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    avatar_url TEXT,
    is_admin BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE periods (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    label VARCHAR(100) NOT NULL,
    frequency VARCHAR(20) NOT NULL CHECK (frequency IN ('weekly', 'biweekly', 'monthly')),
    start_date DATE NOT NULL,
    end_date DATE,
    is_active BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

-- Only one active period at a time
CREATE UNIQUE INDEX one_active_period ON periods (is_active) WHERE is_active = TRUE;

CREATE TABLE matches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    period_id UUID NOT NULL REFERENCES periods(id),
    team1_p1 UUID NOT NULL REFERENCES players(id),
    team1_p2 UUID NOT NULL REFERENCES players(id),
    team2_p1 UUID NOT NULL REFERENCES players(id),
    team2_p2 UUID NOT NULL REFERENCES players(id),
    team1_score INT NOT NULL CHECK (team1_score >= 0),
    team2_score INT NOT NULL CHECK (team2_score >= 0),
    played_at TIMESTAMPTZ DEFAULT NOW(),
    submitted_by UUID NOT NULL REFERENCES players(id),
    CONSTRAINT distinct_players CHECK (
        team1_p1 <> team1_p2 AND team1_p1 <> team2_p1 AND team1_p1 <> team2_p2 AND
        team1_p2 <> team2_p1 AND team1_p2 <> team2_p2 AND
        team2_p1 <> team2_p2
    )
);
