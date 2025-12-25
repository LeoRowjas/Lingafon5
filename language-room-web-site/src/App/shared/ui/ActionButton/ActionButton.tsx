// ui/ActionButton/ActionButton.tsx
import styles from './ActionButton.module.scss'

interface ActionButtonProps {
  children: React.ReactNode
  variant?: 'blue' | 'green' | 'red'
  onClick?: () => void
}

export function ActionButton({ children, variant = 'blue', onClick }: ActionButtonProps) {
  return (
    <button 
      className={`${styles.actionButton} ${styles[variant]}`}
      onClick={onClick}
    >
      {children}
    </button>
  )
}