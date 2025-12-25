// features/DialogReport/DialogReport.tsx
import { BackButton } from '@ui/BackButton/BackButton'
import styles from './DialogReport.module.scss'
import bgImage from "@assets/bgLogin.png"

interface Correction {
  id: number
  text: string
}

interface Recommendation {
  id: number
  text: string
}

const correctionsMock: Correction[] = [
  { id: 1, text: 'Улучшить интонацию' },
  { id: 2, text: 'Добавить больше вежливых фраз' },
]

const recommendationsMock: Recommendation[] = [
  { id: 1, text: 'Практиковать активное слушание' },
  { id: 2, text: 'Изучить деловую лексику' },
]

export function DialogReport() {
  return (

    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.container}>
      <div className={styles.formMain}>
      <div className={styles.header}>
        <h1 className={styles.title}>Отчет о диалоге</h1>
        <BackButton to='/group-details' label="Назад к студенту" />
      </div>

      <div className={styles.mainInfo}>
        <h2 className={styles.sectionTitle}>Основная информация</h2>
        <div className={styles.infoGrid}>
          <div className={styles.infoColumn}>
            <div className={styles.infoItem}>
              <span className={styles.label}>Тема диалога</span>
              <span className={styles.value}>Деловые переговоры</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.label}>Оценка</span>
              <span className={styles.scoreValue}>4.5</span>
            </div>
          </div>
          <div className={styles.infoColumn}>
            <div className={styles.infoItem}>
              <span className={styles.label}>Роль в диалоге</span>
              <span className={styles.value}>Менеджер</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.label}>Совпадение с шаблоном</span>
              <span className={styles.percentValue}>87%</span>
            </div>
          </div>
        </div>

        <div className={styles.audioSection}>
          <span className={styles.label}>Запись диалога</span>
          <div className={styles.audioPlayer}>
            <button className={styles.playButton}>▶</button>
            <span className={styles.audioName}>dialog_001.mp3</span>
          </div>
        </div>
      </div>

      <div className={styles.correctionsSection}>
        <h2 className={styles.sectionTitle}>Исправления</h2>
        <ul className={styles.list}>
          {correctionsMock.map((correction) => (
            <li key={correction.id} className={styles.listItem}>
              {correction.text}
            </li>
          ))}
        </ul>
      </div>

      <div className={styles.recommendationsSection}>
        <h2 className={styles.sectionTitle}>Рекомендации</h2>
        <ul className={styles.list}>
          {recommendationsMock.map((rec) => (
            <li key={rec.id} className={styles.listItem}>
              {rec.text}
            </li>
          ))}
        </ul>
      </div>
      </div>
    </div>
    </div>
  )
}