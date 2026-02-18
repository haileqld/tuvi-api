# Feature Specification: Tuvi Horoscope SPA Frontend

**Feature Branch**: `002-tuvi-horoscope-spa`
**Status**: Draft
**Input**: User requirement: "A React SPA frontend for the Tuvi Horoscope API, with a structure that allows code sharing with a future React Native app."

## Clarifications

### Session 2026-02-18
- Q: The plan specifies using `src/shared` for shareable code. Should I update architectural requirement AR-002 in the spec to make `src/shared` the required directory, replacing the previous suggestions? → A: yes
- Q: The project constitution requires client-side observability. Should I add a new non-functional requirement to the spec mandating the use of Azure Application Insights for error and performance monitoring? → A: yes
- Q: To complete the alignment, should I add a new technical requirement to the spec that mandates **Vitest** and **React Testing Library** for frontend testing? → A: yes

### Session 2026-02-17
- Q: Which UI component library should be used to build the user interface? → A: Material-UI (MUI)
- Q: Which state management library should be used for the application's core logic, as mentioned in AR-002? → A: Redux Toolkit
- Q: How should the frontend application manage and send the API key for authentication with the Azure Function backend? → A: Retrieve from secure endpoint
- Q: What is the minimum browser support required for the application? → A: Modern Browsers (last 2 versions)
- Q: What should the user see when the application first loads, before any horoscope generation requests are made? → A: Brief Onboarding

## Non-Functional Requirements

- **NFR-001 (UI Framework)**: The application MUST use the Material-UI (MUI) component library for all standard UI elements to ensure a consistent, modern, and accessible design.
- **NFR-002 (Observability)**: The application MUST be instrumented with Azure Application Insights to track client-side errors, log key user interactions, and monitor performance metrics.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Generate a Horoscope (Priority: P1)

As a user, I want to enter my birth details into a simple form and submit them to receive a complete horoscope interpretation so that I can understand my life themes.

**Why this priority**: This is the core functionality and the primary value proposition of the application.

**Independent Test**: Can be tested by loading the application, filling out the form with valid data, clicking "Generate", and verifying that an interpretation is displayed.

**Acceptance Scenarios**:

1.  **Given** I have filled out all required fields (gender, birth date/time, timezone), **When** I click the "Generate" button, **Then** I see a loading indicator, which is then replaced by the structured interpretation results.
2.  **Given** I have not filled out a required field, **When** I click the "Generate" button, **Then** I see a validation message next to the empty field, and no API call is made.

---

### User Story 2 - View Technical Chart Details (Priority: P2)

As an advanced user, I want to have the option to view the detailed technical chart data so I can understand how the interpretation was derived.

**Why this priority**: This supports power users and builds trust through transparency.

**Independent Test**: Can be tested by checking the "Include Technical Details" box before submitting the form and verifying that the technical chart is displayed alongside the interpretation.

**Acceptance Scenarios**:

1.  **Given** I have checked the "Include Technical Details" option, **When** the interpretation is successfully generated, **Then** I can see a section displaying the 12 palaces and their associated stars.
2.  **Given** the "Include Technical Details" option is unchecked, **When** the interpretation is generated, **Then** the technical chart section is not visible.

---

### User Story 3 - Choose Display Language (Priority: P3)

As a user, I want to switch the application's display language between English and Vietnamese so I can read the interpretation in the language I am most comfortable with.

**Why this priority**: This improves accessibility and broadens the potential audience.

**Independent Test**: Can be tested by selecting "Vietnamese" in a language switcher, generating a horoscope, and verifying the interpretation text is in Vietnamese.

**Acceptance Scenarios**:

1.  **Given** I select "Vietnamese" as the language, **When** I generate a horoscope, **Then** all narrative fields in the interpretation are displayed in Vietnamese.
2.  **Given** the language is set to English, **When** I generate a horoscope, **Then** the interpretation is in English.

---

### Edge Cases

-   API call times out or returns an error (e.g., 500 Internal Server Error). The UI should display a user-friendly error message.
-   The user has a slow internet connection. The loading state should be clear and persistent.
-   The API returns a successful response but with an empty or malformed `interpretation` array. The UI should handle this gracefully, perhaps showing a "Could not generate interpretation" message.

## Requirements *(mandatory)*

### Architectural & Technical Requirements (Code Sharing)

-   **AR-001**: The application MUST be structured to separate business logic, state management, and API services from the web-specific UI components. This is the highest priority architectural constraint to ensure future code sharing with React Native.
-   **AR-002**: The `src/shared` directory MUST contain all platform-agnostic code, including:
    -   State management logic (using **Redux Toolkit**).
    -   API service layer for making calls to the backend.
    -   Type definitions for API request/response objects.
-   **AR-003**: React components in the web application (`src/components`) MUST be primarily "presentational" (dumb), receiving all data and callback functions as props from higher-level "container" components that interact with the state management layer.
-   **TR-002 (API Key Management)**: The frontend application MUST retrieve the API key from a dedicated, authenticated endpoint at runtime to securely authenticate with the Azure Function backend.
-   **TR-003 (Browser Support)**: The application MUST support the latest two major versions of evergreen browsers (Chrome, Firefox, Edge, Safari).
-   **TR-004 (Frontend Testing)**: Frontend unit and integration tests MUST be written using **Vitest** and **React Testing Library**.

### Functional Requirements

-   **FR-001**: The application MUST present a form with input controls for: `name` (optional text), `gender` (select/radio), `gregorianBirthDate` (datetime-local), `timezoneOffset` (select), `language` (select/toggle), and `includeTechnicalDetails` (checkbox).
-   **FR-002**: The application MUST perform client-side validation on the form before submission to ensure all required fields are present and correctly formatted.
-   **FR-003**: On form submission, the application MUST make a `POST` request to the `/api/v1/horoscope/generate` endpoint with the form data.
-   **FR-004**: While waiting for the API response, the application MUST display a loading state (e.g., a spinner or skeleton loader) to the user.
-   **FR-005**: On a successful API response, the application MUST render the interpretation, displaying the `headline`, `powerScore`, `detail`, and `advice` for each life area.
-   **FR-006**: If the API response contains a `technicalChart`, the application MUST render it in a structured, readable format.
-   **FR-007**: If the API returns an error, the application MUST display a clear, user-friendly error message.
-   **FR-008**: On initial load, the application MUST display a brief onboarding message or instructions to guide the user on how to use the app.

### Key Entities (Frontend State)

-   **`BirthDetailsFormState`**: Represents the current values of the user input form.
-   **`HoroscopeState`**: Represents the state of a generation request, including `isLoading` (boolean), `error` (string | null), and `result` (`HoroscopeGenerateResponse` | null).
-   **`SettingsState`**: Represents user-selected settings like `language`.

## Success Criteria *(mandatory)*

### Measurable Outcomes

-   **SC-001**: First-time users can successfully generate and view a horoscope interpretation in under 60 seconds from page load.
-   **SC-002**: The core business logic and state management code coverage (from unit tests) is ≥ 80%.
-   **SC-003**: The separation of concerns is validated by ensuring no direct API calls or complex state manipulation occurs within presentational UI components.
-   **SC-004**: Lighthouse performance score for the initial page load is ≥ 90.
