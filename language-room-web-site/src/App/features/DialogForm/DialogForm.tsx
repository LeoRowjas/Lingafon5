import styles from "./DialogForm.module.scss";
import bgImage from "@assets/bgLogin.png";
import { useState } from "react";
import {learningMaterials} from './DialogForm.logic'

export function DialogForm () {

    const [selectedMaterial, setSelectedMaterial] = useState<string | null>(null);
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

                        <div className={styles.lineGreen} />

                        <div className={styles.step}>
                        <span className={styles.stepActive}>5</span>
                        <span className={styles.stepText}>Диалог</span>
                        </div>
                    </div>

                    
                    <div>
                        <h2>Образец диалога</h2>

                        <div className={styles.blocksGrid}>
                        
                        <div className={styles.materialsBlock}>
                            <div className={styles.blockHeader}>
                                <h3>Материалы для изучения</h3>
                            </div>
                            
                            <div className={styles.materialsList}>
                                {learningMaterials.map((material) => (
                                    <button 
                                        key={material.id} 
                                        className={`${styles.materialItem} ${selectedMaterial === material.id ? styles.active : ''}`}
                                        onClick={() => setSelectedMaterial(material.id)}
                                    >
                                        <span className={styles.materialIcon}>{material.icon}</span>
                                        <span className={styles.materialLabel}>{material.label}</span>
                                    </button>
                                ))}
                            </div>
                        </div>


                        <div className={styles.infoBlock}>
                            <div className={styles.blockHeader}>
                                <h3>Информация о диалоге</h3>
                            </div>
                            
                            <div className={styles.infoList}>
                                <div className={styles.infoItem}>
                                    <span className={styles.infoLabel}>Тема</span>
                                    <span className={styles.infoValue}>Консультация по продукту</span>
                                </div>
                                <div className={styles.infoItem}>
                                    <span className={styles.infoLabel}>Ваша роль</span>
                                    <span className={styles.infoValue}>Консультант</span>
                                </div>
                                <div className={styles.infoItem}>
                                    <span className={styles.infoLabel}>Параметры</span>
                                    <span className={styles.infoValue}>Анна Петрова</span>
                                </div>
                                <div className={styles.infoItem}>
                                    <span className={styles.infoLabel}>Длительность</span>
                                    <span className={styles.infoValue}>~10 минут</span>
                                </div>
                            </div>
                        </div>

                    </div>
                    </div>
                    

                    <div className={styles.footer}>
                        <button className={styles.nextBtn}>Готов</button>
                    </div>

                </section>

            </div>
        </div>
    )
}