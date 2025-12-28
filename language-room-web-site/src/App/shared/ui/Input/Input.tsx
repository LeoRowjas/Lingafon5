// ui/Input/Input.tsx
import styles from './Input.module.scss'

interface InputProps {
  label: string
  value: string
  onChange: (value: string) => void
  placeholder?: string
}

export function Input({ label, value, onChange, placeholder }: InputProps) {
  return (
    <div className={styles.inputGroup}>
      <label className={styles.label}>{label}</label>
      <input
        type="text"
        className={styles.input}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
      />
    </div>
  )
}