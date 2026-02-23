import '@testing-library/jest-dom';
import { vi } from 'vitest';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      if (key === 'form.name') return 'Name (Optional)';
      if (key === 'form.gender') return 'Gender';
      if (key === 'form.birthDate') return 'Birth Date and Time';
      if (key === 'form.timezone') return 'Timezone Offset';
      if (key === 'form.includeTechnical') return 'Include Technical Details';
      if (key === 'form.submit') return 'Generate Horoscope';
      if (key === 'form.generating') return 'Generating...';
      if (key === 'form.male') return 'Male';
      if (key === 'form.female') return 'Female';
      if (key === 'result.interpretation') return 'Your Horoscope Interpretation';
      if (key === 'result.technicalChart') return 'Technical Chart Details';
      if (key === 'result.location') return 'Location';
      return key;
    },
    i18n: {
      changeLanguage: () => Promise.resolve(),
      language: 'en',
    },
  }),
  initReactI18next: {
    type: '3rdParty',
    init: () => {},
  },
}));
