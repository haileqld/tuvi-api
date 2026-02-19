# Task List: Tuvi Horoscope SPA Frontend

This task list is generated from the feature specification and implementation plan. The tasks are organized into phases that should be completed in order to ensure a smooth, dependency-aware development process.

## Implementation Strategy

The implementation will follow an MVP-first approach, prioritizing the core user story (US1) of generating a horoscope. Each user story is designed to be an independently testable and deliverable increment.

**MVP Scope**: Completion of Phase 1, 2, and 3.

## Phase 1: Project Setup

*Goal: Initialize the monorepo structure and all project scaffolds.*

- [X] T001 Create the core directory structure: `src/api`, `src/web`, `src/shared`, `tests/api`
- [X] T002 Initialize the C# Azure Functions project in `src/api/TuviApi.csproj`
- [X] T003 Initialize the React + Vite frontend project in `src/web/`
- [X] T004 Initialize the shared TypeScript package in `src/shared/`
- [X] T005 [P] Configure the root `pnpm-workspace.yaml` to include `src/web` and `src/shared`
- [X] T006 [P] Create the `.devcontainer/devcontainer.json` file for a consistent development environment
- [X] T007 [P] Create the `azure.yaml` file for Azure Developer CLI orchestration

## Phase 2: Foundational & Core Services

*Goal: Establish the core services, state management, and application shell that all features will depend on.*

- [ ] T008 Copy API type definitions into `src/shared/src/types/api.ts`
- [ ] T009 [P] Set up the main Redux store configuration in `src/shared/src/state/store.ts`
- [ ] T010 Implement a core API service client in `src/shared/src/services/apiService.ts` to handle fetch requests
- [ ] T011 Implement API key retrieval from secure endpoint within the `apiService.ts` (TR-002)
- [ ] T012 Set up the main application router in `src/web/src/app/Router.tsx`
- [ ] T013 Create a main `<App />` component with layout (header, content area) in `src/web/src/app/App.tsx`
- [ ] T014 Instrument the frontend app with Azure Application Insights in `src/web/src/main.tsx` (NFR-002)

## Phase 3: User Story 1 - Generate a Horoscope

*Goal: As a user, I want to enter my birth details into a simple form and submit them to receive a complete horoscope interpretation.*  
*Independent Test: Load the app, fill the form, click "Generate", and see a result.*

- [ ] T015 [US1] Create the `horoscopeSlice` for managing horoscope data and API status in `src/shared/src/state/horoscopeSlice.ts`
- [ ] T016 [US1] Add the `horoscopeSlice` reducer to the main store in `src/shared/src/state/store.ts`
- [ ] T017 [P] [US1] Create the `HoroscopeForm` presentational component with all required input fields in `src/web/src/features/horoscope/HoroscopeForm.tsx` (FR-001)
- [ ] T018 [P] [US1] Create the `HoroscopeResult` presentational component to display interpretation data in `src/web/src/features/horoscope/HoroscopeResult.tsx` (FR-005)
- [ ] T019 [US1] Create the `HomePage` container component to manage the form and result display in `src/web/src/pages/HomePage.tsx`
- [ ] T020 [US1] Implement client-side validation logic within the `HoroscopeForm` component (FR-002)
- [ ] T021 [US1] Write unit tests for `horoscopeSlice` using Vitest in `src/shared/src/state/horoscopeSlice.test.ts` (TR-004)
- [ ] T022 [P] [US1] Write unit tests for the `HoroscopeForm` and `HoroscopeResult` components using RTL in `src/web/src/features/horoscope/` (TR-004)

## Phase 4: User Story 2 - View Technical Chart Details

*Goal: As an advanced user, I want to view the detailed technical chart data.*  
*Independent Test: Check the "Include Technical Details" box, generate a horoscope, and see the chart data.*

- [ ] T023 [US2] Update the `HoroscopeForm` component to include the `includeTechnicalDetails` checkbox in `src/web/src/features/horoscope/HoroscopeForm.tsx`
- [ ] T024 [P] [US2] Create the `TechnicalChart` presentational component to display palaces and stars in `src/web/src/features/horoscope/TechnicalChart.tsx` (FR-006)
- [ ] T025 [US2] Update the `HoroscopeResult` component to conditionally render the `TechnicalChart` component
- [ ] T026 [P] [US2] Write unit tests for the `TechnicalChart` component in `src/web/src/features/horoscope/TechnicalChart.test.tsx` (TR-004)

## Phase 5: User Story 3 - Choose Display Language

*Goal: As a user, I want to switch the application's display language between English and Vietnamese.*  
*Independent Test: Use a language switcher, generate a horoscope, and verify the result is in the selected language.*

- [ ] T027 [US3] Create the `settingsSlice` for managing language preference in `src/shared/src/state/settingsSlice.ts`
- [ ] T028 [US3] Add the `settingsSlice` reducer to the main store in `src/shared/src/state/store.ts`
- [ ] T029 [P] [US3] Create a `LanguageSwitcher` component in `src/web/src/components/LanguageSwitcher.tsx`
- [ ] T030 [US3] Add the `LanguageSwitcher` to the main application layout in `src/web/src/app/App.tsx`
- [ ] T031 [US3] Update the API call logic to include the selected language from the `settingsSlice`
- [ ] T032 [P] [US3] Write unit tests for the `settingsSlice` in `src/shared/src/state/settingsSlice.test.ts` (TR-004)

## Phase 6: Polish & Cross-Cutting Concerns

*Goal: Address final requirements for error handling, loading states, and overall UX.*

- [ ] T033 Implement a global error display mechanism for API errors in `src/web/src/app/App.tsx` (FR-007)
- [ ] T034 Ensure a loading indicator is displayed prominently during API calls (FR-004)
- [ ] T035 Implement the brief onboarding message for first-time users in `src/web/src/pages/HomePage.tsx` (FR-008)
- [ ] T036 Perform a final review of all components to ensure adherence to MUI design principles (NFR-001)
- [ ] T037 Conduct cross-browser testing on the latest two versions of Chrome, Firefox, Edge, and Safari (TR-003)

## Dependency Graph

- **Phase 1** -> **Phase 2**
- **Phase 2** -> **Phase 3 (US1)**
- **Phase 3 (US1)** -> **Phase 4 (US2)**
- **Phase 2** -> **Phase 5 (US3)**

*Note: US2 depends on US1 being complete. US3 can be developed in parallel with US1 and US2 after Phase 2 is done.*
