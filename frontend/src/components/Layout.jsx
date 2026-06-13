import { Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { ROUTES } from '../utils/constants';

// Shared chrome for authenticated pages: a header with the app title and a
// logout action, plus an <Outlet /> where the active protected page renders.
export default function Layout() {
  const navigate = useNavigate();
  const { logout, username } = useAuth();

  const handleLogout = () => {
    // Clears the token via context (which also updates state so the guard
    // re-renders). JWT is stateless server-side, so there is no logout API call.
    logout();
    navigate(ROUTES.LOGIN, { replace: true });
  };

  return (
    <div className="app-shell">
      <header className="app-header">
        <span className="app-header__brand">Task Management System</span>
        <div className="app-header__right">
          {username && <span className="app-header__user">Signed in as {username}</span>}
          <button type="button" className="btn btn--danger" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </header>

      <main className="app-content">
        <Outlet />
      </main>
    </div>
  );
}
