interface ButtonProps {
  onClick: () => void
  children: string
}

const Button = ({ onClick, children } : ButtonProps) => {
  return <button onClick={onClick}>{children}</button>
}

export default Button
