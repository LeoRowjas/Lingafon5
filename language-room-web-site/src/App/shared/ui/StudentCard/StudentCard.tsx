// ui/StudentCard/StudentCard.tsx
import { useNavigate } from 'react-router-dom'
import styles from './StudentCard.module.scss'

interface StudentCardProps {
  id: string
  groupId: string
  name: string
  averageScore: number
}

export function StudentCard({ id, groupId, name, averageScore }: StudentCardProps) {
  const navigate = useNavigate()

  const handleClick = () => {
    navigate(`/groups/${groupId}/students/${id}`)
  }

  return (
    <div className={styles.studentCard} onClick={handleClick}>
      <h3 className={styles.studentName}>{name}</h3>
      <p className={styles.score}>Средний балл: {averageScore}</p>
    </div>
  )
}