import { createContext, useContext, useState } from "react";

/* ====== TYPES ====== */

export type Student = {
  id: string;
  name: string;
  inviteToken?: string;
};

export type StudentChoice = {
  theme?: string;
  role?: string;
  student?: Student;
};

type StudentChoiceContextType = {
  choice: StudentChoice;
  setTheme: (theme: string) => void;
  setRole: (role: string) => void;
  setStudent: (student: Student) => void;
  resetChoice: () => void;
};

/* ====== CONTEXT ====== */

const StudentChoiceContext = createContext<StudentChoiceContextType | null>(null);

/* ====== PROVIDER ====== */

export const StudentChoiceProvider = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  const [choice, setChoice] = useState<StudentChoice>({});

  const setTheme = (theme: string) => {
    setChoice(prev => ({
      ...prev,
      theme,
    }));
  };

  const setRole = (role: string) => {
    setChoice(prev => ({
      ...prev,
      role,
    }));
  };

  const setStudent = (student: Student) => {
    setChoice(prev => ({
      ...prev,
      student,
    }));
  };

  const resetChoice = () => {
    setChoice({});
  };

  return (
    <StudentChoiceContext.Provider
      value={{
        choice,
        setTheme,
        setRole,
        setStudent,
        resetChoice,
      }}
    >
      {children}
    </StudentChoiceContext.Provider>
  );
};

/* ====== HOOK ====== */

export const useStudentChoice = () => {
  const context = useContext(StudentChoiceContext);

  if (!context) {
    throw new Error(
      "useStudentChoice must be used within a StudentChoiceProvider"
    );
  }

  return context;
};
