import React from "react";
import styles from "./InputReg.module.scss";

type InputRegProps = React.InputHTMLAttributes<HTMLInputElement> & {
  icon?: React.ReactNode;
};

export const InputReg: React.FC<InputRegProps> = ({ icon, ...props }) => {
  return (
    <div className={styles.wrapper}>
      <input className={styles.input} {...props} />
      {icon && <span className={styles.icon}>{icon}</span>}
    </div>
  );
};
