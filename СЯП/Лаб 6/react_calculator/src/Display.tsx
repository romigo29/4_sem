// import { useState } from "react"
import { useRef, useEffect } from 'react'
import './Display.css'

type DisplayProps = {
    displayValue: string,
    setDisplayValue: (value: string) => void,
    onKeyPress: (key: string) => void,
    cursorPosition?: number
}

function Display({ displayValue, setDisplayValue, onKeyPress, cursorPosition }: DisplayProps) {
    const inputRef = useRef<HTMLInputElement>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const currentValue = displayValue;
        const newValue = e.target.value;
        if (currentValue === '0' && newValue !== '0') {
            setDisplayValue(newValue);
        }
        else {
            setDisplayValue(currentValue + newValue);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const key = e.key;

        // Разрешаем ввод цифр, точки и минуса
        if (/^[0-9.]$/.test(key) || key === '-') {
            return;
        }

        // Предотвращаем ввод других символов
        if (!['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape'].includes(key)) {
            e.preventDefault();
        }

        // Обрабатываем специальные клавиши
        if (['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape'].includes(key)) {
            e.preventDefault();
            onKeyPress(key);
        }
    };

    useEffect(() => {
        if (cursorPosition !== undefined && inputRef.current) {
            inputRef.current.focus();
            inputRef.current.setSelectionRange(cursorPosition, cursorPosition);
        }
    }, [cursorPosition, displayValue]);

    return (
        <input
            ref={inputRef}
            type="text"
            className="inputValue"
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            value={displayValue}
        />
    );
}

export default Display