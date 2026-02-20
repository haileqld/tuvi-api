import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
// import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { store } from '@tuvi/shared';
import { AppRouter } from './app/Router';
import './app/i18n';
import './index.css';

/*
// Initialize Application Insights (NFR-002)
const connectionString = import.meta.env.VITE_APPINSIGHTS_CONNECTION_STRING;
if (connectionString) {
  const appInsights = new ApplicationInsights({
    config: {
      connectionString: connectionString,
      enableAutoRouteTracking: true,
    },
  });
  appInsights.loadAppInsights();
} else {
  console.warn("Application Insights connection string not found. Skipping initialization.");
}
*/

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <AppRouter />
    </Provider>
  </StrictMode>,
);
