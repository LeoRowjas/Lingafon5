import { api } from "./axios";
import type { RegisterRequest, AuthResponse } from "../types/auth.types";

export const register = async (data: RegisterRequest) => {
  const response = await api.post<AuthResponse>(
    "/Auth/register",
    data
  );
  return response.data;
};