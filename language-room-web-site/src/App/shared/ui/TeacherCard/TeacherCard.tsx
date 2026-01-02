import avatar from '@assets/Avatar.png'
import { useState } from "react"
import { Link } from "react-router-dom"
import styles from "./TeacherCard.module.scss"

export const TeacherCard = () => {

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
              <p className={styles.subText}>Должность</p>
              <h3>Преподаватель английского языка</h3>
          </div>

          <div className={styles.imgAvatar}>
            <img src={avatar} alt="" />
          </div>

        </div>

        <div className={styles.buttonFooter}>
          

          <Link to="/groups-list" className={styles.buttonBlue}>
          Учебные группы
          </Link>
          <Link to="/active-sessions" className={styles.buttonGreen} >
          Активные парные сессии
          </Link>
        </div>
    </div>
  );
};
