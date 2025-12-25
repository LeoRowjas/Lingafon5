// features/GroupDetails/GroupDetails.tsx
import bgImage from "@assets/bgLogin.png"
import { BackButton } from '@ui/BackButton/BackButton'
import { StudentCard } from '@ui/StudentCard/StudentCard'
import { useParams } from 'react-router-dom'
import styles from './GroupDetails.module.scss'

interface Student {
  id: string
  name: string
  averageScore: number
}

const studentsMock: Student[] = [
  { id: '1', name: 'Иванов Иван Иванович', averageScore: 25 },
  { id: '2', name: 'Петров Петр Петрович', averageScore: 23 },
]

export function GroupDetails() {
  const { groupId } = useParams<{ groupId: string }>()

  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />
      <div className={styles.formBlock}>
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Учебная группа ГР-2024-01</h1>
        <BackButton label="Назад к группам" to="/groups" />
      </div>

      <div className={styles.studentsGrid}>
        {studentsMock.map((student) => (
          <StudentCard
            key={student.id}
            id={student.id}
            groupId={groupId!}
            name={student.name}
            averageScore={student.averageScore}
          />
        ))}
      </div>
    </div>
    </div>
    </div>
  )
}