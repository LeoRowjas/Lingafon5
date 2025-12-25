// features/GroupsList/GroupsList.tsx
import bgImage from "@assets/bgLogin.png"
import { BackButton } from '@ui/BackButton/BackButton'
import { GroupCard } from '@ui/GroupCard/GroupCard'
import styles from './GroupsList.module.scss'

interface Group {
  id: string
  name: string
  studentsCount: number
}

const groupsMock: Group[] = [
  { id: '1', name: 'ГР-2024-01', studentsCount: 25 },
  { id: '2', name: 'ГР-2024-02', studentsCount: 23 },
  { id: '3', name: 'ГР-2024-03', studentsCount: 27 },
]

export function GroupsList() {
  return (
    <div className={styles.bg}>
            <img className={styles.bgImage} src={bgImage} alt="background" />
            
    <div className={styles.container}>
      <div className={styles.formBlock}>
      <div className={styles.header}>
        <h1 className={styles.title}>Учебные группы</h1>
        <BackButton label="Назад" to="/" />
      </div>

      <div className={styles.groupsGrid}>
        {groupsMock.map((group) => (
          <GroupCard
            key={group.id}
            id={group.id}
            name={group.name}
            studentsCount={group.studentsCount}
          />
        ))}
      </div>
    </div>
    </div>
    </div>
  )
}