const API_BASE_URL = import.meta.env.VITE_API_URL

function getToken() {
  return localStorage.getItem("token");
}

export async function sendVoice(dialogId: string, file: File) {
  const formData = new FormData()
  formData.append('DialogId', dialogId)
  formData.append('File', file)

  const response = await fetch(`${API_BASE_URL}/api/Message/voice`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${getToken()}`,
    },
    body: formData
  })

  if (!response.ok) {
    throw new Error('Ошибка отправки голоса')
  }

  return response.json()
}

export async function getAiReply(dialogId: string) {
  const response = await fetch(`${API_BASE_URL}/api/Message/ai-reply`, {
    method: 'POST',
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getToken()}`,
    },
    body: JSON.stringify({
      dialogId,
      includeHistory: true,
      historyLimit: 10
    })
  })

  if (!response.ok) {
    throw new Error('Ошибка ответа ИИ')
  }

  return response.json()
}
