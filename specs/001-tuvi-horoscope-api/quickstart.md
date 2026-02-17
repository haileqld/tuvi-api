# Quickstart

This document provides quick examples of how to interact with the Tuvi Horoscope API.

## Generate a Horoscope

This is the primary endpoint for generating a horoscope interpretation.

- **Endpoint**: `POST /api/v1/horoscope/generate`
- **Content-Type**: `application/json`

### Example 1: Successful Request

This example requests a full interpretation, including the technical chart details, in Vietnamese.

```bash
curl -X POST 'http://localhost:7071/api/v1/horoscope/generate' 
-H 'Content-Type: application/json' 
-d '{
  "name": "Nguyễn Văn A",
  "gender": "Male",
  "gregorianBirthDate": "1985-02-20T23:45:00Z",
  "timezoneOffset": 7,
  "language": "vi",
  "includeTechnicalDetails": true
}'
```

A successful response (HTTP 200) will look like this:

```json
{
  "success": true,
  "data": {
    "interpretation": [
      {
        "areaName": "Mệnh (Self)",
        "headline": "Một cuộc đời lãnh đạo.",
        "powerScore": 85,
        "detail": "Lá số của bạn cho thấy sự hiện diện mạnh mẽ của sao Tử Vi...",
        "advice": "Hãy kiên nhẫn và lắng nghe người khác."
      }
    ],
    "technicalChart": {
      "palaces": [
        {
          "name": "Mệnh",
          "location": "Dần",
          "stars": [
            {
              "name": "Tử Vi",
              "category": "Major",
              "brightness": "Vượng"
            }
          ]
        }
      ]
    }
  },
  "error": null
}
```

### Example 2: Validation Error

This example shows a request with a missing `gregorianBirthDate` field.

```bash
curl -X POST 'http://localhost:7071/api/v1/horoscope/generate' 
-H 'Content-Type: application/json' 
-d '{
  "name": "Test User",
  "gender": "Female",
  "timezoneOffset": -5
}'
```

The server will return an HTTP 400 Bad Request with a Problem Details payload, as required by the constitution.

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "gregorianBirthDate": [
      "The gregorianBirthDate field is required."
    ]
  }
}
```
