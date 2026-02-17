# Research & Decisions

This document records the technical decisions made to resolve points marked as "NEEDS CLARIFICATION" in the implementation plan.

## Decision 1: Build Tooling

- **Decision**: **Vite** will be used as the build tool and development server for the React web application.
- **Rationale**: Vite offers a significantly faster developer experience compared to older tools like Create React App, thanks to its native ES module support and extremely fast Hot Module Replacement (HMR). It provides excellent first-party support for TypeScript and React, aligns with modern web development practices, and is highly configurable for production builds.
- **Alternatives Considered**:
    - **Create React App (CRA)**: Rejected due to its slower performance and lack of configuration flexibility without "ejecting".
    - **Next.js**: Rejected because it is a full-stack framework with features like Server-Side Rendering (SSR), which are not required for this simple SPA and would add unnecessary complexity.

## Decision 2: Monorepo Tooling

- **Decision**: **pnpm workspaces** will be used to manage the monorepo structure.
- **Rationale**: To fulfill the core architectural requirement of code sharing (AR-001), a monorepo is essential. `pnpm` is a fast, disk-space-efficient package manager with robust, built-in support for workspaces. This setup will allow for a clean separation of the `packages/core` (shared logic) and `packages/web` (SPA) projects, while simplifying dependency management and cross-package linking. It provides the ideal foundation for adding a `packages/native` project in the future.
- **Alternatives Considered**:
    - **npm/yarn workspaces**: Rejected in favor of pnpm's superior performance and more efficient handling of `node_modules`.
    - **Turborepo/Nx**: Rejected as these are more complex build orchestration tools. While powerful, they are overkill for the current two-package setup and can be added on top of pnpm workspaces later if the project's complexity grows.

## Decision 3: API Key Retrieval Strategy

- **Decision**: A new, dedicated **HTTP-triggered Azure Function** will be created to act as a secure endpoint for retrieving the `TuviApi` function key. This new function will be secured using Azure AD authentication.
- **Rationale**: The specification (TR-002) requires retrieving the API key from a secure, authenticated endpoint. Creating another Azure Function keeps the technology stack consistent with the existing backend. The frontend SPA will first authenticate the user with Azure AD (using a library like MSAL.js), acquire an access token, and then use that token to call this new "key-provider" function. The key-provider function will validate the token and return the API key for the main `GenerateHoroscope` function. This securely brokers access without exposing the function key directly to the client bundle.
- **Alternatives Considered**:
    - **Build-time Environment Variable**: Rejected as this is highly insecure, exposing the key in the compiled JavaScript.
    - **Backend for Frontend (BFF) Proxy**: A valid but more complex alternative. It would involve creating a new Node.js or C# service to proxy all API requests. Creating a single function just to vend the key is a simpler, more direct implementation of the "retrieve from secure endpoint" requirement.
