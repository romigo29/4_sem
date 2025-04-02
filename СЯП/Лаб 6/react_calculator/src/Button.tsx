import './Button.css'
import "react"

function Button({ title, onClick }: { title: string, onClick: () => void }) {

    return (
        <button onClick={onClick}>
            {title}
        </button>
    );

}

export default Button