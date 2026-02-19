import { render, screen, within } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { HoroscopeResult } from './HoroscopeResult';

describe('HoroscopeResult', () => {
  const mockResult = {
    interpretation: [
      {
        areaName: 'Career',
        headline: 'Bright future ahead',
        powerScore: 85,
        detail: 'Detailed career interpretation',
        advice: 'Keep working hard'
      },
      {
        areaName: 'Love',
        headline: 'New relationship coming',
        powerScore: 60,
        detail: 'Detailed love interpretation',
        advice: 'Be patient'
      }
    ]
  };

  it('renders nothing when result is null', () => {
    const { container } = render(<HoroscopeResult result={null} error={null} />);
    expect(container).toBeEmptyDOMElement();
  });

  it('renders error message when error is provided', () => {
    const errorMessage = 'API Error';
    render(<HoroscopeResult result={null} error={errorMessage} />);
    expect(screen.getByText(errorMessage)).toBeInTheDocument();
  });

  it('renders interpretation items when result is provided', () => {
    render(<HoroscopeResult result={mockResult} error={null} />);
    
    expect(screen.getByText(/Your Horoscope Interpretation/i)).toBeInTheDocument();
    
    // Check Career Card
    const careerCard = screen.getByText('Career').closest('.MuiCard-root') as HTMLElement;
    expect(careerCard).toBeInTheDocument();
    if (careerCard) {
      expect(within(careerCard).getByText('Bright future ahead')).toBeInTheDocument();
      expect(within(careerCard).getByText('Detailed career interpretation')).toBeInTheDocument();
      expect(within(careerCard).getByText((_content, element) => {
        return element?.textContent === 'Advice: Keep working hard';
      })).toBeInTheDocument();
      expect(within(careerCard).getByText('Power: 85')).toBeInTheDocument();
    }
    
    // Check Love Card
    const loveCard = screen.getByText('Love').closest('.MuiCard-root') as HTMLElement;
    expect(loveCard).toBeInTheDocument();
    if (loveCard) {
      expect(within(loveCard).getByText('New relationship coming')).toBeInTheDocument();
      expect(within(loveCard).getByText('Detailed love interpretation')).toBeInTheDocument();
      expect(within(loveCard).getByText((_content, element) => {
        return element?.textContent === 'Advice: Be patient';
      })).toBeInTheDocument();
      expect(within(loveCard).getByText('Power: 60')).toBeInTheDocument();
    }
  });
});
