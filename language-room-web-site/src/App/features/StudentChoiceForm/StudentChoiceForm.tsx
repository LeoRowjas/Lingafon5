import styles from "./StudentChoiceForm.module.scss";
import bgImage from "@assets/bgLogin.png";
import { useStudentChoice } from "@entities/ChatSelectionContext";
import { Link, useNavigate } from "react-router-dom";
import { useMemo } from "react";

import { useOnlineUsers } from "@entities/OnlineUsersContext";
import { sendInvite } from "../../entities/invite/api/inviteApi";

function getDisplayName(u: any): string {
  if (typeof u?.name === "string" && u.name.trim()) return u.name.trim();

  const full = [u?.firstName, u?.lastName].filter(Boolean).join(" ").trim();
  if (full) return full;

  if (typeof u?.email === "string" && u.email.trim()) return u.email.trim();

  return "Без имени";
}

function getInitials(displayName: string): string {
  const parts = displayName.split(" ").filter(Boolean);
  const initials = parts.slice(0, 2).map((p) => p[0]).join("");
  return (initials || "U").toUpperCase();
}

function getInviteExpiresAt(): string {
  const date = new Date();
  date.setMinutes(date.getMinutes() + 30);
  return date.toISOString();
}

export function StudentChoiceForm() {
  const { choice, setStudent } = useStudentChoice();
  const navigate = useNavigate();

  const { loading, error, onlineUsers } = useOnlineUsers();
  const currentUserId = String(localStorage.getItem("userId") ?? "");

  const onlineForView = useMemo(
  () =>
    onlineUsers
      .filter((u: any) => String(u.id) !== currentUserId)
      .map((u: any) => ({
        ...u,
        displayName: getDisplayName(u),
      })),
  [onlineUsers, currentUserId]
  );

  async function handleInvite(student: any) {
    const expiresAt = getInviteExpiresAt();
    const tokenUser = localStorage.getItem("token");
    
    if (!tokenUser) {
      alert("Вы не авторизованы");
      navigate("/login");
      return;
    }
    try {
      const { token } = await sendInvite(student.id, expiresAt);
      setStudent({ id: student.id, name: student.displayName, inviteToken: token, });

      navigate("/loading-in");
    } catch (e) {
      console.error(e);
      alert("Не удалось отправить приглашение");
    }
  }

  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />

      <div>
        <section className={styles.selectRole}>
          <div className={styles.header}>
            <div className={styles.icon}>Д</div>
            <h1>Создание диалога</h1>
          </div>

          <div className={styles.steps}>
            <div className={styles.step}>
              <span className={styles.stepDone}>1</span>
              <span className={styles.stepText}>Тема</span>
            </div>

            <div className={styles.lineGreen} />

            <div className={styles.step}>
              <span className={styles.stepActive}>2</span>
              <span className={styles.stepText}>Роль</span>
            </div>

            <div className={styles.lineGreen} />

            <div className={styles.step}>
              <span className={styles.stepActive}>3</span>
              <span className={styles.stepText}>Студент</span>
            </div>

            <div className={styles.line} />
          </div>

          <p className={styles.subtitle}>Выберите студента для приглашения</p>

          <div className={styles.selectedTheme}>
            <div className={styles.infoRole}>
              <div>
                <span>тема:</span>
                <strong>{choice.theme}</strong>
              </div>
              <div>
                <span>Ваша роль:</span>
                <strong>{choice.role}</strong>
              </div>
            </div>
          </div>

          {loading && <p className={styles.subtitle}>Загрузка…</p>}
          {error && <p className={styles.subtitle}>Ошибка: {error}</p>}

          {!loading && !error && onlineUsers.length > 0 && (
            <div className={styles.studentsList}>
              {onlineForView.length === 0 ? (
                <p className={styles.subtitle}>Сейчас никого нет онлайн</p>
              ) : (
                onlineForView.map((student: any) => (
                  <div key={student.id} className={styles.studentCard}>
                    <div className={styles.studentInfo}>
                      <div className={styles.avatar}>
                        {getInitials(student.displayName)}
                      </div>

                      <div>
                        <div className={styles.studentName}>{student.displayName}</div>
                        <div className={styles.status}>
                          <span className={styles.statusOnline} />
                          Онлайн
                        </div>
                      </div>
                    </div>

                    <button
                      className={styles.inviteButton}
                      onClick={() => handleInvite(student)}
                    >
                      ОТПРАВИТЬ ПРИГЛАШЕНИЕ
                    </button>
                  </div>
                ))
              )}
            </div>
          )}

          <Link to="/create-chat" className={styles.backButton}>
            Назад
          </Link>
        </section>
      </div>
    </div>
  );
}