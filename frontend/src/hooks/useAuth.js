import { useContext } from 'react';
import { AuthContext } from '../auth/AuthContext';

// Convenience hook for reading auth state. Throws a clear error if used outside
// of <AuthProvider> so misuse is caught immediately during development.
export function useAuth() {
  const context = useContext(AuthContext);

  if (context === null) {
    throw new Error('useAuth must be used within an AuthProvider');
  }

  return context;
}
