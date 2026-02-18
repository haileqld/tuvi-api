# Implementation Tasks: Vietnamese Horoscope (Tử Vi) Professional API

This task list is generated from the design artifacts and organized for incremental, independent implementation of each user story.

## Phase 1: Project Setup

- [x] T001 Initialize a new .NET 8 Azure Functions project in `src/`
- [x] T002 Create the directory structure (`Functions/`, `Services/`, `Models/`, `Lib/`) inside `src/`
- [x] T003 Add required NuGet packages: `Microsoft.Azure.Functions.Worker.Sdk`, `Microsoft.Azure.Functions.Worker.Extensions.Http`, `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`, `Azure.AI.OpenAI`, `Azure.Data.Tables`

## Phase 2: Foundational Components

- [x] T004 [P] Implement base API response models (`SuccessEnvelope`, `ProblemDetails`) in `src/Models/ApiContract.cs`
- [x] T005 [P] Implement the core, reusable horoscope charting engine based on Nam Phái rules in `src/Lib/Charting/`
- [x] T006 Configure dependency injection in `src/Program.cs` for services and clients
- [x] T007 Implement the `CacheService` to connect to Azure Table Storage in `src/Services/CacheService.cs`

## Phase 3: User Story 1 (Generate Interpretation)

- **Goal**: Submit birth details and receive a full AI-generated interpretation.
- **Independent Test**: Call `POST /api/v1/horoscope/generate` with valid birth data and verify a 200 response with a non-empty `interpretation` array.

- [x] T008 [P] [US1] Create request and response DTOs (`HoroscopeGenerateRequest.cs`, `HoroscopeGenerateResponse.cs`, `InterpretationItem.cs`) in `src/Models/`
- [x] T009 [P] [US1] Implement the `AiInterpretationService` to generate interpretations using `Azure.AI.OpenAI` in `src/Services/AiInterpretationService.cs`
- [x] T010 [US1] Implement the `HoroscopeService` to orchestrate the workflow (charting -> caching -> AI) in `src/Services/HoroscopeService.cs`
- [x] T011 [US1] Implement the `GenerateHoroscope.cs` HTTP trigger in `src/Functions/`, including request validation and calling `HoroscopeService`

## Phase 4: User Story 2 (Provide Technical Details)

- **Goal**: Optionally include the detailed technical chart in the response.
- **Independent Test**: Call the endpoint with `includeTechnicalDetails: true` and verify the `technicalChart` object is present and complete in the response.

- [x] T012 [P] [US2] Create DTOs for the technical chart (`TechnicalChart.cs`, `Palace.cs`, `StarPlacement.cs`) in `src/Models/`
- [x] T013 [US2] Modify `HoroscopeService` in `src/Services/HoroscopeService.cs` to conditionally attach the technical chart to the response object

## Phase 5: User Story 3 (Choose Language)

- **Goal**: Allow users to select either Vietnamese or English for the interpretation.
- **Independent Test**: Call the endpoint with `language: "vi"` and verify the narrative fields in the response are in Vietnamese.

- [x] T014 [P] [US3] Create resource files (`.resx`) for English and Vietnamese prompts and strings in `src/`
- [x] T015 [US3] Update `AiInterpretationService` in `src/Services/AiInterpretationService.cs` to use the appropriate language-specific prompt when calling the AI model

## Phase 6: Polish & Cross-Cutting Concerns

- [x] T016 [P] Implement a basic rate-limiting mechanism (FR-019) as a function filter or middleware
- [x] T017 [P] Configure CORS policy for the SPA client in `src/Program.cs` and `local.settings.json`
- [x] T018 Finalize OpenAPI documentation annotations in `src/Functions/GenerateHoroscope.cs`
- [x] T019 Configure `DefaultAzureCredential` for Managed Identity in `src/Program.cs` and verify secretless connection to Azure OpenAI and Table Storage

---

## Dependencies

- **US1** is the foundational story.
- **US2** depends on **US1** (modifies the response from US1).
- **US3** depends on **US1** (modifies the AI service from US1).

**Completion Order**: US1 → US2 → US3

## Parallel Execution Examples

- Within **Phase 3 (US1)**, `T008` (Models) and `T009` (AI Service) can be developed in parallel as they have no direct dependency on each other.
- All tasks marked with `[P]` within a phase can typically be worked on simultaneously.

## Implementation Strategy

The implementation will follow an MVP-first approach. The primary goal is to complete all tasks for **User Story 1** to deliver the core, end-to-end functionality. Subsequent user stories (US2, US3) and the Polish phase add features and robustness to this stable core.
