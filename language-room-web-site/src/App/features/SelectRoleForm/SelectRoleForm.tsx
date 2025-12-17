import styles from "./SelectRoleForm.module.scss";
import bgImage from "@assets/bgLogin.png";
import {Link} from 'react-router-dom'
import { useStudentChoice } from "@entities/ChatSelectionContext";
import { useNavigate } from "react-router-dom";

export const SelectRoleForm = () => {

    const { choice, setRole } = useStudentChoice();
    const navigate = useNavigate();


  return (

    <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />

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

            <div className={styles.line} />
        </div>

        <p className={styles.subtitle}>
            Выберите свою роль в диалоге
        </p>

        <div className={styles.selectedTheme}>
            <span>Выбранная тема:</span>
            <strong>{choice.theme}</strong>
        </div>

        <div className={styles.rolesGrid}>
            <div className={styles.roleCard}
                        onClick={() => {
                setRole("Консультант");
                navigate("/student-choice");
            }}
            >
            <span className={styles.roleIcon}>К</span>
            <div>
                <h3>Консультант</h3>
                <p>Помогает клиенту с вопросами</p>
            </div>
            </div>

            <div className={styles.roleCard}
            onClick={() => {
                setRole("Клиент");
                navigate("/student-choice");
            }}>
            <span className={styles.roleIconBlue}>К</span>
            <div>
                <h3>Клиент</h3>
                <p>Обращается за помощью</p>
            </div>
            </div>
        </div>

        <Link to="/create-chat" className={styles.backButton}>Назад</Link>
        </section>
    </div>
  );
};
