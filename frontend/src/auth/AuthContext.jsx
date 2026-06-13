import { createContext } from 'react';

// The auth context object. Kept in its own file so the provider component and
// the consuming hook can both import it without a circular dependency.
// Default is null so useAuth can detect usage outside of an AuthProvider.
export const AuthContext = createContext(null);
