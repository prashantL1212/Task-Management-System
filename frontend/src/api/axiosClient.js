import axios from 'axios';
import { getToken, clearToken } from '../auth/authStorage';
import { ROUTES } from '../utils/constants';

// Base URL: configurable per environment (Vite env), defaulting to the local API.
// .env.development -> http://localhost:5210/api ; .env.production -> /api (nginx proxy)
const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5210/api';

const axiosClient = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// ---------- Request interceptor ----------
// Attach the JWT (if present) to every outgoing request.
axiosClient.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

// ---------- Response interceptor ----------
// Pass successful responses through untouched. On a 401 (expired/invalid token),
// clear the token and send the user to /login — globally, so individual callers
// don't each have to handle auth expiry. The login request itself is exempted so
// bad-credential 401s are handled by the login page instead of triggering a redirect.
axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status;
    const requestUrl = error.config?.url ?? '';
    const isLoginRequest = requestUrl.includes('/auth/login');

    if (status === 401 && !isLoginRequest) {
      clearToken();
      if (window.location.pathname !== ROUTES.LOGIN) {
        window.location.assign(ROUTES.LOGIN);
      }
    }

    return Promise.reject(error);
  },
);

export default axiosClient;
