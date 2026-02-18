# Quickstart Guide: Horoscope SPA Frontend

This guide explains how to set up and run the frontend development environment using the mandatory Dev Container.

## Prerequisites

1.  **Git**: To clone the repository.
2.  **VS Code**: The recommended editor.
3.  **Docker Desktop**: The Dev Container feature relies on Docker to build and run the containerized environment.
4.  **VS Code Dev Containers Extension**: Ensure the `ms-vscode-remote.remote-containers` extension is installed in VS Code.

## Running the Development Environment

### 1. Open the Project in the Dev Container

1.  Clone the repository to your local machine.
2.  Open the repository's root folder in VS Code.
3.  VS Code will detect the `.devcontainer/devcontainer.json` file and show a notification toast in the bottom-right corner: *"Folder contains a Dev Container configuration file. Reopen in Container."*
4.  Click the **"Reopen in Container"** button.
5.  VS Code will now build the Docker image and start the container. This may take several minutes on the first run. A terminal will open showing the progress.

### 2. Install Dependencies

Once the container is running and you have a terminal prompt inside VS Code (it should look something like `vscode ➜ /workspaces/tuvi-api`), install all monorepo dependencies using `pnpm`.

```bash
pnpm install
```

### 3. Run the Frontend Dev Server

After the installation is complete, you can start the React application's Vite dev server.

```bash
# This command targets the 'web' package defined in the root pnpm-workspace.yaml
pnpm --filter web dev
```

The server will start, and you should see output similar to this:

```
  VITE v5.x.x  ready in XXXms

  ➜  Local:   http://localhost:5173/
  ➜  Network: use --host to expose
  ➜  press h + enter to show help
```

VS Code will automatically forward port 5173 from the container to your local machine. You can now open a browser and navigate to **http://localhost:5173** to see the application running.

### 4. Running the Backend API

To have a fully functional application, you also need to run the backend Azure Function API.

1.  Open a **new terminal** in VS Code (while still inside the Dev Container).
2.  Navigate to the API project directory and run it:

```bash
# Ensure you are in the correct directory for the C# project
cd src/

# Run the Azure Function host
func start
```

The API will start on port 7071, which is also forwarded automatically. The frontend is configured to send requests to this port.
