// Thin wrapper over localStorage for the auth token.
// Kept separate so the rest of the app never touches localStorage directly,
// and so the real login flow (added with API integration) has one place to set it.

import { STORAGE_KEYS } from '../utils/constants';

export function getToken() {
  return localStorage.getItem(STORAGE_KEYS.TOKEN);
}

export function setToken(token) {
  localStorage.setItem(STORAGE_KEYS.TOKEN, token);
}

export function clearToken() {
  localStorage.removeItem(STORAGE_KEYS.TOKEN);
}

export function getUsername() {
  return localStorage.getItem(STORAGE_KEYS.USERNAME);
}

export function setUsername(username) {
  localStorage.setItem(STORAGE_KEYS.USERNAME, username);
}

export function clearUsername() {
  localStorage.removeItem(STORAGE_KEYS.USERNAME);
}

export function isAuthenticated() {
  return Boolean(getToken());
}
