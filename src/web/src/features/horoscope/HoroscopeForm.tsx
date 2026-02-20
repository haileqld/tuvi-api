import React, { useState } from 'react';
import { 
  Box, 
  Button, 
  TextField, 
  FormControl, 
  InputLabel, 
  Select, 
  MenuItem, 
  FormControlLabel, 
  Checkbox, 
  Grid, 
  FormHelperText,
  SelectChangeEvent
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import type { HoroscopeGenerateRequest } from '@tuvi/shared';

interface HoroscopeFormProps {
  onSubmit: (data: HoroscopeGenerateRequest) => void;
  isLoading: boolean;
}

export const HoroscopeForm: React.FC<HoroscopeFormProps> = ({ onSubmit, isLoading }) => {
  const { t } = useTranslation();
  const [formData, setFormData] = useState<Partial<HoroscopeGenerateRequest>>({
    gender: 'male',
    language: 'en',
    includeTechnicalDetails: false,
    timezoneOffset: 7, // Default for Vietnam
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement> | SelectChangeEvent<string | number>) => {
    const { name, value } = e.target;
    
    // Type guard or explicit check if needed, but for this simple form, value is fine
    // MUI Select value can be string or number, TextField value is string
    
    setFormData((prev) => ({ ...prev, [name]: value }));
    
    // Clear error when user types
    if (errors[name]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFormData((prev) => ({ ...prev, [name]: checked }));
  };

  const validate = (): boolean => {
    const newErrors: Record<string, string> = {};
    if (!formData.gender) newErrors.gender = t('form.gender') + ' is required';
    if (!formData.gregorianBirthDate) newErrors.gregorianBirthDate = t('form.birthDate') + ' is required';
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      onSubmit(formData as HoroscopeGenerateRequest);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <TextField
            name="name"
            fullWidth
            label={t('form.name')}
            value={formData.name || ''}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <FormControl fullWidth error={!!errors.gender}>
            <InputLabel id="gender-label">{t('form.gender')}</InputLabel>
            <Select
              labelId="gender-label"
              id="gender-select"
              name="gender"
              value={formData.gender}
              label={t('form.gender')}
              onChange={handleChange}
            >
              <MenuItem value="male">{t('form.male')}</MenuItem>
              <MenuItem value="female">{t('form.female')}</MenuItem>
            </Select>
            {errors.gender && <FormHelperText>{errors.gender}</FormHelperText>}
          </FormControl>
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            name="gregorianBirthDate"
            fullWidth
            label={t('form.birthDate')}
            type="datetime-local"
            value={formData.gregorianBirthDate || ''}
            onChange={handleChange}
            InputLabelProps={{ shrink: true }}
            error={!!errors.gregorianBirthDate}
            helperText={errors.gregorianBirthDate}
            required
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <FormControl fullWidth>
            <InputLabel id="timezone-label">{t('form.timezone')}</InputLabel>
            <Select
              labelId="timezone-label"
              id="timezone-select"
              name="timezoneOffset"
              value={formData.timezoneOffset}
              label={t('form.timezone')}
              onChange={handleChange}
            >
              {Array.from({ length: 27 }, (_, i) => i - 12).map((offset) => (
                <MenuItem key={offset} value={offset}>
                  UTC {offset >= 0 ? `+${offset}` : offset}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12} sm={6}>
          <FormControlLabel
            control={
              <Checkbox
                name="includeTechnicalDetails"
                checked={formData.includeTechnicalDetails}
                onChange={handleCheckboxChange}
                color="primary"
              />
            }
            label={t('form.includeTechnical')}
          />
        </Grid>
        <Grid item xs={12}>
          <Button
            type="submit"
            fullWidth
            variant="contained"
            disabled={isLoading}
            sx={{ mt: 3, mb: 2 }}
          >
            {isLoading ? t('form.generating') : t('form.submit')}
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};
