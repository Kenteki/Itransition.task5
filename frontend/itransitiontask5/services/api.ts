import axios from 'axios';

const API_URL = process.env.NEXT_PUBLIC_API_URL;

// axios instance
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// JWT Token Interceptor
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// 401 Code error (blocked/deleted users)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export interface User {
  id: string;
  name: string;
  email: string;
  position: string;
  registrationTime: string;
  lastLogin: string | null;
  status: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  position: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  name: string;
  email: string;
  status: string;
}

// Api calls for authentication
export const authService = {
  register: (data: RegisterRequest) => 
    api.post('/auth/register', data),
  
  login: (data: LoginRequest) => 
    api.post<LoginResponse>('/auth/login', data),
  
  verifyEmail: (token: string) => 
    api.get(`/auth/verify-email?token=${token}`),
};

// Api calls for user management
export const userService = {
   getAllUsers: () => 
    api.get<User[]>('/user'),
  
  blockUsers: (userIds: string[]) => 
    api.post('/user/block', userIds),
  
  unblockUsers: (userIds: string[]) => 
    api.post('/user/unblock', userIds),
  
  deleteUsers: (userIds: string[]) => 
    api.post('/user/delete', userIds),
  
  deleteUnverified: () => 
    api.post('/user/delete-unverified'),
};

export default api;