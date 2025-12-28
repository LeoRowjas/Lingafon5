// features/GroupDetails/GroupDetails.tsx
import bgImage from "@assets/bgLogin.png"
import { ActionButton } from '@ui/ActionButton/ActionButton'
import { BackButton } from '@ui/BackButton/BackButton'
import { Input } from '@ui/Input/Input'
import { Modal } from '@ui/Modal/Modal'
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import styles from './GroupDetails.module.scss'

interface Student {
  id: string
  name: string
  averageScore: number
}

const studentsMock: Student[] = [
  { id: '1', name: 'Иванов Иван Иванович', averageScore: 25 },
  { id: '2', name: 'Петров Петр Петрович', averageScore: 23 },
]

export function GroupDetails() {
  const { groupId } = useParams<{ groupId: string }>()
  
  const [isAddModalOpen, setIsAddModalOpen] = useState(false)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null)
  
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [middleName, setMiddleName] = useState('')

  const handleEdit = (student: Student) => {
    setSelectedStudent(student)
    const [last, first, middle] = student.name.split(' ')
    setLastName(last)
    setFirstName(first)
    setMiddleName(middle)
    setIsEditModalOpen(true)
  }

  const handleDelete = (student: Student) => {
    setSelectedStudent(student)
    setIsDeleteModalOpen(true)
  }

  const handleAddStudent = () => {
    console.log('Добавить:', firstName, lastName, middleName)
    setIsAddModalOpen(false)
    setFirstName('')
    setLastName('')
    setMiddleName('')
  }

  const handleSaveEdit = () => {
    console.log('Сохранить:', firstName, lastName, middleName)
    setIsEditModalOpen(false)
  }

  const handleConfirmDelete = () => {
    console.log('Удалить:', selectedStudent)
    setIsDeleteModalOpen(false)
  }

  return (
    <div className={styles.bg}>
      <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.container}>
      <div className={styles.formMain}>
      <div className={styles.header}>
        <h1 className={styles.title}>Учебная группа ГР-2024-01</h1>
        <div className={styles.headerActions}>
          <BackButton label="Назад к группам" to="/groups-list" />
          <ActionButton variant="blue" onClick={() => setIsAddModalOpen(true)}>
            + Добавить ученика
          </ActionButton>
        </div>
      </div>

      <div className={styles.studentsGrid}>
        {studentsMock.map((student) => (
          <Link to="/student-profile">
          <div key={student.id} className={styles.studentCard}>
            <div className={styles.studentInfo}>
              <h3 className={styles.studentName}>{student.name}</h3>
              <p className={styles.score}>Средний балл: {student.averageScore}</p>
            </div>
            <div className={styles.studentActions}>
              <button 
                className={styles.editBtn}
                onClick={() => handleEdit(student)}
              >
                Редактировать
              </button>
              <button 
                className={styles.deleteBtn}
                onClick={() => handleDelete(student)}
              >
                Удалить
              </button>
            </div>
          </div>
          </Link>
        ))}
      </div>

      {/* Модалка добавления ученика */}
      <Modal isOpen={isAddModalOpen} onClose={() => setIsAddModalOpen(false)}>
        <h2 className={styles.modalTitle}>Добавить нового ученика</h2>
        <div className={styles.form}>
          <div className={styles.formRow}>
            <Input
              label="Имя"
              value={firstName}
              onChange={setFirstName}
              placeholder="Например: Иван"
            />
            <Input
              label="Фамилия"
              value={lastName}
              onChange={setLastName}
              placeholder="Например: Иванов"
            />
          </div>
          <Input
            label="Отчество"
            value={middleName}
            onChange={setMiddleName}
            placeholder="Например: Иванович"
          />
          <div className={styles.modalActions}>
            <ActionButton variant="blue" onClick={handleAddStudent}>
              Добавить
            </ActionButton>
            <button 
              className={styles.cancelBtn}
              onClick={() => setIsAddModalOpen(false)}
            >
              Отмена
            </button>
          </div>
        </div>
      </Modal>

      {/* Модалка редактирования */}
      <Modal isOpen={isEditModalOpen} onClose={() => setIsEditModalOpen(false)}>
        <h2 className={styles.modalTitle}>Редактировать данные ученика</h2>
        <div className={styles.form}>
          <div className={styles.formRow}>
            <Input
              label="Имя"
              value={firstName}
              onChange={setFirstName}
              placeholder="Например: Иван"
            />
            <Input
              label="Фамилия"
              value={lastName}
              onChange={setLastName}
              placeholder="Например: Иванов"
            />
          </div>
          <Input
            label="Отчество"
            value={middleName}
            onChange={setMiddleName}
            placeholder="Например: Иванович"
          />
          <div className={styles.modalActions}>
            <ActionButton variant="blue" onClick={handleSaveEdit}>
              Сохранить
            </ActionButton>
            <button 
              className={styles.cancelBtn}
              onClick={() => setIsEditModalOpen(false)}
            >
              Отмена
            </button>
          </div>
        </div>
      </Modal>

      {/* Модалка удаления */}
      <Modal isOpen={isDeleteModalOpen} onClose={() => setIsDeleteModalOpen(false)}>
        <h2 className={styles.modalTitle}>
          Вы уверены что хотите удалить данные ученика "{selectedStudent?.name}"?
        </h2>
        <div className={styles.modalActions}>
          <ActionButton variant="blue" onClick={handleConfirmDelete}>
            Подтвердить
          </ActionButton>
          <button 
            className={styles.cancelBtn}
            onClick={() => setIsDeleteModalOpen(false)}
          >
            Отменить
          </button>
        </div>
      </Modal>
    </div>
    </div>
    </div>
  )
}