type Listener = (payload: any) => void;

let listeners: Listener[] = [];

export const subscribeToChatInvite = (cb: Listener) => {
  listeners.push(cb);
};

export const sendChatInvite = (payload: any) => {
  setTimeout(() => {
    listeners.forEach(cb => cb(payload));
  }, 1000);
};
