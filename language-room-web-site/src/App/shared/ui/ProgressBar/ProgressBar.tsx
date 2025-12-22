import styles from "./ProgressBar.module.scss";

export const ProgressBar = ({ value }: { value: number }) => (
  <div className={styles.progress}>
    <div className={styles.track}>
      <div
        className={styles.bar}
        style={{ width: `${value}%` }}
      />
    </div>
    <span>{value}%</span>
  </div>
);