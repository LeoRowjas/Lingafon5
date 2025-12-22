
import { Link } from 'react-router-dom'
import styles from './Header.module.scss'
import logo from '@assets/logo.png'

export function Header() {
    return (
        <header className={styles.header}>
            <Link to="/" className={styles.logoLink}>
                <img src={logo} alt="Logo" className={styles.logo} />
            </Link>
        </header>
    )
}