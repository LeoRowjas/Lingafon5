import { useState } from "react";
import styles from './WebLoginForm.module.scss'
import bgImage from "@assets/bgLogin.png";
import { InputReg } from '@ui/inputReg/InputReg'
import { ButtonRegSocial } from "@ui/buttonRegSocial/buttonRegSocial";
import {ButtonReg} from '@ui/buttonReg/ButtonReg'
import { RoleSwitcher } from '@ui/RoleSwitcher/RoleSwitcher'
import { PiAtThin } from "react-icons/pi";
import { FaFacebookF } from "react-icons/fa";
import { FcGoogle } from "react-icons/fc";
import { useNavigate } from "react-router-dom";
import { login } from "../../../api/auth.api";

export function WebLoginForm() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async () => {
        try {
            setLoading(true);
            setError(null);

            const data = await login({email, password});

            localStorage.setItem("token", data.token);

            navigate("/profile");
        } catch (e: any){
            const msg =
                e.response?.data?.message ||
                e.response?.data?.error ||
                "Ошибка входа";

            setError(msg);
        } finally {
            setLoading(false);
        }
    };

    return(
    <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />

        <div className={styles.form}>
            <div className='flex flex-col items-center mb-6'>
                <h3 className='text-2xl font-bold'>Вход в кабинет</h3>
                <p className='text-[14px] text-[#737373] mb-8'>Войдите, чтобы продолжить свое обучение</p>
                <div className={styles.roleSwitcher}>
                    <RoleSwitcher />
                </div>
            </div>
            <div className={styles.inner}>

                <p>Адрес электронной почты</p>
                <InputReg type="email"
                placeholder="Введите свой адрес электронной почты"
                icon={<PiAtThin size={20} />}
                value={email}
                onChange={(e) => setEmail(e.target.value)}/>

                <p>Пароль</p>

                <InputReg type="password"
                placeholder="Введите ваш пароль"
                icon={<PiAtThin size={20} />}
                value={password}
                onChange={(e) => setPassword(e.target.value)}/>

                <div>
                    <div>
                        <input type="checkbox" />
                        <p>Запомнить меня</p>
                    </div>
                    <a href="">Забыли пароль?</a>
                </div>

                <ButtonReg text={loading ? "Вход..." : "Войти"} onClick={handleLogin} />
                
                <p className='text-center'>Или продолжить с</p>
                
                <div className='flex gap-4'>
                <ButtonRegSocial icon={<FcGoogle size={26} />} />
                <ButtonRegSocial icon={<FaFacebookF color="#1877F2" />} />
                </div>
            </div>

                <div className={styles.wrapper}>
                    <p className={styles.row}>
                        У вас нет учетной записи?
                        <a href="/register" className={styles.link}>
                        Зарегистрируйтесь здесь
                        </a>
                    </p>
                </div>
        </div>
    </div>
    )
}