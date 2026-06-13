import axiosClient from './axiosClient';

// Task API calls. Each returns the backend's ApiResponse<T> envelope.
// All of these hit protected endpoints; the JWT is attached automatically by
// the request interceptor in axiosClient.

// filter: { status?, priority? } — undefined members are omitted from the query.
export async function getTasks(filter = {}) {
  const { data } = await axiosClient.get('/tasks', { params: filter });
  return data; // ApiResponse<TaskDto[]>
}

export async function getTaskById(id) {
  const { data } = await axiosClient.get(`/tasks/${id}`);
  return data; // ApiResponse<TaskDto>
}

// payload: { title, description?, priority, assignedTo? }
export async function createTask(payload) {
  const { data } = await axiosClient.post('/tasks', payload);
  return data; // ApiResponse<TaskDto>
}

// payload: partial update — { title?, description?, status?, priority?, assignedTo? }
export async function updateTask(id, payload) {
  const { data } = await axiosClient.put(`/tasks/${id}`, payload);
  return data; // ApiResponse<TaskDto>
}

export async function deleteTask(id) {
  const { data } = await axiosClient.delete(`/tasks/${id}`);
  return data; // ApiResponse<object>
}

export async function getTaskSummary() {
  const { data } = await axiosClient.get('/tasks/summary');
  return data; // ApiResponse<TaskSummaryDto>
}
