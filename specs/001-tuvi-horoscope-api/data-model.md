# Data Model

This document is generated from the "Key Entities" section of the feature specification. It defines the primary data structures used within the Tuvi Horoscope API.

## Entities

### HoroscopeGenerateRequest
Client-submitted birth profile and options.

| Field | Type | Description | Constraints |
| :--- | :--- | :--- | :--- |
| `name` | string | Display name for personalization. | Not used in chart calculation. |
| `gender` | string | User's gender. | Must be a valid enum value (e.g., Male, Female, Other). |
| `gregorianBirthDate` | string | ISO 8601 formatted date-time string. | Required. |
| `timezoneOffset` | number | Timezone offset from UTC in hours (e.g., -7, +5.5). | Required. Must be between -12 and +14. |
| `language` | string | Desired output language (e.g., "en", "vi"). | Defaults to a system-wide setting if omitted. |
| `includeTechnicalDetails` | boolean | Flag to include detailed chart data in the response. | Defaults to `false`. |

---

### BirthNormalization
Internal, normalized representation of the birth moment.

| Field | Type | Description |
| :--- | :--- | :--- |
| `utcBirthMoment` | DateTime | The precise birth moment converted to UTC. |
| `originalOffset` | number | The `timezoneOffset` provided by the user. |

---

### LunarAndCanChiProfile
Derived lunar and sexagenary cycle data for the birth moment.

| Field | Type | Description |
| :--- | :--- | :--- |
| `lunarYear` | integer | The lunar year of birth. |
| `lunarMonth` | integer | The lunar month of birth. |
| `lunarDay` | integer | The lunar day of birth. |
| `lunarHour` | integer | The lunar hour of birth. |
| `canChiYear` | string | The Can Chi (Stem-Branch) for the year. |
| `canChiMonth` | string | The Can Chi (Stem-Branch) for the month. |
| `canChiDay` | string | The Can Chi (Stem-Branch) for the day. |
| `canChiHour` | string | The Can Chi (Stem-Branch) for the hour. |

---

### TechnicalChart
The complete, deterministic 12-palace structure with all star placements.

| Field | Type | Description |
| :--- | :--- | :--- |
| `palaces` | Palace[] | An array containing exactly 12 palace objects. |

---

### Palace
Represents one of the 12 life domains in the horoscope chart.

| Field | Type | Description |
| :--- | :--- | :--- |
| `name` | string | The name of the palace (e.g., "Mệnh", "Phụ Mẫu"). |
| `location` | string | The earthly branch (Địa Chi) where the palace is located. |
| `stars` | StarPlacement[] | A collection of stars located within this palace. |

---

### StarPlacement
Represents a single star within a palace.

| Field | Type | Description |
| :--- | :--- | :--- |
| `name` | string | The name of the star (e.g., "Tử Vi", "Thiên Phủ"). |
| `category` | string | The star's classification (e.g., "Major", "Lucky", "Inauspicious"). |
| `brightness` | string | The state or brightness of the star where applicable (e.g., "Vượng", "Hãm"). |

---

### InterpretationItem
The final, human-friendly narrative output for a single life area.

| Field | Type | Description |
| :--- | :--- | :--- |
| `areaName` | string | The name of the life area, corresponding to a palace. |
| `headline` | string | A short, summary headline for the interpretation. |
| `powerScore` | integer | A 0-100 score representing the strength/favorability of this area. |
| `detail` | string | The detailed, multi-paragraph interpretation text. |
| `advice` | string | A short, practical tip related to the interpretation. |

---

### CacheEntry
The payload stored in the caching system.

| Field | Type | Description |
| :--- | :--- | :--- |
| `cacheKey` | string | The deterministic hash generated from the request. |
| `payload` | object | The full JSON response `data` object that was sent to the client. |
| `expiry` | DateTime | The UTC timestamp when this cache entry should expire. |
