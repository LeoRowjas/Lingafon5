// features/ActiveSessions/ActiveSessions.tsx
import bgImage from "@assets/bgLogin.png"
import { BackButton } from '@ui/BackButton/BackButton'
import { useNavigate } from 'react-router-dom'
import styles from './ActiveSessions.module.scss'


interface Session {
  id: string
  title: string
  participants: string[]
  startTime: string
  isLive: boolean
}

const sessionsMock: Session[] = [
  {
    id: '1',
    title: 'Деловые переговоры',
    participants: ['Иванов Иван Иванович', 'Петров Петр Петрович'],
    startTime: '14:30',
    isLive: true,
  },
  {
    id: '2',
    title: 'Собеседование',
    participants: ['Сидорова Анна Владимировна', 'Козлов Дмитрий Сергеевич'],
    startTime: '15:00',
    isLive: true,
  },
]

export function ActiveSessions() {
  const navigate = useNavigate()

  const handleSessionClick = (sessionId: string) => {
    navigate(`/live-dialog/${sessionId}`)
  }

  return (
    <div className={styles.bg}>
    <div>
      <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.container}>
      <div className={styles.formBlock}>
      <div className={styles.header}>
        <h1 className={styles.title}>Активные парные сессии</h1>
        <BackButton label="Назад" to="/" />
      </div>

      <div className={styles.sessionsGrid}>
        {sessionsMock.map((session) => (
          <div
            key={session.id}
            className={styles.sessionCard}
            onClick={() => handleSessionClick(session.id)}
          >
            <div className={styles.cardHeader}>
              <h3 className={styles.sessionTitle}>{session.title}</h3>
              {session.isLive && <span className={styles.liveBadge}>В эфире</span>}
            </div>
            <div className={styles.participants}>
              <span className={styles.participant}>{session.participants[0]}</span>
              <span className={styles.divider}>&</span>
              <span className={styles.participant}>{session.participants[1]}</span>
            </div>
            <div className={styles.startTime}>Начало: {session.startTime}</div>
          </div>
        ))}
      </div>
    </div>
    </div>
    </div>
    </div>
  )
}