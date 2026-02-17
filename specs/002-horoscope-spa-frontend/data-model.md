# Data Model (Frontend State)

This document is generated from the "Key Entities (Frontend State)" section of the feature specification. It defines the primary data structures used to manage the application's state.

## State Entities

### `BirthDetailsFormState`
Represents the current values of the user input form.

| Property | Type | Description |
| :--- | :--- | :--- |
| `name` | `string` | The optional display name. |
| `gender` | `string` | The selected gender. |
| `gregorianBirthDate` | `string` | The ISO 8601 date/time string from the input. |
| `timezoneOffset` | `number` | The selected timezone offset. |
| `language` | `string` | The selected language code (e.g., "en", "vi"). |
| `includeTechnicalDetails` | `boolean` | Whether to request the detailed chart data. |

---

### `HoroscopeState`
Represents the state of a horoscope generation request.

| Property | Type | Description |
| :--- | :--- | :--- |
| `isLoading` | `boolean` | `true` when an API request is in flight. |
| `error` | `string \| null` | Stores any error message from a failed API request. |
| `result` | `HoroscopeGenerateResponse \| null` | Stores the successful data from the API. |

---

### `SettingsState`
Represents global user-selected settings.

| Property | Type | Description |
| :--- | :--- | :--- |
| `language` | `string` | The currently active language for the UI and API requests. |
