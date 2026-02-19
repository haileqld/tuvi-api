import React from 'react';
import { ButtonGroup, Button } from '@mui/material';
import { useAppDispatch, useAppSelector, setLanguage } from '@tuvi/shared';

export const LanguageSwitcher: React.FC = () => {
  const dispatch = useAppDispatch();
  const { language } = useAppSelector((state) => state.settings);

  const handleLanguageChange = (lang: string) => {
    dispatch(setLanguage(lang));
  };

  return (
    <ButtonGroup variant="outlined" color="inherit" size="small">
      <Button 
        variant={language === 'en' ? 'contained' : 'outlined'} 
        onClick={() => handleLanguageChange('en')}
      >
        EN
      </Button>
      <Button 
        variant={language === 'vi' ? 'contained' : 'outlined'} 
        onClick={() => handleLanguageChange('vi')}
      >
        VI
      </Button>
    </ButtonGroup>
  );
};
