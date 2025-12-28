const WS_URL = import.meta.env.VITE_WS_URL;

export function connectOnlineWs(token: string) {
  const url = new URL(`${WS_URL}/ws`);
  url.searchParams.set("access_token", token);

  return new WebSocket(url.toString());
}
