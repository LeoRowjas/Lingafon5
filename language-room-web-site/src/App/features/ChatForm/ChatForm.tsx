import bgImage from "@assets/bgLogin.png"
import { InfoItem } from '@ui/InfoItem/InfoItem'
import { ProgressBar } from '@ui/ProgressBar/ProgressBar'
import styles from './ChatForm.module.scss'

import { dialogsMock, formatDuration, getStatusText, issuesMock, messagesMock, recommendationsMock } from './ChatForm.logic.ts'

import type { Dialog } from './ChatForm.logic.ts'



export function ChatForm() {
    const getInitial = (name: string) => name.charAt(0).toUpperCase();

    return(
        <div>
            <div className={styles.bg}>
                <img className={styles.bgImage} src={bgImage} alt="background" />
                <div className={styles.cardBg}>

                <h3 className={styles.textTitle}>Пройденный диалог</h3>

                {dialogsMock.map((dialog: Dialog) =>(
                    <div className={styles.meta} key={dialog.id}>
                    <span className={styles.status}>
                        {getStatusText("active")}
                    </span>
                    <span className={styles.date}>
                        {dialog.date}
                    </span>
                </div>
                ))}
                

                <div className={styles.block}>
                    <h2>Информация о диалоге</h2>
                    <div className={styles.gridCard}>
                        <InfoItem label='Тема диалога'>
                            Консультация по продукту
                        </InfoItem>

                        <InfoItem label='Роль в диалоге'>
                            Консультант
                        </InfoItem>

                        <InfoItem label="Длительность">
                            {formatDuration(34)}
                        </InfoItem>
                        
                        <InfoItem label="Процент совпадения">
                            <div className={styles.progressBar}>
                                <ProgressBar value={60} />
                            </div>
                        </InfoItem>
                        
                    </div>
                </div>

                <div className={styles.dialogBlock}>
                    <div className={styles.dialogHeader}>
                        <h3>Запись диалога</h3>
                        <div className={styles.timeInfo}>0:00 / 12:34</div>
                        <button className={styles.playBtn}>▶ Воспроизвести</button>
                    </div>
                    
                    <div className={styles.messagesWrapper}>
                        <div className={styles.messagesList}>
                            {messagesMock.map((msg) => (
                                <div key={msg.id} className={`${styles.message} ${styles[msg.sender]}`}>
                                    <div className={`${styles.avatar} ${styles[`avatar-${msg.sender}`]}`}>
                                        {getInitial(msg.name)}
                                    </div>
                                    <div className={styles.messageContent}>
                                        <div className={styles.messageText}>{msg.text}</div>
                                        <div className={styles.messageTime}>{msg.time}</div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>

                <div className={styles.issuesForm}>
                    <div className={styles.issuesBlock}>
                    <div className={styles.blockHeader}>
                        <h3>Исправления</h3>
                    </div>
                    
                    <div className={styles.issuesList}>
                        {issuesMock.map((issue) => (
                            <div key={issue.id} className={`${styles.issueItem} ${styles[`issue-${issue.type}`]}`}>
                                <div className={styles.issueContent}>
                                    <div className={styles.issueTitle}>{issue.title}</div>
                                    <div className={styles.issueTime}>Время: {issue.time}</div>
                                </div>
                                <button className={styles.expandBtn}>+</button>
                            </div>
                        ))}
                    </div>
                </div>

                <div className={styles.recommendationsBlock}>
                    <div className={styles.blockHeader}>
                        <h3>Рекомендации</h3>
                    </div>
                    
                    <div className={styles.recommendationsList}>
                        {recommendationsMock.map((rec) => (
                            <div key={rec.id} className={styles.recommendationItem}>
                                <div className={styles.recIcon}>{rec.id}</div>
                                <div className={styles.recContent}>
                                    <div className={styles.recTitle}>{rec.title}</div>
                                    <div className={styles.recDescription}>{rec.description}</div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
                </div>

                </div>
            </div>
        </div>
    )
}