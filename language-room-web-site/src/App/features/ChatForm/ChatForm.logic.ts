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



export interface Message {
  id: number;
  sender: 'user' | 'consultant';
  name: string;
  text: string;
  time: string;
}

export const messagesMock: Message[] = [
  { id: 1, sender: 'user', name: 'Анна', text: 'Добро пожаловать! Меня зовут Анна, я консультант. Чем могу помочь?', time: '14:30:15' },
  { id: 2, sender: 'consultant', name: 'Пользователь', text: 'Здравствуйте! Хотел бы узнать подробнее о ваших услугах.', time: '14:30:28' },
  { id: 3, sender: 'user', name: 'Анна', text: 'Конечно! Расскажите, что именно вас интересует?', time: '14:30:35' },
  { id: 4, sender: 'consultant', name: 'Пользователь', text: 'Меня интересуют тарифы и возможности подключения.', time: '14:30:42' }
];

export interface Issue {
  id: number;
  title: string;
  time: string;
  type: 'error' | 'warning' | 'success';
}

export interface Recommendation {
  id: number;
  title: string;
  description: string;
}


export const issuesMock: Issue[] = [
  { id: 1, title: 'Неправильное приветствие', time: '14:30:15', type: 'error' },
  { id: 2, title: 'Упущенная возможность', time: '14:31:20', type: 'warning' },
  { id: 3, title: 'Хорошее завершение', time: '14:42:49', type: 'success' }
];

export const recommendationsMock: Recommendation[] = [
  { id: 1, title: 'Улучшить приветствие', description: 'Используйте стандартную формулу приветствия с указанием компании и своей роли' },
  { id: 2, title: 'Активнее предлагать услуги', description: 'При обсуждении основных услуг всегда упоминайте дополнительные возможности' },
  { id: 3, title: 'Продолжать хорошую работу', description: 'Завершение диалога было выполнено отлично - используйте этот подход и дальше' }
];