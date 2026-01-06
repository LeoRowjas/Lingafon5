import { authFetch } from "../../../../shared/api/authFetch";

const API_URL = import.meta.env.VITE_API_URL;

function getToken() {
  return localStorage.getItem("token");
}

export async function getInvites() {
  const res = await fetch(`${API_URL}/Invite`, {
    headers: {
      Authorization: `Bearer ${getToken()}`,
    },
  });

  if (!res.ok) {
    throw new Error("Ошибка получения приглашений");
  }

  return res.json();
}

export async function acceptInvite(token: string): Promise<void> {
  const response = await authFetch(
    `${API_URL}/Invite/accept?token=${token}`
  );

  if (!response.ok) {
    throw new Error("Ошибка принятия приглашения");
  }
}

export async function sendInvite(
  teacherId: string,
  expiresAt: string
): Promise<{ token: string }> {
  const res = await fetch(`${API_URL}/Invite`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getToken()}`,
    },
    body: JSON.stringify({
      teacherId,
      expiresAt,
    }),
  });

  if (!res.ok) {
    throw new Error("Ошибка отправки приглашения");
  }
  return res.json();
}

export async function deleteInvite(id: string): Promise<void> {
  const res = await fetch(`${API_URL}/Invite/${id}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getToken()}`,
    },
  });

  if (!res.ok) {
    throw new Error("Ошибка отклонения приглашения");
  }
  return res.json();
}