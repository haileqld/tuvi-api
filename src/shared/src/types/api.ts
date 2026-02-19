// This file defines the TypeScript types for the Tuvi Horoscope API contract.
// It is intended to be used in the shared 'core' package.

/**
 * The request body sent to the /api/v1/horoscope/generate endpoint.
 */
export interface HoroscopeGenerateRequest {
  name?: string;
  gender: string;
  gregorianBirthDate: string; // ISO 8601 date-time string
  timezoneOffset: number;
  language?: string;
  includeTechnicalDetails?: boolean;
}

/**
 * The 'data' object within a successful API response.
 */
export interface HoroscopeGenerateResponse {
  interpretation: InterpretationItem[];
  technicalChart?: TechnicalChart | null;
}

export interface InterpretationItem {
  areaName: string;
  headline: string;
  powerScore: number;
  detail: string;
  advice: string;
}

export interface TechnicalChart {
  palaces: Palace[];
}

export interface Palace {
  name: string;
  location: string;
  stars: StarPlacement[];
}

export interface StarPlacement {
  name: string;
  category: string;
  brightness: string;
}
