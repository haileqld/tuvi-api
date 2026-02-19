import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { HoroscopeForm } from './HoroscopeForm';

describe('HoroscopeForm', () => {
  const mockOnSubmit = vi.fn();

  it('renders all form fields', () => {
    render(<HoroscopeForm onSubmit={mockOnSubmit} isLoading={false} />);
    
    expect(screen.getByLabelText(/Name \(Optional\)/i)).toBeInTheDocument();
    expect(screen.getByRole('combobox', { name: /Gender/i })).toBeInTheDocument();
    expect(screen.getByLabelText(/Birth Date and Time/i)).toBeInTheDocument();
    expect(screen.getByRole('combobox', { name: /Timezone Offset/i })).toBeInTheDocument();
    expect(screen.getByLabelText(/Include Technical Details/i)).toBeInTheDocument();
  });

  it('shows validation errors for required fields', async () => {
    render(<HoroscopeForm onSubmit={mockOnSubmit} isLoading={false} />);
    
    const submitButton = screen.getByRole('button', { name: /Generate Horoscope/i });
    fireEvent.click(submitButton);

    expect(await screen.findByText(/Birth date and time are required/i)).toBeInTheDocument();
    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('submits form with valid data', async () => {
    render(<HoroscopeForm onSubmit={mockOnSubmit} isLoading={false} />);
    
    fireEvent.change(screen.getByLabelText(/Birth Date and Time/i), {
      target: { value: '1990-01-01T12:00' },
    });

    const submitButton = screen.getByRole('button', { name: /Generate Horoscope/i });
    fireEvent.click(submitButton);

    expect(mockOnSubmit).toHaveBeenCalledWith(expect.objectContaining({
      gregorianBirthDate: '1990-01-01T12:00',
      gender: 'male',
      timezoneOffset: 7,
    }));
  });

  it('disables submit button when loading', () => {
    render(<HoroscopeForm onSubmit={mockOnSubmit} isLoading={true} />);
    
    const submitButton = screen.getByRole('button', { name: /Generating.../i });
    expect(submitButton).toBeDisabled();
  });
});
