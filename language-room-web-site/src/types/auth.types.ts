export interface RegisterRequest {
  firstName: string;
  lastName: string;
  //middleName?: string;
  email: string;
  password: string;
  //confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  userId: string;
  email: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  userId: string;
}