import styles from "./../SelectRoleForm/SelectRoleForm.module.scss";
import bgImage from "@assets/bgLogin.png";
import { useStudentChoice } from "@entities/ChatSelectionContext";
import clockImg from '@assets/clok.png'
import { sendChatInvite } from "@shared/mock/chatEvents.mock";
import { useNavigate, Link } from 'react-router-dom'
import { getInvites } from "@entities/invite/api/inviteApi";
import { useEffect } from "react";

export function LoadingInvitationsForm () {

    const { choice } = useStudentChoice();
    const navigate = useNavigate();

    useEffect(() => {
        if (!choice.student?.inviteToken) return;

        const interval = setInterval(async () => {
            try {
                const invites = await getInvites();

                const myInvite = invites.find(
                    (inv: any) => inv.token === choice.student?.inviteToken
                );

                if (myInvite && myInvite.isUsed === true) {
                clearInterval(interval);
                navigate("/dialog");
                }
            } catch (e) {
                console.error(e);
            }
        }, 3000);
        return () => clearInterval(interval);
    }, [choice.student, navigate]);

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
                        <img className={styles.clockImg} src={clockImg} alt="" />
                        <p className={styles.subtitleMain}>
                            Ожидание принятия приглашения
                        </p>

                        <div className={styles.summary}>

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
                        <p>Ожидаем ответа от студента...</p>
                        <button className={styles.buttonRed}><Link to='/profile'>Отмена</Link></button>
                        
                    </div>
                </section>
            </div>
        </div>
    )
}