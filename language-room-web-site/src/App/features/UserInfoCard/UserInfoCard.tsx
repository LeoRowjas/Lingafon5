import {UserCard} from '@ui/UserCard/UserCard'
import styles from "./UserInfoCard.module.scss";
import bgImage from "@assets/bgLogin.png";

export const UserInfoCard = () => {
  return (
    <div>
      <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />
        
          <div className={styles.form}>
              <UserCard/>
          </div>
      </div>
    </div>
  );
};
