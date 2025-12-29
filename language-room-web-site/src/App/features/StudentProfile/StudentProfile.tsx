// features/StudentProfile/StudentProfile.tsx
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { DialogCard } from '@ui/DialogCard/DialogCard'
import { BackButton } from '@ui/BackButton/BackButton'
import styles from './StudentProfile.module.scss'
import bgImage from "@assets/bgLogin.png"

interface Dialog {
  id: string
  title: string
  role: string
  score: number
}

const dialogsMock: Dialog[] = [
  { id: '1', title: 'Деловые переговоры', role: 'Менеджер', score: 4.5 },
  { id: '2', title: 'Собеседование', role: 'Кандидат', score: 3.8 },
]

export function StudentProfile() {
  const { groupId } = useParams<{ groupId: string }>()

  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.container}>
      <div className={styles.formMain}>
      <div className={styles.header}>
        <h1 className={styles.title}>Профиль ученика</h1>
        <BackButton label="Назад к группе" to={`/group-details`} />
      </div>

      <div className={styles.infoCard}>
        <h2 className={styles.sectionTitle}>Личная информация</h2>
        <div className={styles.infoGrid}>
          <div className={styles.infoItem}>
            <span className={styles.label}>ФИО</span>
            <span className={styles.value}>Иванов Иван Иванович</span>
          </div>
          <div className={styles.infoItem}>
            <span className={styles.label}>Учебная группа</span>
            <span className={styles.value}>ГР-2024-01</span>
          </div>
          <div className={styles.infoItem}>
            <span className={styles.label}>Средний балл</span>
            <span className={styles.scoreValue}>4.2</span>
          </div>
        </div>
      </div>

      <div className={styles.dialogsSection}>
        <h2 className={styles.sectionTitle}>Пройденные диалоги</h2>
        <div className={styles.dialogsGrid}>
          {dialogsMock.map((dialog) => (
            <Link to="/dialog-report">
            <DialogCard
              key={dialog.id}
              id={dialog.id}
              title={dialog.title}
              role={dialog.role}
              score={dialog.score}
            />
            </Link>
          ))}
        </div>
      </div>
      </div>
    </div>
    </div>
  )
}