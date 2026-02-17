# Implementation Plan: Tuvi Horoscope SPA Frontend

**Branch**: `002-horoscope-spa-frontend` | **Date**: 2026-02-17 | **Spec**: [spec.md](./spec.md)

## Summary

This plan outlines the implementation of a React SPA frontend for the Tuvi Horoscope API. The application will be built using a modern TypeScript and React stack, featuring a shareable core logic layer designed for future reuse in a React Native application, as mandated by the specification (AR-001). The project will be set up as a monorepo to facilitate this code-sharing strategy.

## Technical Context

**Language/Version**: TypeScript, React 18+
**Primary Dependencies**: React, Redux Toolkit, Material-UI (MUI), Vite
**Storage**: N/A (Client-side, uses browser local storage for settings if needed)
**Testing**: Jest, React Testing Library
**Target Platform**: Modern Browsers (last 2 versions)
**Project Type**: Web Application (Monorepo with shareable core)
**Performance Goals**: Lighthouse score >= 90; Initial interpretation in < 60s.
**Constraints**: MUST be structured for code sharing with React Native.
**Scale/Scope**: A single-page application with one primary user flow (horoscope generation).
**Build Tooling**: Vite (NEEDS CLARIFICATION)
**Monorepo Tooling**: pnpm workspaces (NEEDS CLARIFICATION)
**API Key Retrieval**: Dedicated authenticated Azure Function (NEEDS CLARIFICATION)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Development Environment Principle**: PASS. The monorepo structure with designated ports for API (7071) and Web (5173) aligns with the Dev Container principle. The plan will include creating the `.devcontainer` configuration.

**Result**: All gates pass.

## Project Structure

### Documentation (this feature)

```text
specs/002-horoscope-spa-frontend/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
│   └── api-types.ts
└── tasks.md             # Phase 2 output (created by /speckit.tasks)
```

### Source Code (repository root)

```text
# Monorepo Structure
packages/
├── core/                  # Platform-agnostic shared logic
│   ├── src/
│   │   ├── state/         # Redux Toolkit store, slices
│   │   └── services/      # API client
│   └── package.json
└── web/                   # React SPA
    ├── src/
    │   ├── components/    # Presentational React components
    │   ├── containers/    # Container components (logic)
    │   ├── pages/         # App pages
    │   └── main.tsx
    └── package.json

pnpm-workspace.yaml        # pnpm workspaces definition
package.json               # Root package.json
```

**Structure Decision**: A monorepo managed by pnpm workspaces is chosen to align with the primary architectural requirement (AR-001) of separating a `core` logic package from the `web` presentation package. This structure provides the best foundation for future code sharing with a React Native application.

## Complexity Tracking

> No violations of the constitution have been identified or justified.
