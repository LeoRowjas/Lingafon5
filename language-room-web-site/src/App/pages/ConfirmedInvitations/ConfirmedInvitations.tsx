import { ConfirmedInvitationsForm } from '@features/ConfirmedInvitationsForm/ConfirmedInvitationsForm'
import { useStudentChoice } from "@entities/ChatSelectionContext";
import { useNavigate } from "react-router-dom";
import { useState } from "react";


export function ConfirmedInvitations() {

    const { choice, resetChoice } = useStudentChoice();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
    try {
      setLoading(true);

      // ⬇️ пример запроса
      const response = await fetch("/api/create-chat", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          theme: choice.theme,
          role: choice.role,
          studentId: choice.student?.id,
        }),
      });

      if (!response.ok) {
        throw new Error("Ошибка при создании диалога");
      }

      const data = await response.json();

      navigate("/confirm-in", {
        replace: true,
        state: {
          chatId: data.chatId,
        },
      });

      // (опционально) очистка
      resetChoice();
    } catch (error) {
      console.error(error);
      alert("Ошибка. Попробуйте ещё раз");
    } finally {
      setLoading(false);
    }
  };

    return(
        <div>
            <ConfirmedInvitationsForm/>
        </div>
    )
}