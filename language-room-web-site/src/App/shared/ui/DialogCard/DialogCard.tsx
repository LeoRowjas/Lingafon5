// ui/DialogCard/DialogCard.tsx
import { useNavigate } from 'react-router-dom'
import styles from './DialogCard.module.scss'

interface DialogCardProps {
  id: string
  title: string
  role: string
  score: number
}

export function DialogCard({ id, title, role, score }: DialogCardProps) {
  const navigate = useNavigate()

  const handleClick = () => {
    navigate(`/dialogs/${id}/report`)
  }

  return (
    <div className={styles.dialogCard} onClick={handleClick}>
      <h3 className={styles.dialogTitle}>{title}</h3>
      <div className={styles.info}>
        <span className={styles.role}>Роль: {role}</span>
        <span className={styles.score}>Оценка: {score}</span>
      </div>
    </div>
  )
}