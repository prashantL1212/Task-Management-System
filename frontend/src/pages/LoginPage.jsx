import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login as loginRequest } from '../api/authApi';
import { useAuth } from '../hooks/useAuth';
import { ROUTES } from '../utils/constants';

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [fieldErrors, setFieldErrors] = useState([]);

  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setErrorMessage('');
    setFieldErrors([]);

    try {
      // 2xx only reaches here; non-2xx (400/401) throws and is caught below.
      const response = await loginRequest({ username, password });
      const { token, username: returnedUsername } = response.data;

      // Persist token + username through AuthContext and go to the dashboard.
      login(token, returnedUsername);
      navigate(ROUTES.DASHBOARD, { replace: true });
    } catch (error) {
      const apiResponse = error.response?.data;

      if (apiResponse) {
        // Backend ApiResponse envelope: { success, message, errors }.
        setErrorMessage(apiResponse.message || 'Login failed.');
        if (Array.isArray(apiResponse.errors)) {
          setFieldErrors(apiResponse.errors);
        }
      } else {
        // No response = network/server unreachable.
        setErrorMessage('Unable to reach the server. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <form className="auth-card" onSubmit={handleSubmit}>
        <h1 className="auth-card__title">Sign in</h1>
        <p className="auth-card__subtitle">Access your task dashboard</p>

        {errorMessage && (
          <div className="alert alert--error" role="alert">
            <p className="alert__message">{errorMessage}</p>
            {fieldErrors.length > 0 && (
              <ul className="alert__list">
                {fieldErrors.map((message) => (
                  <li key={message}>{message}</li>
                ))}
              </ul>
            )}
          </div>
        )}

        <label className="field">
          <span className="field__label">Username</span>
          <input
            className="field__input"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Enter username"
            autoComplete="username"
            disabled={loading}
          />
        </label>

        <label className="field">
          <span className="field__label">Password</span>
          <input
            className="field__input"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter password"
            autoComplete="current-password"
            disabled={loading}
          />
        </label>

        <button type="submit" className="btn btn--primary btn--block" disabled={loading}>
          {loading ? 'Signing in…' : 'Login'}
        </button>
      </form>
    </div>
  );
}
