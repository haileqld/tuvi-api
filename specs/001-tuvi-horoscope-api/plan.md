# Implementation Plan: Vietnamese Horoscope (Tử Vi) Professional API

**Branch**: `001-tuvi-horoscope-api` | **Date**: 2026-02-16 | **Spec**: [spec.md](./spec.md)

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

This plan outlines the implementation of a Vietnamese Horoscope (Tử Vi) API on an Azure Functions backend. The system will accept a user's birth details, generate a deterministic technical horoscope chart, use a managed AI service to create a human-friendly interpretation, and return the structured result. The implementation will adhere strictly to the project's constitution, emphasizing clean architecture, managed identity, and a stable API contract.

## Technical Context

**Language/Version**: C# (.NET 8 Isolated Worker)
**Primary Dependencies**: `Azure.AI.OpenAI`, `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`, `System.Text.Json`
**Storage**: Azure Table Storage for caching (NEEDS CLARIFICATION)
**Testing**: xUnit + Moq
**Target Platform**: Azure Functions v4
**Project Type**: single
**Performance Goals**: p90 < 5000ms (per spec SC-006)
**Constraints**: Must use Managed Identity for Azure resource access.
**Scale/Scope**: Initial scope is a single API endpoint with caching and AI integration.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Principle I (Clean Architecture):** PASS. The proposed structure will separate HTTP triggers, services, and models.
- **Principle II (Zero-Trust Secrets):** PASS. The plan will use `DefaultAzureCredential` and Managed Identity.
- **Principle III (Stable API Contract):** PASS. The plan will implement the standard success envelope and Problem Details for errors.
- **Principle IV (Async, DI-First):** PASS. All I/O will be async, and dependencies will be injected.
- **Principle V (AI Safety & Cost):** PASS. The plan will incorporate token logging and timeouts for AI calls.

**Result**: All gates pass.

## Project Structure

### Documentation (this feature)

```text
specs/001-tuvi-horoscope-api/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
│   └── openapi.yaml
└── tasks.md             # Phase 2 output (created by /speckit.tasks)
```

### Source Code (repository root)

```text
src/
├── Functions/
│   └── GenerateHoroscope.cs
├── Services/
│   ├── HoroscopeService.cs
│   ├── AiInterpretationService.cs
│   └── CacheService.cs
├── Models/
│   ├── HoroscopeGenerateRequest.cs
│   └── HoroscopeGenerateResponse.cs
└── Lib/
    ├── Charting/
    └── Core/

tests/
├── Integration/
└── Unit/
```

**Structure Decision**: The selected structure is a single project (`src/`) that aligns with the "Clean Architecture Boundaries" principle in the constitution. It separates the Function trigger from business logic (Services) and data transfer objects (Models). A `Lib` folder is included for the core, reusable horoscope calculation logic.

## Complexity Tracking

> No violations of the constitution have been identified or justified.
