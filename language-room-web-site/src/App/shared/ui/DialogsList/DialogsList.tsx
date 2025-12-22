import { Link } from "react-router-dom";
import styles from "./DialogsList.module.scss";
import {InfoItem} from '@ui/InfoItem/InfoItem'
import {ProgressBar} from '@ui/ProgressBar/ProgressBar'

import {
  dialogsMock,
  formatDuration,
  getStatusText,
} from "./dialogs.logic";

import type { Dialog } from "./dialogs.logic";


/* =========================
   MAIN UI
========================= */

export const DialogsList = () => {
  return (
    <div className={styles.list}>
      {dialogsMock.map((dialog: Dialog) => (
        <Link
          key={dialog.id}
          to={`/chat/${dialog.id}`}
          className={styles.card}
        >
          <div className={styles.content}>
            <h3 className={styles.cardTitle}>
              {dialog.title}
            </h3>

            <div className={styles.meta}>
              <span className={styles.status}>
                {getStatusText(dialog.status)}
              </span>
              <span className={styles.date}>
                {dialog.date}
              </span>
            </div>

            <div className={styles.info}>
              <InfoItem label="Роль в диалоге">
                {dialog.role}
              </InfoItem>

              <InfoItem label="Длительность">
                {formatDuration(dialog.durationSeconds)}
              </InfoItem>

              <InfoItem label="Процент совпадения">
                <ProgressBar value={dialog.matchPercent} />
              </InfoItem>
              
            </div>
          </div>

          <div className={styles.avatar}>
            {dialog.clientName?.[0]}
          </div>
        </Link>
      ))}
    </div>
  );
};
