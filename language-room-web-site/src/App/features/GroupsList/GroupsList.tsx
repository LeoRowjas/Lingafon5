// features/GroupsList/GroupsList.tsx
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { BackButton } from '@ui/BackButton/BackButton'
import { ActionButton } from '@ui/ActionButton/ActionButton'
import { Modal } from '@ui/Modal/Modal'
import { Input } from '@ui/Input/Input'
import styles from './GroupsList.module.scss'
import bgImage from "@assets/bgLogin.png"

interface Group {
  id: string
  name: string
  studentsCount: number
}

const groupsMock: Group[] = [
  { id: '1', name: 'ГР-2024-01', studentsCount: 25 },
  { id: '2', name: 'ГР-2024-02', studentsCount: 23 },
  { id: '3', name: 'ГР-2024-03', studentsCount: 27 },
]

export function GroupsList() {
  const navigate = useNavigate()
  
  const [isAddModalOpen, setIsAddModalOpen] = useState(false)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [selectedGroup, setSelectedGroup] = useState<Group | null>(null)
  
  const [groupName, setGroupName] = useState('')
  const [studentsCount, setStudentsCount] = useState('')

  const handleGroupClick = (groupId: string) => {
    navigate(`/groups/${groupId}`)
  }

  const handleEdit = (group: Group, e: React.MouseEvent) => {
    e.stopPropagation()
    setSelectedGroup(group)
    setGroupName(group.name)
    setStudentsCount(String(group.studentsCount))
    setIsEditModalOpen(true)
  }

  const handleDelete = (group: Group, e: React.MouseEvent) => {
    e.stopPropagation()
    setSelectedGroup(group)
    setIsDeleteModalOpen(true)
  }

  const handleAddGroup = () => {
    console.log('Добавить группу:', groupName, studentsCount)
    setIsAddModalOpen(false)
    setGroupName('')
    setStudentsCount('')
  }

  const handleSaveEdit = () => {
    console.log('Сохранить группу:', groupName, studentsCount)
    setIsEditModalOpen(false)
  }

  const handleConfirmDelete = () => {
    console.log('Удалить группу:', selectedGroup)
    setIsDeleteModalOpen(false)
  }

  return (
    <div className={styles.bg}>
            <img className={styles.bgImage} src={bgImage} alt="background" />
    <div className={styles.container}>
      <div className={styles.formBlock}>
      <div className={styles.header}>
        <h1 className={styles.title}>Учебные группы</h1>
        <div className={styles.headerActions}>
          <BackButton label="Назад" to="/" />
          <ActionButton variant="blue" onClick={() => setIsAddModalOpen(true)}>
            + Добавить группу
          </ActionButton>
        </div>
      </div>

      <div className={styles.groupsGrid}>
        {groupsMock.map((group) => (
          <div key={group.id} className={styles.groupCard}>
            <div className={styles.groupInfo} onClick={() => handleGroupClick(group.id)}>
              <h3 className={styles.groupName}>{group.name}</h3>
              <p className={styles.studentsCount}>Учеников: {group.studentsCount}</p>
            </div>
            <div className={styles.groupActions}>
              <button 
                className={styles.editBtn}
                onClick={(e) => handleEdit(group, e)}
              >
                Редактировать
              </button>
              <button 
                className={styles.deleteBtn}
                onClick={(e) => handleDelete(group, e)}
              >
                Удалить
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Модалка добавления группы */}
      <Modal isOpen={isAddModalOpen} onClose={() => setIsAddModalOpen(false)}>
        <h2 className={styles.modalTitle}>Добавить новую группу</h2>
        <div className={styles.form}>
          <Input
            label="Название группы"
            value={groupName}
            onChange={setGroupName}
            placeholder="Например: ГР-2024-04"
          />
          <Input
            label="Количество учеников"
            value={studentsCount}
            onChange={setStudentsCount}
            placeholder="25"
          />
          <div className={styles.modalActions}>
            <ActionButton variant="blue" onClick={handleAddGroup}>
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

      {/* Модалка редактирования группы */}
      <Modal isOpen={isEditModalOpen} onClose={() => setIsEditModalOpen(false)}>
        <h2 className={styles.modalTitle}>Редактировать группу</h2>
        <div className={styles.form}>
          <Input
            label="Название группы"
            value={groupName}
            onChange={setGroupName}
            placeholder="Например: ГР-2024-04"
          />
          <Input
            label="Количество учеников"
            value={studentsCount}
            onChange={setStudentsCount}
            placeholder="25"
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

      {/* Модалка удаления группы */}
      <Modal isOpen={isDeleteModalOpen} onClose={() => setIsDeleteModalOpen(false)}>
        <h2 className={styles.modalTitle}>
          Вы уверены что хотите удалить группу "{selectedGroup?.name}"
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