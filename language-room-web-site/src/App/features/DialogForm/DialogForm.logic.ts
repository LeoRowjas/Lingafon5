export interface LearningMaterial {
  id: string;
  icon: string;
  label: string;
}

export const learningMaterials: LearningMaterial[] = [
  { id: 'materials', icon: '📚', label: 'Материалы для изучения' },
  { id: 'transcription', icon: '📝', label: 'Разметка и транскрипция перенесён' },
  { id: 'rating', icon: '⭐', label: 'Подсчёт и оценки' },
  { id: 'structure', icon: '🔍', label: 'Схема и структура' },
];