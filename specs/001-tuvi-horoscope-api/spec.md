# Feature Specification: Vietnamese Horoscope (Tử Vi) Professional API

**Feature Branch**: `001-tuvi-horoscope-api`  
**Created**: 2026-02-16  
**Status**: Draft  
**Input**: User description: "Vietnamese Horoscope (Tử Vi) API: birth data → lunar/can-chi → Nam Phái chart → AI interpretation"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Generate an interpretation from birth details (Priority: P1)

As a user of the mobile app or SPA, I want to submit my birth details and receive an accurate Vietnamese Horoscope (Tử Vi) interpretation in modern, easy-to-understand language so I can quickly understand key life themes across major life areas.

**Why this priority**: This is the core product value for all user levels and is the smallest usable end-to-end experience.

**Independent Test**: Can be fully tested by calling the generate endpoint with a valid request and verifying a complete response envelope containing interpretation results.

**Acceptance Scenarios**:

1. **Given** a valid birth request (name, gender, birth date/time, timezone offset), **When** I call the generate endpoint, **Then** I receive `success=true` with a structured interpretation covering the standard life areas and a 0–100 score per area.
2. **Given** an invalid request (missing required fields or invalid timezone offset), **When** I call the generate endpoint, **Then** I receive a Problem Details response describing the validation issues.

---

### User Story 2 - Provide technical chart details for advanced users (Priority: P2)

As an advanced user (or a professional reader), I want to optionally receive the technical chart data (12 palaces and star placements) so I can audit, visualize, or explain how the interpretation was derived.

**Why this priority**: Transparency builds trust and supports professional/advanced usage, but the product is still useful without it.

**Independent Test**: Can be fully tested by submitting the same input twice with `includeTechnicalDetails=true` and verifying that the returned technical chart is deterministic and structurally complete (12 palaces).

**Acceptance Scenarios**:

1. **Given** `includeTechnicalDetails=true`, **When** I call the generate endpoint, **Then** the response includes a complete technical chart object containing exactly 12 palaces with star lists and locations.
2. **Given** identical birth inputs, **When** I call the endpoint multiple times, **Then** the technical chart portion is identical across responses.

---

### User Story 3 - Choose output language (Vietnamese or English) (Priority: P3)

As a user, I want to choose Vietnamese or English output so I can read the interpretation comfortably.

**Why this priority**: Improves accessibility and broadens audience, but does not change the core chart generation.

**Independent Test**: Can be fully tested by calling the same endpoint with `language=vi` and `language=en` and verifying that the response text is in the requested language while the deterministic chart data remains consistent.

**Acceptance Scenarios**:

1. **Given** a supported language selection, **When** I generate a horoscope, **Then** all narrative fields (headline/detail/advice) are returned in the selected language.

---

### Edge Cases

- Birth time is missing, invalid, or out of range (e.g., `25:61`).
- Timezone offset is outside a reasonable range (e.g., less than -12 or greater than +14).
- Conflicting input where `gregorianBirthDate` includes an offset but `timezoneOffset` disagrees.
- Requests that differ only by `name` (same birth details) and how that affects caching and personalization.
- Very high traffic spikes causing repeated identical requests (cache effectiveness).
- AI provider is temporarily unavailable or times out.
- `includeTechnicalDetails=false` and the client expects `technicalChart` to be absent (or null) consistently.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST expose a single generate endpoint at `POST /api/v1/horoscope/generate` for mobile and SPA clients.
- **FR-002**: System MUST accept the following input fields at minimum: `name`, `gender`, `gregorianBirthDate` (ISO 8601), `timezoneOffset`, `includeTechnicalDetails`.
- **FR-003**: System MUST validate requests and return Problem Details for validation failures (e.g., missing fields, invalid enum values, invalid timezone offsets).
- **FR-004**: System MUST normalize the birth moment using `gregorianBirthDate` and `timezoneOffset` such that the same real-world birth moment yields consistent results.

- **FR-005**: System MUST convert Gregorian birth data into lunar date and sexagenary (Can Chi) data for year/month/day/hour.
- **FR-006**: System MUST derive additional metadata required for chart computation, including five elements (Ngũ Hành), destiny/fate descriptor (Bản Mệnh), life palace (Cung Mệnh), and “Cục”.

- **FR-007**: System MUST generate a deterministic “An Sao” technical chart using the Nam Phái (Southern School) tradition.
- **FR-008**: The technical chart MUST map stars into exactly 12 palaces (cung) and include, at minimum:
  - the 14 major stars (Chính Tinh)
  - key star clusters/cycles (e.g., Thai Tuế, Lộc Tồn, Tràng Sinh)
  - major lucky/inauspicious stars (Cát Tinh / Lục Sát Tinh)
  - star “brightness/state” where applicable
- **FR-009**: The deterministic chart output MUST be reproducible: identical birth inputs (excluding optional display-only fields) MUST produce identical technical chart results.

- **FR-010**: System MUST generate a human-friendly interpretation of the chart using an AI “translation layer” that:
  - explains jargon in plain language
  - covers major life areas aligned to the 12 palaces
  - assigns a 0–100 “Power Score” per life area
  - provides at least one practical tip per major area

- **FR-011**: System MUST return responses using a consistent JSON envelope with `success`, `data`, and `error` fields for successful outcomes.
- **FR-012**: When `includeTechnicalDetails=true`, System MUST include the technical chart in the response; when false, System MUST omit it (or return it as null) consistently.

- **FR-013**: System MUST support localization for at least Vietnamese and English via a language parameter.

- **FR-014**: System MUST cache generated interpretations for 30 days based on a stable hash of chart-relevant birth details and request options that affect narrative output (e.g., language and includeTechnicalDetails).
- **FR-015**: System MUST avoid storing secrets in code and MUST use managed identity/keyless authentication for cloud resource access in production.
- **FR-016**: System MUST track and log token usage per request for cost monitoring.
- **FR-017**: System MUST provide machine-readable API documentation for the endpoint, including request/response schemas and error shapes.

- **FR-018**: System MUST enforce a reasonable timeout for AI interpretation generation and return an appropriate error response when the AI provider is unavailable.

### Key Entities *(include if feature involves data)*

- **HoroscopeGenerateRequest**: Client-submitted birth profile and options (name, gender, birth moment, timezone, language, includeTechnicalDetails).
- **BirthNormalization**: Normalized representation of the birth moment used to drive deterministic calculations.
- **LunarAndCanChiProfile**: Derived lunar date and Can Chi for year/month/day/hour.
- **ChartMetadata**: Derived descriptors (destiny, element/cục, life palace).
- **TechnicalChart**: The 12-palace structure with star placements and states.
- **Palace**: One of the 12 life domains; includes location/branch and a collection of stars.
- **StarPlacement**: Star name, category (major/lucky/inauspicious/cycle), and any relevant state/brightness.
- **InterpretationItem**: Narrative output per life area/palace including headline, score, detail, and advice.
- **CacheEntry**: Cached interpretation payload keyed by a deterministic request hash with an expiry time.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can generate a complete interpretation successfully in ≥ 95% of valid requests.
- **SC-002**: For repeated identical requests (same chart-relevant inputs), ≥ 80% are served from cache after the first request within the 30-day window.
- **SC-003**: The deterministic technical chart matches an approved reference suite with 100% pass rate for a curated set of test cases.
- **SC-004**: The response includes a score (0–100) for each major life area and at least one actionable tip per area in 100% of successful responses.
- **SC-005**: Token usage is captured for ≥ 99% of AI-invoking requests and can be aggregated by day.

## Assumptions

- The Nam Phái (Southern School) chart rules will be treated as a fixed, versioned ruleset validated against an expert-approved reference suite.
- The client provides `timezoneOffset` accurately; when inputs are ambiguous or conflicting, the API will reject the request rather than guess.
- Name is treated as display/personalization only and does not change deterministic chart computation.
