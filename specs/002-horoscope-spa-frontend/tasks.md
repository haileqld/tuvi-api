# Implementation Tasks: Tuvi Horoscope SPA Frontend

This task list is generated from the design artifacts and organized for incremental, independent implementation of each user story.

## Phase 1: Project Setup (Monorepo & Web App)

- [x] T001 Create `pnpm-workspace.yaml` in the repository root for monorepo setup
- [x] T002 Initialize root `package.json` and configure `pnpm` workspaces
- [x] T003 Create `packages/core` directory and initialize `package.json` for shared logic
- [ ] T004 Create `packages/web` directory and initialize a Vite + React + TypeScript project inside it
- [ ] T005 Install root development dependencies (e.g., `typescript`, `vitest`, `jest`, `eslint`, `prettier`)
- [ ] T006 Configure shared ESLint and Prettier for the monorepo
- [ ] T007 Configure `tsconfig.json` for `packages/core` and `packages/web` with appropriate path aliases
- [ ] T008 Create `.devcontainer` folder and `devcontainer.json` based on the project constitution (Node.js LTS, Azure CLI, port forwarding)

## Phase 2: Foundational Components (Shared Core & UI Base)

- [ ] T009 Copy `api-types.ts` from `specs/002-horoscope-spa-frontend/contracts/` to `packages/core/src/api/api-types.ts`
- [ ] T010 Implement API client (e.g., using `axios` or `fetch`) in `packages/core/src/services/apiClient.ts`
- [ ] T011 Set up Redux Toolkit store, initial state, and root reducer in `packages/core/src/state/store.ts`
- [ ] T012 Configure `packages/web` to use the shared Redux store from `packages/core` in `packages/web/src/main.tsx`
- [ ] T013 Configure Material-UI (MUI) theme and `ThemeProvider` in `packages/web/src/App.tsx`

## Phase 3: User Story 1 (Generate Interpretation)

- **Goal**: Allow users to enter birth details and receive an AI-generated horoscope interpretation.
- **Independent Test**: Load application, fill form with valid data, click "Generate", verify interpretation is displayed.

- [ ] T014 [P] [US1] Create `BirthDetailsForm` presentational component in `packages/web/src/components/forms/BirthDetailsForm.tsx` (using MUI components)
- [ ] T015 [P] [US1] Create `InterpretationDisplay` presentational component in `packages/web/src/components/display/InterpretationDisplay.tsx` (using MUI components)
- [ ] T016 [US1] Create Redux slice (`horoscopeSlice.ts`) in `packages/core/src/state/` to manage horoscope generation state (`isLoading`, `error`, `result`)
- [ ] T017 [US1] Implement Redux async thunk in `horoscopeSlice.ts` to call the API client (`packages/core/src/services/apiClient.ts`)
- [ ] T018 [US1] Create `HoroscopePage` container component in `packages/web/src/pages/HoroscopePage.tsx`
- [ ] T019 [US1] Integrate `BirthDetailsForm` and `InterpretationDisplay` into `HoroscopePage`, connecting them to the Redux store
- [ ] T020 [US1] Implement client-side form validation using a library like `react-hook-form` and `yup` within `BirthDetailsForm.tsx`

## Phase 4: User Story 2 (Provide Technical Details)

- **Goal**: Allow users to view the detailed technical chart data if requested.
- **Independent Test**: Check "Include Technical Details" box, generate horoscope, verify technical chart display.

- [ ] T021 [P] [US2] Create `TechnicalChartDisplay` presentational component in `packages/web/src/components/display/TechnicalChartDisplay.tsx` (using MUI)
- [ ] T022 [US2] Update `HoroscopePage` to conditionally render `TechnicalChartDisplay` based on `includeTechnicalDetails` state
- [ ] T023 [US2] Ensure `horoscopeSlice.ts` correctly handles the `technicalChart` part of the API response

## Phase 5: User Story 3 (Choose Display Language)

- **Goal**: Allow users to switch between English and Vietnamese for the interpretation.
- **Independent Test**: Select "Vietnamese" in language switcher, generate horoscope, verify interpretation text is in Vietnamese.

- [ ] T024 [P] [US3] Create Redux slice (`settingsSlice.ts`) in `packages/core/src/state/` to manage user settings, including `language`
- [ ] T025 [P] [US3] Implement `LanguageSwitcher` component in `packages/web/src/components/controls/LanguageSwitcher.tsx` (using MUI)
- [ ] T026 [US3] Update `horoscopeSlice.ts` to use the `language` setting when making API calls
- [ ] T027 [US3] Integrate `LanguageSwitcher` into `HoroscopePage` or main application layout

## Phase 6: Polish & Cross-Cutting Concerns

- [ ] T028 Implement `InitialOnboarding` component in `packages/web/src/components/onboarding/InitialOnboarding.tsx` and integrate into `HoroscopePage` (FR-008)
- [ ] T029 Implement API Key retrieval from a secure endpoint (TR-002) in `packages/core/src/services/auth.ts`
- [ ] T030 Integrate API Key retrieval into `apiClient.ts` for authenticated calls
- [ ] T031 Implement global error handling and display user-friendly notifications (e.g., using MUI Snackbar)
- [ ] T032 Ensure responsiveness of the UI across common device breakpoints (e.g., using MUI Grid system)
- [ ] T033 Add basic unit tests for Redux slices in `packages/core/src/state/`
- [ ] T034 Add basic component tests for key presentational components (e.g., `BirthDetailsForm`) in `packages/web/src/components/`
- [ ] T035 Review and optimize build configuration for `packages/web` (production build, tree-shaking)
- [ ] T036 Set up `pnpm` run scripts for development, build, and test
