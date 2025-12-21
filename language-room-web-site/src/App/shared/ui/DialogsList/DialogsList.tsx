import styles from "./DialogsList.module.scss";


const dialogs = [1, 2, 3];

const dialogStats = {
  role: "Консультант",
  durationSeconds: 722,
  matchPercent: 85,
};

const formatDuration = (seconds: number) => {
  const min = Math.floor(seconds / 60);
  const sec = seconds % 60;

  return `${min} минут ${sec.toString().padStart(2, "0")} секунд`;
};


export function DialogsList(){

  return(
    <div>
        <div className={styles.list}>
          {dialogs.map((_, index) => (
            <div key={index} className={styles.card}>
              <div className={styles.content}>
                <h3 className={styles.cardTitle}>
                  Консультация по продукту
                </h3>

                <div className={styles.meta}>
                  <span className={styles.status}>Завершен</span>
                  <span className={styles.date}>
                    15 декабря 2024, 14:30
                  </span>
                </div>

                <div className={styles.info}>
                  {/* Роль */}
                  <div>
                    <span className={styles.label}>Роль в диалоге</span>
                    <strong>{dialogStats.role}</strong>
                  </div>

                  {/* Длительность */}
                  <div>
                    <span className={styles.label}>Длительность</span>
                    <strong>{formatDuration(dialogStats.durationSeconds)}</strong>
                  </div>

                  {/* Процент совпадения */}
                  <div>
                    <span className={styles.label}>Процент совпадения</span>
                    <div className={styles.progress}>
                      <div className={styles.track}>
                        <div
                          className={styles.bar}
                          style={{ width: `${dialogStats.matchPercent}%` }}
                        />
                      </div>
                      <span>{dialogStats.matchPercent}%</span>
                    </div>
                  </div>
                </div>

              </div>

              <div className={styles.avatar}>64 × 64</div>
            </div>
          ))}
        </div>
    </div>
  )
}