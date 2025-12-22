
import styles from "./InfoItem.module.scss";

export const InfoItem = ({
    label,
    children,
    }: {
    label: string;
    children: React.ReactNode;
    }) => (
    <div className={styles.infoItem}>
        <span className={styles.label}>{label}</span>
        <strong>{children}</strong>
    </div>
);