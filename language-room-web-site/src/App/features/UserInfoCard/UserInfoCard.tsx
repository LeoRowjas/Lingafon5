import {UserCard} from '@ui/UserCard/UserCard'
import styles from "./UserInfoCard.module.scss";

export const UserInfoCard = () => {
  return (
    <div className={styles.form}>
      <div className='grid grid-cols-3'>
        <UserCard/>
        <div className='col-span-2 ml-8 mt-14'>
          <h3 className='text-[70px] font-bold'>Личный кабинет</h3>
          <div className='flex items-center'>
            <h4 className='text-[36px] font-semibold mr-3'>Приглашение к диалогу:</h4>
            <button className={styles.button}>Отправить приглашение</button>
          </div>
        </div>
      </div>
    </div>
  );
};
