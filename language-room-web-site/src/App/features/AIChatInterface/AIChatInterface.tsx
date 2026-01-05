import bgImage from "@assets/bgLogin.png"
import { useState } from 'react'
import styles from './AIChatInterface.module.scss'
import { sendVoice, getAiReply } from '../../../api/aiChat'

interface Message {
  id: number
  type: 'text' | 'voice'
  sender: 'ai' | 'user'
  content: string
  time: string
  voiceDuration?: string
  transcription?: string
  showTranscription?: boolean
}

export function AIChatInterface() {
  const [mediaRecorder, setMediaRecorder] = useState<MediaRecorder | null>(null)
  const [audioChunks, setAudioChunks] = useState<Blob[]>([])
  const [dialogId] = useState(crypto.randomUUID())
  const [inputText, setInputText] = useState('')
  const [isRecording, setIsRecording] = useState(false)
  const [currentAudio, setCurrentAudio] = useState<HTMLAudioElement | null>(null)
  const [playingMessageId, setPlayingMessageId] = useState<number | null>(null)

  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1,
      type: "text",
      sender: "ai",
      content: "Привет! Я ваш ИИ-помощник. Нажмите 🎤 и начните говорить.",
      time: new Date().toLocaleTimeString("ru-RU", { hour: "2-digit", minute: "2-digit" })
    }
  ])

  const togglePlay = (msg: Message) => {
    // если нажали на то же сообщение
    if (playingMessageId === msg.id && currentAudio) {
      currentAudio.pause()
      currentAudio.currentTime = 0
      setPlayingMessageId(null)
      return
    }

    // если что-то другое уже играет
    if (currentAudio) {
      currentAudio.pause()
      currentAudio.currentTime = 0
    }

    // запускаем новое
    const audio = new Audio(msg.content)
    audio.play()

    audio.onended = () => {
      setPlayingMessageId(null)
    }

    setCurrentAudio(audio)
    setPlayingMessageId(msg.id)
  }

  const toggleTranscription = (id: number) => {
    setMessages(prev =>
      prev.map(m =>
        m.id === id ? { ...m, showTranscription: !m.showTranscription } : m
      )
    )
  }

  const sendTextMessage = () => {
    if (!inputText.trim()) return

    setMessages(prev => [
      ...prev,
      {
        id: Date.now(),
        type: "text",
        sender: "user",
        content: inputText,
        time: new Date().toLocaleTimeString("ru-RU", { hour: "2-digit", minute: "2-digit" })
      }
    ])

    setInputText("")
  }

  const startRecording = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true })
      const recorder = new MediaRecorder(stream)

      setAudioChunks([])

      recorder.ondataavailable = e => {
        if (e.data.size > 0) {
          setAudioChunks(prev => [...prev, e.data])
        }
      }

      recorder.start()
      setMediaRecorder(recorder)
      setIsRecording(true)
    } catch (e) {
      console.error("Микрофон недоступен", e)
    }
  }

  const stopRecording = () => {
    if (!mediaRecorder) return

    mediaRecorder.stop()
    setIsRecording(false)

    mediaRecorder.onstop = async () => {
      const blob = new Blob(audioChunks, { type: "audio/webm" })
      const file = new File([blob], "voice.webm", { type: "audio/webm" })
      const audioUrl = URL.createObjectURL(file)

      const userMessageId = Date.now()

      /** добавляем сообщение пользователя */
      setMessages(prev => [
        ...prev,
        {
          id: userMessageId,
          type: "voice",
          sender: "user",
          content: audioUrl,
          time: new Date().toLocaleTimeString("ru-RU", { hour: "2-digit", minute: "2-digit" }),
          showTranscription: false
        }
      ])

      /** отправляем голос */
      try {
        const voiceResult = await sendVoice(dialogId, file)

        setMessages(prev =>
          prev.map(m =>
            m.id === userMessageId
              ? { ...m, transcription: voiceResult.transcription }
              : m
          )
        )

        /** запрашиваем ответ ИИ */
        const aiResult = await getAiReply(dialogId)

        setMessages(prev => [
          ...prev,
          {
            id: Date.now(),
            type: "voice",
            sender: "ai",
            content: aiResult.audioUrl ?? "",
            transcription: aiResult.reply,
            showTranscription: false,
            time: new Date().toLocaleTimeString("ru-RU", { hour: "2-digit", minute: "2-digit" })
          }
        ])
      } catch (e) {
        console.error("Ошибка диалога", e)
      }
    }
  }

  const handleMicClick = () => {
    isRecording ? stopRecording() : startRecording()
  }

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault()
      sendTextMessage()
    }
  }

  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.chatContainer}>
      {/* Header */}
      <div className={styles.chatHeader}>
        <div className={styles.headerLeft}>
          <div className={styles.aiAvatar}>ИИ</div>
          <div className={styles.headerInfo}>
            <h3 className={styles.aiName}>ИИ Помощник</h3>
            <p className={styles.aiStatus}>всегда онлайн</p>
          </div>
        </div>
      </div>

      {/* Messages */}
      <div className={styles.messagesArea}>
        {messages.map((msg) => (
          <div 
            key={msg.id} 
            className={`${styles.messageWrapper} ${styles[msg.sender]}`}
          >
            {msg.sender === 'ai' && (
              <div className={styles.messageAvatar}>ИИ</div>
            )}

            <div className={styles.messageBubble}>
              {msg.type === 'text' ? (
                <p className={styles.messageText}>{msg.content}</p>
              ) : (
                <div className={styles.voiceMessage}>
                  <button
                      className={styles.playButton}
                      onClick={() =>  togglePlay(msg)}
                    >
                      {playingMessageId === msg.id ? "⏸" : "▶"}
                  </button>
                  <div className={styles.voiceProgress}>
                    <div className={styles.progressBar} style={{ width: '0%' }}></div>
                  </div>
                  <span className={styles.voiceDuration}>{msg.voiceDuration}</span>
                  <button 
                    className={styles.transcribeBtn}
                    onClick={() => toggleTranscription(msg.id)}
                    title="Показать текст"
                  >
                    ➜ A
                  </button>
                </div>
              )}

              {msg.type === 'voice' && msg.showTranscription && (
                <div className={styles.transcription}>
                  <p>{msg.transcription}</p>
                </div>
              )}

              <span className={styles.messageTime}>{msg.time}</span>
            </div>

            {msg.sender === 'user' && (
              <div className={`${styles.messageAvatar} ${styles.userAvatar}`}>Я</div>
            )}
          </div>
        ))}
      </div>

      {/* Input */}
      <div className={styles.inputArea}>
        <input
          type="text"
          className={styles.messageInput}
          placeholder="Напишите сообщение..."
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          onKeyPress={handleKeyPress}
        />

        {inputText.trim() ? (
          <button className={styles.sendButton} onClick={sendTextMessage}>
            ➤
          </button>
        ) : (
          <button 
            className={`${styles.micButton} ${isRecording ? styles.recording : ''}`}
            onClick={handleMicClick}
          >
            🎤
          </button>
        )}
      </div>
    </div>
    </div>
  )
}