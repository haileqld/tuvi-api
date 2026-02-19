import { HoroscopeGenerateRequest, HoroscopeGenerateResponse } from '../types/api';

const BASE_URL = import.meta.env.VITE_API_BASE_URL || '';

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: ApiError;
}

export interface ApiError {
  type?: string;
  title: string;
  status: number;
  detail?: string;
  instance?: string;
  extensions?: Record<string, unknown>;
}

class ApiService {
  private apiKey: string | null = null;

  private async getHeaders(): Promise<HeadersInit> {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    if (!this.apiKey) {
      await this.refreshApiKey();
    }

    if (this.apiKey) {
      headers['X-API-KEY'] = this.apiKey;
    }

    return headers;
  }

  /**
   * Retrieves the API key from a secure endpoint.
   * Implementation for TR-002.
   */
  async refreshApiKey(): Promise<void> {
    try {
      // In a real scenario, this endpoint might be protected by a session cookie or other means
      const response = await fetch(`${BASE_URL}/api/v1/auth/key`);
      if (response.ok) {
        const result = await response.json();
        this.apiKey = result.data.apiKey;
      }
    } catch (error) {
      console.error('Failed to refresh API key:', error);
    }
  }

  async generateHoroscope(request: HoroscopeGenerateRequest): Promise<ApiResponse<HoroscopeGenerateResponse>> {
    try {
      const headers = await this.getHeaders();
      const response = await fetch(`${BASE_URL}/api/v1/horoscope/generate`, {
        method: 'POST',
        headers,
        body: JSON.stringify(request),
      });

      const result = await response.json();

      if (response.ok) {
        return {
          success: true,
          data: result.data,
        };
      } else {
        return {
          success: false,
          error: result as ApiError,
        };
      }
    } catch (error) {
      return {
        success: false,
        error: {
          title: 'Network Error',
          status: 0,
          detail: error instanceof Error ? error.message : String(error),
        },
      };
    }
  }
}

export const apiService = new ApiService();
