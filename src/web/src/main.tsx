import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { store } from '@tuvi/shared';
import { AppRouter } from './app/Router';
import './index.css';

// Initialize Application Insights (NFR-002)
const appInsights = new ApplicationInsights({
  config: {
    connectionString: import.meta.env.VITE_APPINSIGHTS_CONNECTION_STRING,
    enableAutoRouteTracking: true,
  },
});
appInsights.loadAppInsights();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <AppRouter />
    </Provider>
  </StrictMode>,
);
