import React from 'react';
import ReactDOM from 'react-dom/client';
import App from "./App";
import { Auth0Provider } from "@auth0/auth0-react";
import { createTheme, ThemeProvider } from '@mui/material/styles';
import "./styles.css";

const theme = createTheme({
  palette: {
    primary: {
      main: '#000000',
      light: '#000000',
      dark: '#000000'
    },
    secondary: {
      main: '#000000',
      light: '#000000',
      dark: '#000000'
    },
}});

const redirect_uri = process.env.REACT_APP_AUTH_REDIRECT_URL;
const audience = process.env.REACT_APP_AUTH_AUDIENCE;

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <Auth0Provider
      domain="dev-ef1halkrlv63rlmh.eu.auth0.com"
      clientId="bQiaZ9P6aqKSLGdiyZJltBQLk4edr9hZ"
      authorizationParams={{
        redirect_uri: redirect_uri + 'admin',
        audience: audience
      }}
      >
        <App/>
      </Auth0Provider>
    </ThemeProvider >

  </React.StrictMode>
);
