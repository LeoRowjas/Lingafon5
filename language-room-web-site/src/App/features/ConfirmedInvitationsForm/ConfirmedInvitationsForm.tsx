import styles from "./../SelectRoleForm/SelectRoleForm.module.scss";
import bgImage from "@assets/bgLogin.png";
import { useStudentChoice } from "@entities/ChatSelectionContext";
import applayImg from '@assets/applay.png'

export function ConfirmedInvitationsForm () {

    const { choice } = useStudentChoice();

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

                        <div className={styles.lineGreen} />

                        <div className={styles.step}>
                        <span className={styles.stepActive}>4</span>
                        <span className={styles.stepText}>Ожидание</span>
                        </div>

                        <div className={styles.line} />
                    </div>


                    <div className={styles.mainShow}>
                        <img className={styles.clockImg} src={applayImg} alt="" />
                        <p className={styles.subtitleMain}>
                            Приглашение принято!
                        </p>

                        <div className={styles.summaryConfirmed}>

                            <div className={styles.summaryItem}>
                            <span>Приглашение отправлено:</span>
                            <strong>{choice.student?.name}</strong>
                            </div>

                            <div className={styles.summaryItem}>
                            <span>Тема:</span>
                            <strong>{choice.theme}</strong>
                            </div>

                            <div className={styles.summaryItem}>
                            <span>Ваша роль:</span>
                            <strong>{choice.role}</strong>
                            </div>

                        </div>
                        <button className={styles.buttonGreen}>Перейти к образцу диалога</button>
                    </div>
                </section>
            </div>
        </div>
    )
}