import { describe, it, expect } from 'vitest';
import settingsReducer, { setLanguage } from './settingsSlice';

describe('settingsSlice', () => {
  const initialState = {
    language: 'en',
  };

  it('should return the initial state', () => {
    expect(settingsReducer(undefined, { type: 'unknown' })).toEqual(initialState);
  });

  it('should handle setLanguage', () => {
    const nextState = settingsReducer(initialState, setLanguage('vi'));
    expect(nextState.language).toBe('vi');
  });
});
