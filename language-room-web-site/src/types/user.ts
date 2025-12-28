export type User = {
  id: string;
  name?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
};

export type UserStatusResponse = {
  isOnline: boolean;
  lastSeenAt: string;
};