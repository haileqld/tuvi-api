import React from 'react';
import { 
  Box, 
  Typography, 
  Grid, 
  Card, 
  CardContent, 
  List, 
  ListItem, 
  ListItemText, 
  Chip 
} from '@mui/material';
import type { TechnicalChart as TechnicalChartType } from '@tuvi/shared';

interface TechnicalChartProps {
  chart: TechnicalChartType;
}

export const TechnicalChart: React.FC<TechnicalChartProps> = ({ chart }) => {
  return (
    <Box sx={{ mt: 4 }}>
      <Typography variant="h5" gutterBottom>
        Technical Chart Details
      </Typography>
      <Grid container spacing={2}>
        {chart.palaces.map((palace, index) => (
          <Grid item xs={12} sm={6} md={4} lg={3} key={index}>
            <Card variant="outlined" sx={{ height: '100%' }}>
              <CardContent>
                <Typography variant="h6" color="secondary" gutterBottom>
                  {palace.name}
                </Typography>
                <Typography variant="body2" color="text.secondary" gutterBottom>
                  Location: {palace.location}
                </Typography>
                <List dense>
                  {palace.stars.map((star, sIndex) => (
                    <ListItem key={sIndex} disablePadding>
                      <ListItemText 
                        primary={star.name} 
                        secondary={
                          <Typography component="span" variant="caption" sx={{ display: 'flex', gap: 0.5, mt: 0.5 }}>
                            <Chip label={star.category} size="small" variant="outlined" />
                            {star.brightness && (
                              <Chip label={star.brightness} size="small" />
                            )}
                          </Typography>
                        }
                      />
                    </ListItem>
                  ))}
                </List>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};
