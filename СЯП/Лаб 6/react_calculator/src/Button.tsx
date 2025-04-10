import './Button.css'

type ButtonProps = {
    title: string;
    onClick: () => void;
}

function Button({ title, onClick }: ButtonProps) {

    return (
        <button onClick={onClick}>
            {title}
        </button>
    );

}

export default Button