import axiosClient from './axiosClient';

// Authentication API calls. Each returns the backend's ApiResponse<T> envelope
// (i.e. { success, data, message, errors }).

// credentials: { username, password }
export async function login(credentials) {
  const { data } = await axiosClient.post('/auth/login', credentials);
  return data; // ApiResponse<LoginResponseDto>
}
