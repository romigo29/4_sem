import React, { useState } from 'react'

const Button = ({ title, onClick, disabled = false }) => {
  return (
    <button onClick={onClick} disabled={disabled}>
      {title}
    </button>
  );
}

const Counter = () => {

  const [count, setCount] = useState(0)

  const handleIncrease = () => {
    setCount(count + 1)
  }

  const handleDecrease = () => {
    setCount(count - 1)
  }

  const handleReset = () => {
    setCount(0)
  }

  return (
    <>
      <div className="counter-display">
        Текущее значение: {count}
      </div>
      <div className="card">
        <Button
          title="Увеличить"
          onClick={handleIncrease}
          disabled={count >= 5}
        />

        <Button
          title="Уменьшить"
          onClick={handleDecrease}
          disabled={count <= -5}
        />

        <Button
          title="Сбросить"
          onClick={handleReset}
          disabled={count === 0}
        />
      </div>
    </>
  )
}

export { Counter, Button };