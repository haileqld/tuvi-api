# Frontend Data Model

This document outlines the key data structures and state models for the Tuvi Horoscope SPA frontend. These models will be managed by Redux Toolkit.

## State Slices

The application state is divided into logical slices, each managed by its own reducer.

### 1. `settings`

Represents user-configurable settings that persist across sessions (e.g., in `localStorage`).

- **Type**: `SettingsState`
- **Fields**:
    - `language: 'en' | 'vi'`: The selected display language for the UI and horoscope interpretations.
- **Initial State**: `{ language: 'en' }`

### 2. `horoscope`

Represents the complete lifecycle of a horoscope generation request.

- **Type**: `HoroscopeState`
- **Fields**:
    - `isLoading: boolean`: `true` when a request is in flight. Used to show loading indicators.
    - `error: string | null`: Stores the error message if the API request fails.
    - `result: HoroscopeGenerateResponse | null`: Stores the successful response from the API.
- **Initial State**: `{ isLoading: false, error: null, result: null }`

### 3. `form`

Represents the current state of the user input form for generating a horoscope. This state is typically local to the form component but can be lifted to Redux if needed for complex multi-step forms in the future.

- **Type**: `BirthDetailsFormState`
- **Fields**:
    - `name?: string`: The user's name (optional).
    - `gender: 'male' | 'female' | null`: The user's gender.
    - `gregorianBirthDate: string | null`: The user's birth date and time in ISO format (e.g., from an `<input type="datetime-local">`).
    - `timezoneOffset: number | null`: The user's timezone offset in minutes from UTC.
    - `includeTechnicalDetails: boolean`: Flag to request the detailed technical chart.
- **Initial State**: `{ name: '', gender: null, gregorianBirthDate: null, timezoneOffset: null, includeTechnicalDetails: false }`

## Core API Types

These types define the contract with the backend API. They will be located in the shared `packages/core` directory to be used by both the API service layer and the Redux state.

```typescript
// Located in: packages/core/src/types/api.ts

export interface HoroscopeGenerateRequest {
  name?: string;
  gender: 'male' | 'female';
  gregorianBirthDate: string; // ISO 8601 format
  timezoneOffset: number;     // In minutes from UTC
  language: 'en' | 'vi';
  includeTechnicalDetails: boolean;
}

export interface InterpretationItem {
  headline: string;
  powerScore: number;
  detail: string;
  advice: string;
}

export interface TechnicalChart {
  palaces: any[]; // Define more specifically if possible
  stars: any[];   // Define more specifically if possible
}

export interface HoroscopeGenerateResponse {
  interpretation: {
    [lifeArea: string]: InterpretationItem;
  };
  technicalChart?: TechnicalChart;
}
```
