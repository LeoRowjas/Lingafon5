import { useEffect, useState, useRef } from "react";
import { Link, useNavigate } from "react-router-dom";
import styles from "./IncomingInvitationsForm.module.scss";

import type { Invite } from "../../entities/invite/model/types";
import { getInvites, acceptInvite, deleteInvite } from "../../entities/invite/api/inviteApi";

export function IncomingInvitationsForm() {
  const [invites, setInvites] = useState<Invite[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const timersRef = useRef<Record<string, number>>({});

  useEffect(() => {
    loadInvites();

    return () => {
//      // очистка всех таймеров при уходе со страницы
//      Object.values(timersRef.current).forEach(clearTimeout);
    };
  }, []);

  async function loadInvites() {
    try {
      const data = await getInvites();
//     const now = Date.now();
//
//      const validInvites: Invite[] = [];
//
//      for (const invite of data) {
//        const expiresAt = new Date(invite.expiresAt).getTime();
//        const timeLeft = expiresAt - now;
//
//        if (invite.isUsed || timeLeft <= 0) {
//          await deleteInvite(invite.id);
//          continue;
//        }
//
//        validInvites.push(invite);
//
//        // таймер авто-удаления
//        timersRef.current[invite.id] = window.setTimeout(
//          async () => {
//            await deleteInvite(invite.id);
//            setInvites((prev) =>
//              prev.filter((i) => i.id !== invite.id)
//            );
//          },
//          timeLeft
//        );
//      }

      setInvites(data);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  }

  async function handleAccept(invite: Invite) {
    try {
      await acceptInvite(invite.token);

      setInvites((prev) => prev.filter((i) => i.id !== invite.id));

      navigate("/dialog");
    } catch (e) {
      alert("Не удалось принять приглашение");
    }
  }

  async function handleReject(invite: Invite) {
    try {
      await deleteInvite(invite.token);

      setInvites((prev) =>
        prev.filter((i) => i.token !== invite.token)
      );
    } catch (e) {
      alert("Не удалось отклонить приглашение");
    }
  }

  return (
    <div className={styles.bg}>

      <section className={styles.container}>
        <h1 className={styles.title}>Входящие приглашения</h1>
        <Link className={styles.back} to="/profile">
          ← Вернуться в личный кабинет
        </Link>

        <div className={styles.box}>
          <h2 className={styles.boxTitle}>Новые приглашения</h2>

          {loading && <p>Загрузка...</p>}

          {!loading && invites.length === 0 && (
            <p>Нет новых приглашений</p>
          )}

          <div className={styles.list}>
            {invites.map((invite) => (
              <div key={invite.id} className={styles.card}>
                <div className={styles.info}>
                  <h3>Приглашение на диалог</h3>
                  <p>
                    <strong>От: Тестовый Пользователь</strong>
                  </p>
                  <span>
                    Действует до:{" "}
                    {new Date(invite.expiresAt).toLocaleString()}
                  </span>
                </div>

                <div className={styles.actions}>
                  <button
                    className={styles.accept}
                    onClick={() => handleAccept(invite)}
                  >
                    Принять
                  </button>

                  <button
                    className={styles.reject}
                    onClick={() => handleReject(invite)}
                  >
                    Отклонить
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>
    </div>
  );
}