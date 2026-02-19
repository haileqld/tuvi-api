import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import { TechnicalChart } from './TechnicalChart';

describe('TechnicalChart', () => {
  const mockChart = {
    palaces: [
      {
        name: 'Mệnh',
        location: 'Dần',
        stars: [
          { name: 'Tử Vi', category: 'Major', brightness: 'Vượng' },
          { name: 'Hóa Lộc', category: 'Lucky', brightness: '' }
        ]
      },
      {
        name: 'Phụ Mẫu',
        location: 'Mão',
        stars: [
          { name: 'Thiên Cơ', category: 'Major', brightness: 'Đắc' }
        ]
      }
    ]
  };

  it('renders technical chart details', () => {
    render(<TechnicalChart chart={mockChart} />);
    
    expect(screen.getByText(/Technical Chart Details/i)).toBeInTheDocument();
    
    expect(screen.getByText('Mệnh')).toBeInTheDocument();
    expect(screen.getByText(/Location: Dần/i)).toBeInTheDocument();
    expect(screen.getByText('Tử Vi')).toBeInTheDocument();
    expect(screen.getByText('Vượng')).toBeInTheDocument();
    expect(screen.getByText('Hóa Lộc')).toBeInTheDocument();
    
    expect(screen.getByText('Phụ Mẫu')).toBeInTheDocument();
    expect(screen.getByText(/Location: Mão/i)).toBeInTheDocument();
    expect(screen.getByText('Thiên Cơ')).toBeInTheDocument();
    expect(screen.getByText('Đắc')).toBeInTheDocument();
  });
});
