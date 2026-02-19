import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { HoroscopeGenerateRequest, HoroscopeGenerateResponse } from '../types/api';
import { apiService } from '../services/apiService';

export interface HoroscopeState {
  isLoading: boolean;
  error: string | null;
  result: HoroscopeGenerateResponse | null;
}

const initialState: HoroscopeState = {
  isLoading: false,
  error: null,
  result: null,
};

export const generateHoroscope = createAsyncThunk(
  'horoscope/generate',
  async (request: HoroscopeGenerateRequest, { rejectWithValue }) => {
    const response = await apiService.generateHoroscope(request);
    if (response.success) {
      return response.data!;
    } else {
      return rejectWithValue(response.error?.title || 'Failed to generate horoscope');
    }
  }
);

const horoscopeSlice = createSlice({
  name: 'horoscope',
  initialState,
  reducers: {
    resetHoroscope: (state) => {
      state.isLoading = false;
      state.error = null;
      state.result = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(generateHoroscope.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(generateHoroscope.fulfilled, (state, action: PayloadAction<HoroscopeGenerateResponse>) => {
        state.isLoading = false;
        state.result = action.payload;
      })
      .addCase(generateHoroscope.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload as string;
      });
  },
});

export const { resetHoroscope } = horoscopeSlice.actions;
export default horoscopeSlice.reducer;
