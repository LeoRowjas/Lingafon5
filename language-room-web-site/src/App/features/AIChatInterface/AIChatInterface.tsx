import bgImage from "@assets/bgLogin.png"
import { useState } from 'react'
import styles from './AIChatInterface.module.scss'
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
  const [messages, setMessages] = useState<Message[]>([
    {
      id: 1,
      type: 'text',
      sender: 'ai',
      content: 'Привет! Я ваш ИИ-помощник. Чем могу помочь?',
      time: '14:30'
    },
    {
      id: 2,
      type: 'voice',
      sender: 'user',
      content: '',
      time: '14:31',
      voiceDuration: '0:04',
      transcription: 'Привет! Расскажи мне о погоде',
      showTranscription: false
    },
    {
      id: 3,
      type: 'voice',
      sender: 'ai',
      content: '',
      time: '14:32',
      voiceDuration: '0:16',
      transcription: 'К сожалению, у меня нет доступа к актуальным данным о погоде. Но я могу рассказать вам о том, как формируется погода или помочь с другими вопросами!',
      showTranscription: false
    }
  ])

  const [inputText, setInputText] = useState('')
  const [isRecording, setIsRecording] = useState(false)

  const toggleTranscription = (messageId: number) => {
    setMessages(messages.map(msg => 
      msg.id === messageId 
        ? { ...msg, showTranscription: !msg.showTranscription }
        : msg
    ))
  }

  const sendTextMessage = () => {
    if (inputText.trim()) {
      const newMessage: Message = {
        id: Date.now(),
        type: 'text',
        sender: 'user',
        content: inputText,
        time: new Date().toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' })
      }
      setMessages([...messages, newMessage])
      setInputText('')
    }
  }

  const startRecording = () => {
    setIsRecording(true)
    console.log('Начало записи голосового...')
    // Здесь будет логика записи голосового через MediaRecorder API
  }

  const stopRecording = () => {
    setIsRecording(false)
    console.log('Отправка голосового...')

    // Симуляция отправки голосового
    const newVoiceMessage: Message = {
      id: Date.now(),
      type: 'voice',
      sender: 'user',
      content: '',
      time: new Date().toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' }),
      voiceDuration: '0:05',
      transcription: 'Текст вашего голосового сообщения',
      showTranscription: false
    }
    setMessages([...messages, newVoiceMessage])
  }

  const handleMicClick = () => {
    if (isRecording) {
      stopRecording()
    } else {
      startRecording()
    }
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
                  <button className={styles.playButton}>▶</button>
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