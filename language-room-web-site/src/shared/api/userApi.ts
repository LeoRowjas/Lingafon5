import { apiGet } from "./http";
import type { User, UserStatusResponse } from "../../types/user";

export function getUsers(): Promise<User[]> {
  return apiGet<User[]>("/api/User");
}

export function getUserStatus(id: string): Promise<UserStatusResponse> {
  return apiGet<UserStatusResponse>(`/api/User/${encodeURIComponent(id)}/status`);
}
