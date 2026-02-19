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
  FormHelperText
} from '@mui/material';
import type { HoroscopeGenerateRequest } from '@tuvi/shared';

interface HoroscopeFormProps {
  onSubmit: (data: HoroscopeGenerateRequest) => void;
  isLoading: boolean;
}

export const HoroscopeForm: React.FC<HoroscopeFormProps> = ({ onSubmit, isLoading }) => {
  const [formData, setFormData] = useState<Partial<HoroscopeGenerateRequest>>({
    gender: 'male',
    language: 'en',
    includeTechnicalDetails: false,
    timezoneOffset: 7, // Default for Vietnam
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement> | { target: { name: string; value: any } }) => {
    const { name, value } = e.target;
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
    if (!formData.gender) newErrors.gender = 'Gender is required';
    if (!formData.gregorianBirthDate) newErrors.gregorianBirthDate = 'Birth date and time are required';
    
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
            label="Name (Optional)"
            value={formData.name || ''}
            onChange={handleChange}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <FormControl fullWidth error={!!errors.gender}>
            <InputLabel id="gender-label">Gender</InputLabel>
            <Select
              labelId="gender-label"
              id="gender-select"
              name="gender"
              value={formData.gender}
              label="Gender"
              onChange={(e) => handleChange(e as any)}
            >
              <MenuItem value="male">Male</MenuItem>
              <MenuItem value="female">Female</MenuItem>
            </Select>
            {errors.gender && <FormHelperText>{errors.gender}</FormHelperText>}
          </FormControl>
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            name="gregorianBirthDate"
            fullWidth
            label="Birth Date and Time"
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
            <InputLabel id="timezone-label">Timezone Offset</InputLabel>
            <Select
              labelId="timezone-label"
              id="timezone-select"
              name="timezoneOffset"
              value={formData.timezoneOffset}
              label="Timezone Offset"
              onChange={(e) => handleChange(e as any)}
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
            label="Include Technical Details"
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
            {isLoading ? 'Generating...' : 'Generate Horoscope'}
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};
