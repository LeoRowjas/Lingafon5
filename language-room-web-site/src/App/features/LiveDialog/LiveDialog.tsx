// features/LiveDialog/LiveDialog.tsx
import bgImage from "@assets/bgLogin.png"
import { ActionButton } from '@ui/ActionButton/ActionButton'
import { BackButton } from '@ui/BackButton/BackButton'
import styles from './LiveDialog.module.scss'

interface Message {
  id: number
  sender: 'C1' | 'C2'
  text: string
  time: string
}

const messagesMock: Message[] = [
  { id: 1, sender: 'C1', text: 'Добро пожаловать на собеседование! Расскажите немного о себе.', time: '14:32' },
  { id: 2, sender: 'C2', text: 'Спасибо! Меня зовут Иван, я изучаю английский язык уже 3 года...', time: '14:33' },
  { id: 3, sender: 'C1', text: 'Отлично! Какой у вас опыт работы в команде?', time: '14:34' },
]

export function LiveDialog() {
  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />

    <div className={styles.container}>
      <div className={styles.formBlock}>
      <div className={styles.header}>
        <h1 className={styles.title}>Диалог в реальном времени</h1>
        <BackButton label="Назад к сессиям" to="/active-sessions" />
      </div>

      <div className={styles.dialogCard}>
        <div className={styles.dialogHeader}>
          <div className={styles.headerLeft}>
            <h3 className={styles.dialogTitle}>Деловые переговоры</h3>
            <div className={styles.participants}>
              <span>Иванов Иван Иванович</span>
              <span className={styles.divider}>&</span>
              <span>Петров Петр Петрович</span>
            </div>
          </div>
          <span className={styles.recBadge}>REC</span>
        </div>

        <div className={styles.messagesArea}>
          {messagesMock.map((msg) => (
            <div key={msg.id} className={`${styles.message} ${styles[msg.sender.toLowerCase()]}`}>
              <div className={styles.avatar}>{msg.sender}</div>
              <div className={styles.messageContent}>
                <div className={styles.messageText}>{msg.text}</div>
                <div className={styles.messageTime}>{msg.time}</div>
              </div>
            </div>
          ))}
        </div>

        <div className={styles.actions}>
          <ActionButton variant="red">
            Завершить сессию
          </ActionButton>
        </div>
      </div>
      </div>
    </div>
    </div>
  )
}