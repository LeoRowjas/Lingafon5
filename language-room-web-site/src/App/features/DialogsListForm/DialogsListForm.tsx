import styles from './DialogsListForm.module.scss'
import {DialogsList} from '@ui/DialogsList/DialogsList'
import bgImage from "@assets/bgLogin.png";
import {Link} from 'react-router-dom'

export function DialogsListForm() {
    return(
        <div>
            <div className={styles.bg}>
            <img className={styles.bgImage} src={bgImage} alt="background" />
                <div className={styles.form}>

                    <h2 className={styles.formText}>Статистика заданий</h2>
                    <Link className={styles.link} to='/profile'>← Вернуться в личный кабинет</Link>

                    <div className={styles.cardBg}>
                    <h3 className={styles.cardText}>Список диалогов</h3>
                    <div className={styles.cardForm}>
                        <DialogsList/>
                    </div>
                </div>
                </div>
            </div>
        </div>
    )
}