import styles from "./UsereCard.module.scss";
import Avatars from '@assets/Avatar.png'


export const UserCard = () => {
  return (
    <div className={styles.card}>
    <div className={styles.rectangle}>
    
      <div className={styles.wrapper}>
        <div className={styles.blogAuthor}>
            <img
          src={Avatars}
          alt="avatar"
          className={styles.avatar}
        />

        <div>
          <p className={styles.name}>ФИО: Александр Сергеевич Пушкин</p>
          <p className={styles.teacher}>Преподаватель: Козлова Софья Владимировна</p>
        </div>
        </div>
        <a className={styles.group}>Учебная группа: АТ-01</a>
      </div>
      </div>
    </div>
  );
};
