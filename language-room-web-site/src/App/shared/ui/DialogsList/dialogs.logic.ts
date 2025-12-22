/* =========================
   TYPES
========================= */

export type DialogStatus =
  | "active"
  | "completed"
  | "pending"
  | "cancelled";

export interface Dialog {
  id: string;
  title: string;
  status: DialogStatus;
  date: string;
  role: string;
  durationSeconds: number;
  matchPercent: number;
  clientName?: string;
  avatarUrl?: string;
}

/* =========================
   MOCK DATA
========================= */

export const dialogsMock: Dialog[] = [
  {
    id: "chat_1a2b3c4d",
    title: "Консультация по продукту A",
    status: "completed",
    date: "15 декабря 2024, 14:30",
    role: "Консультант",
    durationSeconds: 722,
    matchPercent: 100,
    clientName: "Иван Иванов",
  },
  {
    id: "chat_1a2b3c4d",
    title: "Консультация",
    status: "completed",
    date: "15 декабря 2024, 11:40",
    role: "Консультант",
    durationSeconds: 546,
    matchPercent: 34,
    clientName: "Анна Петрова",
  },
  {
    id: "chat_1a2b3c4d",
    title: "Консультация по продукту A",
    status: "completed",
    date: "15 декабря 2024, 14:30",
    role: "Консультант",
    durationSeconds: 722,
    matchPercent: 67,
    clientName: "Иван Иванов",
  }
];

/* =========================
   HELPERS
========================= */

export const formatDuration = (seconds: number) => {
  const min = Math.floor(seconds / 60);
  const sec = seconds % 60;
  return `${min} минут ${sec.toString().padStart(2, "0")} секунд`;
};

export const getStatusText = (status: DialogStatus) => {
  switch (status) {
    case "active": return "В процессе";
    case "completed": return "Завершен";
    case "pending": return "Ожидание";
    case "cancelled": return "Отменен";
  }
};
