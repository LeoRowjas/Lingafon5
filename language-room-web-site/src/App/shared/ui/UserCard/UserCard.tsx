import { useState } from "react";
import styles from "./UsereCard.module.scss";
import avatar from '@assets/Avatar.png'
import { Link } from "react-router-dom";

export const UserCard = () => {

  const [invitesCount] = useState<number>(2);

  const displayCount =
    invitesCount > 9 ? "9+" : invitesCount.toString();

  return (
    <div className={styles.card}>

        <h1>Личный кабинет</h1>

        <div className={styles.avatar}>

          <div className="">
              <h4>Личная информация</h4>
              <p className={styles.subText}>ФИО</p>
              <h3>Иванов Иван Иванович</h3>
              <p className={styles.subText}>Учебная группа</p>
              <h3>ГР-2024-01</h3>
          </div>

          <div className={styles.imgAvatar}>
            <img src={avatar} alt="" />
          </div>

        </div>

        <div className={styles.buttonFooter}>
          

          <Link to="/task" className={styles.buttonBlue}>
          Статистика заданий
          </Link>
          <Link to="/create-chat" className={styles.buttonGreen} >
          Отправить приглашение
          </Link>
          <div className={styles.buttonWithBadge}>
            {invitesCount > 0 && (
              <span className={styles.badge}>{displayCount}</span>
            )}
            <Link to="/incoming-invitations" className={styles.buttonOrange}>
              Входящие приглашения
            </Link>
          </div>
        </div>
    </div>
  );
};
