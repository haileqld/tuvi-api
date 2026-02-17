# Quickstart: Horoscope SPA Frontend

This guide provides instructions for setting up and running the frontend development environment.

## Prerequisites

-   [Node.js](https://nodejs.org/) (LTS version)
-   [pnpm](https://pnpm.io/installation)

## Setup

The project is structured as a monorepo using pnpm workspaces.

1.  **Install dependencies**:
    From the root of the `tuvi-api` repository, run the pnpm install command. This will install dependencies for all packages (`core`, `web`, etc.).

    ```bash
    pnpm install
    ```

## Running the Development Server

1.  **Start the web application**:
    To run the React SPA in development mode, use the pnpm filter command to target the `web` package.

    ```bash
    pnpm --filter web dev
    ```

2.  **Access the application**:
    Once the command completes, you can access the application in your browser at the URL provided (typically `http://localhost:5173`).

    The development server supports Hot Module Replacement (HMR), so changes you make to the source code will be reflected in the browser instantly without a full page reload.

## Running the Backend API

For the frontend to function, the backend API must also be running. Ensure the API is running on port `7071` as specified in the project constitution.
