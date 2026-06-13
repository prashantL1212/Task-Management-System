import { useState, useEffect, useCallback } from 'react';
import { getTasks, getTaskSummary } from '../api/taskApi';

// Loads the dashboard's data (task list + summary) together, exposing a single
// loading/error state plus a reload() for manual refresh. A 401 is handled
// globally by the axios interceptor (redirect to /login), so it isn't surfaced here.
export function useDashboardData() {
  const [tasks, setTasks] = useState([]);
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = useCallback(async () => {
    setLoading(true);
    setError('');

    try {
      const [tasksResponse, summaryResponse] = await Promise.all([
        getTasks(),
        getTaskSummary(),
      ]);

      setTasks(tasksResponse.data ?? []);
      setSummary(summaryResponse.data ?? null);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load dashboard data.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    load();
  }, [load]);

  return { tasks, summary, loading, error, reload: load };
}
