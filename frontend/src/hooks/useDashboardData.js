import { useState, useEffect, useCallback } from 'react';
import { getTasks, getTaskSummary } from '../api/taskApi';

// Loads the dashboard's data (task list + summary). The `filters` object
// ({ status?, priority? }) is sent to the API so filtering happens server-side.
// A 401 is handled globally by the axios interceptor (redirect to /login).
//
// NOTE: pass a *stable* filters reference (e.g. useMemo) so this doesn't refetch
// on every render.
export function useDashboardData(filters = {}) {
  const [tasks, setTasks] = useState([]);
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = useCallback(async () => {
    setLoading(true);
    setError('');

    try {
      const [tasksResponse, summaryResponse] = await Promise.all([
        getTasks(filters),     // filtered list
        getTaskSummary(),      // summary is always the full totals
      ]);

      setTasks(tasksResponse.data ?? []);
      setSummary(summaryResponse.data ?? null);
    } catch (err) {
      // Keep any previously loaded data; just surface the error.
      setError(err.response?.data?.message || 'Failed to load dashboard data.');
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    load();
  }, [load]);

  return { tasks, summary, loading, error, reload: load };
}
