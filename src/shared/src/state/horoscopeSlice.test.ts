import { describe, it, expect, vi } from 'vitest';
import horoscopeReducer, { resetHoroscope, generateHoroscope } from './horoscopeSlice';
import { apiService } from '../services/apiService';

vi.mock('../services/apiService', () => ({
  apiService: {
    generateHoroscope: vi.fn(),
  },
}));

describe('horoscopeSlice', () => {
  const initialState = {
    isLoading: false,
    error: null,
    result: null,
  };

  it('should return the initial state', () => {
    expect(horoscopeReducer(undefined, { type: 'unknown' })).toEqual(initialState);
  });

  it('should handle resetHoroscope', () => {
    const state = {
      isLoading: false,
      error: 'some error',
      result: { interpretation: [] },
    };
    expect(horoscopeReducer(state as any, resetHoroscope())).toEqual(initialState);
  });

  describe('generateHoroscope async thunk', () => {
    it('should handle pending state', () => {
      const state = horoscopeReducer(initialState, generateHoroscope.pending('', {} as any));
      expect(state.isLoading).toBe(true);
      expect(state.error).toBeNull();
    });

    it('should handle fulfilled state', async () => {
      const mockResult = { interpretation: [{ areaName: 'test', headline: 'test', powerScore: 100, detail: 'test', advice: 'test' }] };
      vi.mocked(apiService.generateHoroscope).mockResolvedValueOnce({
        success: true,
        data: mockResult,
      });

      const state = horoscopeReducer(initialState, generateHoroscope.fulfilled(mockResult, '', {} as any));
      expect(state.isLoading).toBe(false);
      expect(state.result).toEqual(mockResult);
    });

    it('should handle rejected state', async () => {
      const errorMessage = 'Network Error';
      vi.mocked(apiService.generateHoroscope).mockResolvedValueOnce({
        success: false,
        error: { title: errorMessage, status: 500 },
      });

      const state = horoscopeReducer(initialState, generateHoroscope.rejected(null, '', {} as any, errorMessage));
      expect(state.isLoading).toBe(false);
      expect(state.error).toBe(errorMessage);
    });
  });
});
