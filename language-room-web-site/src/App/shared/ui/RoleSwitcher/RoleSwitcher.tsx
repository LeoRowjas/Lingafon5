import { NavLink } from "react-router";
import { PiStudentThin } from "react-icons/pi";
import { PiChalkboardTeacherThin } from "react-icons/pi";
import styles from "./RoleSwitcher.module.scss";

export const RoleSwitcher = () => {
  return (
    <div className={styles.switcher}>
      <NavLink
        to="/register/student"
        className={({ isActive }) =>
          isActive ? `${styles.tab} ${styles.active}` : styles.tab
        }
      >
        <PiStudentThin size={18} />
        <span>Студент</span>
      </NavLink>

      <NavLink
        to="/register/teacher"
        className={({ isActive }) =>
          isActive ? `${styles.tab} ${styles.active}` : styles.tab
        }
      >
        <PiChalkboardTeacherThin size={18} />
        <span>Преподаватель</span>
      </NavLink>
    </div>
  );
};
