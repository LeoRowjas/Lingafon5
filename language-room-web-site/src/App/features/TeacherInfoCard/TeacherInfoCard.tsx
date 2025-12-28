import {TeacherCard} from '@ui/TeacherCard/TeacherCard'
import styles from "./TeacherInfoCard.module.scss";
import bgImage from "@assets/bgLogin.png";

export const TeacherInfoCard = () => {
  return (
    <div>
      <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />
        
          <div className={styles.form}>
              <TeacherCard/>
          </div>
      </div>
    </div>
  );
};
