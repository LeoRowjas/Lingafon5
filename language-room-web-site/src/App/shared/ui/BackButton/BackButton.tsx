import { useNavigate } from 'react-router-dom'
import styles from './BackButton.module.scss'

interface BackButtonProps {
  to?: string
  label: string
}

export function BackButton({ to, label }: BackButtonProps) {
  const navigate = useNavigate()

  const handleClick = () => {
    if (to) {
      navigate(to)
    } else {
      navigate(-1)
    }
  }

  return (
    <button className={styles.backButton} onClick={handleClick}>
      <span className={styles.arrow}>←</span>
      <span className={styles.label}>{label}</span>
    </button>
  )
}