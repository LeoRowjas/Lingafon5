import styles from './StudentChoiceForm.module.scss'
import bgImage from "@assets/bgLogin.png";
import { useStudentChoice } from "@entities/ChatSelectionContext";
import {Link} from 'react-router-dom'
import { useNavigate } from "react-router-dom";



export function StudentChoiceForm () {

    // test
    type Student = {
        id: number;
        name: string;
        online: boolean;
        };

        const students: Student[] = [
        {
            id: 1,
            name: "Анна Петрова",
            online: true,
        },
        {
            id: 2,
            name: "Михаил Иванов",
            online: false,
        },
    ];
    //


    const { choice } = useStudentChoice();
    const { setStudent } = useStudentChoice();
    const navigate = useNavigate();


    return(
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

                <p className={styles.subtitle}>
                    Выберите студента для приглашения
                </p>

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

                <div className={styles.studentsList}>
                    {students.map(student => (
                        <div key={student.id} className={styles.studentCard}>
                        <div className={styles.studentInfo}>
                            <div className={styles.avatar}>
                            {student.name
                                .split(" ")
                                .map(n => n[0])
                                .join("")}
                            </div>

                            <div>
                            <div className={styles.studentName}>
                                Сессия {student.id}, {student.name}
                            </div>
                            <div className={styles.status}>
                                <span
                                className={
                                    student.online
                                    ? styles.statusOnline
                                    : styles.statusOffline
                                }
                                />
                                {student.online ? "Онлайн" : "Оффлайн"}
                            </div>
                            </div>
                        </div>

                        <button className={styles.inviteButton}
                        onClick={() => {
                            setStudent({ id: student.id, name: student.name });
                            navigate("/loading-in");
                        }}>
                            ОТПРАВИТЬ ПРИГЛАШЕНИЕ
                        </button>
                        </div>
                    ))}
                </div>

                <Link to="/create-chat" className={styles.backButton}>Назад</Link>
                </section>
            </div>
        </div>
    )
}