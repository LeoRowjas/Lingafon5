import React from "react";
import styles from "./ButtonRegSocial.module.scss";

type ButtonRegSocialProps = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  icon: React.ReactNode;
};

export const ButtonRegSocial: React.FC<ButtonRegSocialProps> = ({ icon, ...props }) => {
  return (
    <button className={styles.button} {...props}>
      <span className={styles.icon}>{icon}</span>
    </button>
  );
};
