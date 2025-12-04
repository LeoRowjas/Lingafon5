import "./ButtonReg.module.scss"

interface ButtonRegProps {
    text: string
    onClick: () => void
}


export function ButtonReg({ text, onClick }: ButtonRegProps) {
    return(
        <div>
            <button onClick={onClick}>{text}</button>
        </div>
    )
}