// ui/GroupCard/GroupCard.tsx
import { useNavigate } from 'react-router-dom'
import styles from './GroupCard.module.scss'

interface GroupCardProps {
  id: string
  name: string
  studentsCount: number
}

export function GroupCard({ id, name, studentsCount }: GroupCardProps) {
  const navigate = useNavigate()

  const handleClick = () => {
    navigate(`/groups/${id}`)
  }

  return (
    <div className={styles.groupCard} onClick={handleClick}>
      <h3 className={styles.groupName}>{name}</h3>
      <p className={styles.studentsCount}>Студентов: {studentsCount}</p>
    </div>
  )
}