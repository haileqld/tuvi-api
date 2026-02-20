import React from 'react';
import { 
  Box, 
  Typography, 
  Card, 
  CardContent, 
  Grid, 
  Divider, 
  Chip, 
  Alert 
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import type { HoroscopeGenerateResponse } from '@tuvi/shared';
import { TechnicalChart } from './TechnicalChart';

interface HoroscopeResultProps {
  result: HoroscopeGenerateResponse | null;
  error: string | null;
}

export const HoroscopeResult: React.FC<HoroscopeResultProps> = ({ result, error }) => {
  const { t } = useTranslation();

  if (error) {
    return (
      <Box sx={{ mt: 2 }}>
        <Alert severity="error">{error}</Alert>
      </Box>
    );
  }

  if (!result) return null;

  return (
    <Box sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        {t('result.interpretation')}
      </Typography>
      <Grid container spacing={3}>
        {result.interpretation.map((item, index) => (
          <Grid item xs={12} key={index}>
            <Card variant="outlined">
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
                  <Typography variant="h6" color="primary">
                    {item.areaName}
                  </Typography>
                  <Chip 
                    label={`Power: ${item.powerScore}`} 
                    color={item.powerScore > 70 ? 'success' : item.powerScore > 40 ? 'warning' : 'error'} 
                    size="small" 
                  />
                </Box>
                <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                  {item.headline}
                </Typography>
                <Typography variant="body1" paragraph>
                  {item.detail}
                </Typography>
                <Divider sx={{ my: 1 }} />
                <Typography variant="body2" color="text.secondary">
                  <strong>Advice:</strong> {item.advice}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
      {result.technicalChart && <TechnicalChart chart={result.technicalChart} />}
    </Box>
  );
};
