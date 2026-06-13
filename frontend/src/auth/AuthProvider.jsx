import { useState, useCallback, useMemo } from 'react';
import { AuthContext } from './AuthContext';
import {
  getToken,
  setToken as persistToken,
  clearToken,
  getUsername,
  setUsername as persistUsername,
  clearUsername,
} from './authStorage';

// Holds the authentication state for the whole app and exposes it via context.
// The token is the single source of truth for "is the user logged in".
export default function AuthProvider({ children }) {
  // Initialize from localStorage so a page refresh keeps the user signed in.
  const [token, setTokenState] = useState(() => getToken());
  const [username, setUsernameState] = useState(() => getUsername());

  // Called after a successful login with the JWT and username returned by the API.
  const login = useCallback((newToken, newUsername) => {
    persistToken(newToken);
    setTokenState(newToken);

    if (newUsername) {
      persistUsername(newUsername);
      setUsernameState(newUsername);
    }
  }, []);

  const logout = useCallback(() => {
    clearToken();
    clearUsername();
    setTokenState(null);
    setUsernameState(null);
  }, []);

  const value = useMemo(
    () => ({
      token,
      username,
      isAuthenticated: Boolean(token),
      login,
      logout,
    }),
    [token, username, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
