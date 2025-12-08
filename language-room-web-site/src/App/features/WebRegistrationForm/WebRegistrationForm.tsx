import { useState } from "react";
import styles from './WebRegistrationForm.module.scss'
import bgImage from "@assets/bgLogin.png";
import { RoleSwitcher } from '@ui/RoleSwitcher/RoleSwitcher'
import {InputReg} from '@ui/inputReg/InputReg'
import { Link, useNavigate } from "react-router-dom";
import { ButtonRegSocial } from "@ui/buttonRegSocial/buttonRegSocial";
import {ButtonReg} from '@ui/buttonReg/ButtonReg'
import { PiAtThin } from "react-icons/pi";
import { FaFacebookF } from "react-icons/fa";
import { FcGoogle } from "react-icons/fc";
import { register } from "../../../api/auth.api";

//add api patch!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

export function WebRegistrationForm() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        //middleName: "",
        email: "",
        password: "",
        //confirmPassword: "",
    });

    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const onChange = (field: string, value: string) => {
        setForm(prev => ({ ...prev, [field]: value }));
    };

    const onSubmit = async () => {
        try {
        setLoading(true);
        setError(null);

        await register(form);

        navigate("/login");
        } catch (e: any) {
        setError(
            e.response?.data?.message ||
            "Ошибка при регистрации"
        );
        } finally {
        setLoading(false);
        }
    };
    
    return (
    <div className={styles.bg}>
        <img className={styles.bgImage} src={bgImage} alt="background" />

        <div className={styles.formMain}>
            <div className={styles.formBg}>
                <div className={styles.formContainer}>
                    <h4 className='text-center font-bold text-[28px] mb-xl'>Зарегистрироваться</h4>
                    {error && (
                    <p className="text-red-500 text-center mb-3">
                        {error}
                    </p>
                    )}
                    <p className='text-[14px] text-[#737373] text-center mb-8'>Создайте свою учетную запись, чтобы начать обучение</p>

                    <div className={styles.roleSwitcher}>
                        <RoleSwitcher />
                    </div>

                    <div className={styles.holder}>
                        <p>Имя</p>
                        <p>Фамилия</p>
                        <span className={styles.grid}>
                            <p className='flex items-center gap-2'>Отчество <p className='text-[#737373]'>*если имеется</p></p>
                        </span>
                    </div>

                    <div className={styles.form}>
                        <InputReg type='text' placeholder='Александр' value={form.firstName} onChange={e => onChange("firstName", e.target.value)}/> 
                        <InputReg type='text' placeholder='Пушкин' value={form.lastName} onChange={e => onChange("lastName", e.target.value)}/> 
                        <span className={styles.grid}>
                            <InputReg type='text' placeholder='Сергеевич'/> 
                        </span>
                    </div>

                    <div className={styles.formGrid}>
                        <p>Адрес электронной почты</p>
                        <p>Пароль</p>
                        <InputReg type='text' placeholder='Почта'icon={<PiAtThin size={20} />} value={form.email} onChange={e => onChange("email", e.target.value)}/> 
                        <InputReg type='password' placeholder='Создайте надежный пароль' icon={<PiAtThin size={20} />} value={form.password} onChange={e => onChange("password", e.target.value)}/> 
                        <p>Подтвердите пароль</p>
                        <p></p>
                        <InputReg type='password' placeholder='Подтвердите свой пароль' icon={<PiAtThin size={20} />}/> 
                    </div>

                    <div className='flex justify-center items-center my-[25px]'>
                        <input className='mr-1'  type="checkbox"/>
                        <p className='text-[14px] text-[#737373]'>Я согласен с&nbsp;</p>
                        <a href="/terms" className={styles.policy}>
                        Условиями обслуживания
                        </a>
                        <pre className='text-[14px] text-[#737373]'> и </pre>
                        <a href="/terms" className={styles.policy}>
                            Политикой конфиденциальности
                        </a>
                    </div>
                    
                    <ButtonReg text={loading ? "Регистрация..." : "Зарегистрироваться"} onClick={onSubmit} />

                    <p className='text-center text-[14px] text-[#737373] my-[25px]'>Или зарегистрируйтесь с помощью</p>

                    <div className='flex justify-center gap-3 my-[15px]'>
                        <ButtonRegSocial icon={<FcGoogle size={26} />} />
                        <ButtonRegSocial icon={<FaFacebookF size={26} color="#1877F2" />} />
                    </div>

                    <div className='flex justify-center'>
                        <p className='text-center text-[14px] text-[#737373]'>У вас уже есть аккаунт?</p>
                        <Link className='text-[#667EEA] text-[14px] ' to="/login">&nbsp;Войти здесь</Link>
                    </div>
                </div>
            </div>
        </div>
    </div>
    );
}