import styles from '../App.module.css'

interface ButtonProps {
  onClick: () => void
  children: string
  className?: string
}

const Button = ({ onClick, children, className = '' }: ButtonProps) => {
  return <button className={`${styles.button} ${className}`} onClick={onClick}>{children}</button>
}

export default Button