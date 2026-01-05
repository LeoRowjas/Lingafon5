import { useState } from 'react'
import styles from "./CreateChat.module.scss"
import bgImage from "@assets/bgLogin.png"
import { Link } from "react-router-dom"
import { useStudentChoice } from '@entities/ChatSelectionContext'

export const CreateChatForm = () => {

  const { setTheme } = useStudentChoice();
  const [isAIMode, setIsAIMode] = useState(false)

  const cards = [
  { id: 1, title: "Консультация по продукту" },
  { id: 2, title: "Поддержка по сервису" },
  { id: 3, title: "Настройка продукта" },
  { id: 4, title: "Обучение и инструкции" },
  ]


  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />

    <section className={styles.createChat}>
      <div className={styles.header}>
        <div className={styles.icon}>Д</div>
        <h1>Создание диалога</h1>
      </div>

      <div className={styles.step}>
        <span className={styles.stepNumber}>1</span>
        <span className={styles.stepText}>Тема</span>
        <div className={styles.line} />
      </div>

      <div className={styles.titleRow}>
          <p className={styles.subtitle}>
            {isAIMode ? "Выберите тему диалога с ИИ" : "Выберите тему диалога"}
          </p>

          <div className={styles.toggle}>
            <button
              className={`${styles.toggleBtn} ${!isAIMode ? styles.active : ''}`}
              onClick={() => setIsAIMode(false)}
            >
              Обычные
            </button>
            
            <button
              className={`${styles.toggleBtn} ${isAIMode ? styles.active : ''}`}             
            >
              <Link to="/ai-chat">
              С ИИ
              </Link>
            </button>
          </div>
          </div>

        <div className={styles.grid}>
          {cards.map(card => (
            <Link
              key={card.id}
              to="/select-role"
              onClick={() => setTheme(card.title)}
              state={{ selectedCard: card, isAIMode }}
              className={styles.card}
            >
              <span className={styles.cardNumber}>{card.id}</span>
              <div>
                <h3>{card.title}</h3>
                <p>Нажмите для выбора</p>
              </div>
            </Link>
          ))}
        </div>
        <Link to="/profile" className={styles.backButton}>Назад</Link>
      </section>
    </div>
  )
}
