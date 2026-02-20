import React from 'react';
import { 
  Typography, 
  Paper, 
  Box, 
  CircularProgress, 
  Alert 
} from '@mui/material';
import { useAppDispatch, useAppSelector, generateHoroscope } from '@tuvi/shared';
import type { HoroscopeGenerateRequest } from '@tuvi/shared';
import { HoroscopeForm } from '../features/horoscope/HoroscopeForm';
import { HoroscopeResult } from '../features/horoscope/HoroscopeResult';

import { useTranslation } from 'react-i18next';

const HomePage: React.FC = () => {
  const dispatch = useAppDispatch();
  const { t } = useTranslation();
  const { isLoading, error, result } = useAppSelector((state) => state.horoscope);
  const { language } = useAppSelector((state) => state.settings);

  const handleSubmit = (data: HoroscopeGenerateRequest) => {
    dispatch(generateHoroscope({ ...data, language }));
  };

  return (
    <Box>
      <Box sx={{ mb: 4, textAlign: 'center' }}>
        <Typography variant="h2" component="h1" gutterBottom>
          {t('app.title')}
        </Typography>
        <Typography variant="h5" color="text.secondary">
          {t('app.onboarding')}
        </Typography>
      </Box>

      {!result && !isLoading && (
        <Box sx={{ mb: 4 }}>
          <Alert severity="info" variant="outlined">
            {t('app.onboarding')}
          </Alert>
        </Box>
      )}

      <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
        <Typography variant="h6" gutterBottom>
          {t('form.birthDate')}
        </Typography>
        <HoroscopeForm onSubmit={handleSubmit} isLoading={isLoading} />
      </Paper>

      {isLoading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress />
          <Typography variant="h6" sx={{ ml: 2 }}>{t('form.generating')}</Typography>
        </Box>
      )}

      {error && (
        <Alert severity="error" sx={{ mt: 2 }}>
          {error}
        </Alert>
      )}

      {result && <HoroscopeResult result={result} error={null} />}
    </Box>
  );
};

export default HomePage;
