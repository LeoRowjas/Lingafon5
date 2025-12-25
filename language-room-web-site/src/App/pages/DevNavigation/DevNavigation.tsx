// DevNavigation.tsx
import { Link } from 'react-router-dom'
import styles from './DevNavigation.module.scss'

export function DevNavigation() {
    const routes = [
        { path: '/login', name: 'Login' },
        { path: '/register', name: 'Registration' },
        { path: '/profile', name: 'Profile' },
        { path: '/create-chat', name: 'Create Chat' },
        { path: '/select-role', name: 'Select Role' },
        { path: '/student-choice', name: 'Student Choice' },
        { path: '/loading-in', name: 'Loading Invitations' },
        { path: '/confirm-in', name: 'Confirmed Invitations' },
        { path: '/task', name: 'Task Statistics' },
        { path: '/dialog', name: 'Dialog' },
        { path: '/chat/1', name: 'Chat (example id: 1)' },
        { path: '/live-dialog', name: 'Live Dialog' },
        { path: '/groups-list', name: 'Groups List' },
        { path: '/group-details', name: 'Group Details' },
        { path: '/dialog-report', name: 'Dialog Report' },
        { path: '/active-sessions', name: 'Active Sessions' },
    ]

    return (
        <div className={styles.container}>
            <div className={styles.card}>
                <h1 className={styles.title}>🚀 Dev Navigation</h1>
                <p className={styles.subtitle}>Быстрый доступ ко всем страницам</p>
                
                <div className={styles.grid}>
                    {routes.map((route) => (
                        <Link 
                            key={route.path} 
                            to={route.path} 
                            className={styles.link}
                        >
                            <span className={styles.linkText}>{route.name}</span>
                            <span className={styles.linkPath}>{route.path}</span>
                        </Link>
                    ))}
                </div>
            </div>
        </div>
    )
}