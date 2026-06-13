import { Link } from 'react-router-dom';
import { ROUTES } from '../utils/constants';

// Catch-all "*" route.
export default function NotFoundPage() {
  return (
    <div className="auth-page">
      <div className="auth-card auth-card--center">
        <h1 className="notfound__code">404</h1>
        <p className="page__text">The page you are looking for does not exist.</p>
        <Link className="btn btn--primary" to={ROUTES.DASHBOARD}>
          Go to dashboard
        </Link>
      </div>
    </div>
  );
}
