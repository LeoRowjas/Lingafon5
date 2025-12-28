import React, { createContext, useContext, useEffect, useMemo, useRef, useState } from "react";
import type { User } from "../../types/user";
import { getUsers, getUserStatus } from "../../shared/api/userApi";
import { connectOnlineWs } from "../../shared/api/onlineWs";

type UserWithStatus = User & { isOnline: boolean; lastSeenAt?: string };

type OnlineUsersContextValue = {
  loading: boolean;
  error: string | null;
  users: UserWithStatus[];
  onlineUsers: UserWithStatus[];
  refresh: () => Promise<void>;
};

const OnlineUsersContext = createContext<OnlineUsersContextValue | null>(null);

export function OnlineUsersProvider({ children }: { children: React.ReactNode }) {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [users, setUsers] = useState<UserWithStatus[]>([]);
  const wsRef = useRef<WebSocket | null>(null);

  const refresh = async () => {
    try {
      setError(null);
      const list = await getUsers();

      const withStatuses: UserWithStatus[] = await Promise.all(
        list.map(async (u: any) => {
          try {
            const st = await getUserStatus(u.id);
            return { ...u, isOnline: st.isOnline, lastSeenAt: st.lastSeenAt };
          } catch {
            return { ...u, isOnline: false };
          }
        })
      );

      setUsers(withStatuses);
    } catch (e: any) {
      setError(e?.message ?? "Ошибка загрузки пользователей");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    refresh();
    
  }, []);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      console.warn("Нет token в localStorage — WS не подключен");
      return;
    }

    const ws = connectOnlineWs(token);
    wsRef.current = ws;

    ws.onopen = () => console.log("WS connected (global)");
    ws.onerror = (e) => console.error("WS error (global)", e);
    ws.onclose = () => console.log("WS closed (global)");

    ws.onmessage = (msg) => {
      try {
        const data = JSON.parse(msg.data);

        if (data?.userId && typeof data?.status === "string") {
          const isOnline = data.status === "online";

          setUsers((prev) =>
            prev.map((u) => (u.id === data.userId ? { ...u, isOnline } : u))
          );
        }
      } catch {
        
      }
    };

    return () => {
      wsRef.current?.close();
      wsRef.current = null;
    };
  }, []);

  // фолбэк polling
  useEffect(() => {
    if (loading || users.length === 0) return;

    let cancelled = false;

    async function refreshStatuses() {
      try {
        const updated = await Promise.all(
          users.map(async (u) => {
            try {
              const st = await getUserStatus(u.id);
              return { ...u, isOnline: st.isOnline, lastSeenAt: st.lastSeenAt };
            } catch {
              return u;
            }
          })
        );
        if (!cancelled) setUsers(updated);
      } catch {
        
      }
    }

    const timer = window.setInterval(refreshStatuses, 15000);
    return () => {
      cancelled = true;
      window.clearInterval(timer);
    };
  }, [loading, users]);

  const onlineUsers = useMemo(() => users.filter((u) => u.isOnline), [users]);

  const value: OnlineUsersContextValue = {
    loading,
    error,
    users,
    onlineUsers,
    refresh,
  };

  return <OnlineUsersContext.Provider value={value}>{children}</OnlineUsersContext.Provider>;
}

export function useOnlineUsers() {
  const ctx = useContext(OnlineUsersContext);
  if (!ctx) throw new Error("useOnlineUsers must be used inside OnlineUsersProvider");
  return ctx;
}