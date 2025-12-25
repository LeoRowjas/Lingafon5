import bgImage from "@assets/bgLogin.png"
import styles from "./IncomingInvitationsForm.module.scss"
type Invitation = {
  id: number;
  topic: string;
  from: string;
  date: string;
};

const invitations: Invitation[] = [
  {
    id: 1,
    topic: "Консультация клиента",
    from: "Анна Петрова",
    date: "16 декабря 2024, 10:00",
  },
  {
    id: 2,
    topic: "Техническая поддержка",
    from: "Михаил Сидоров",
    date: "16 декабря 2024, 14:30",
  },
];

export function IncomingInvitationsForm() {
  return (
    <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />
    <section className={styles.container}>

      <h1 className={styles.title}>Входящие приглашения</h1>
      <a className={styles.back} href="#">
        ← Вернуться в личный кабинет
      </a>

      {/* Box */}
      <div className={styles.box}>
        <h2 className={styles.boxTitle}>Новые приглашения</h2>

        <div className={styles.list}>
          {invitations.map((item) => (
            <div key={item.id} className={styles.card}>
              <div className={styles.info}>
                <h3>{item.topic}</h3>
                <p>
                  <strong>От:</strong> {item.from}
                </p>
                <span>{item.date}</span>
              </div>

              <div className={styles.actions}>
                <button className={styles.accept}>Принять</button>
                <button className={styles.reject}>Отклонить</button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
    </div>
  );
}
