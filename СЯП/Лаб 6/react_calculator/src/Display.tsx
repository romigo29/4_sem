// import { useState } from "react"
import { useRef, useEffect, useState } from 'react'
import './Display.css'

type DisplayProps = {
    displayValue: string,
    setDisplayValue: (value: string) => void,
    onKeyPress: (key: string) => void,
    cursorPosition?: number,
    waitingForOperand: boolean,
    pendingOperator: string,
    onError: (message: string) => void,
    setWaitingForOperand: (value: boolean) => void,
    setPendingOperator: (value: string) => void
}

function Display({
    displayValue,
    setDisplayValue,
    onKeyPress,
    cursorPosition,
    waitingForOperand,
    pendingOperator,
    onError,
    setWaitingForOperand,
    setPendingOperator
}: DisplayProps) {
    const inputRef = useRef<HTMLInputElement>(null);
    const [currentCursorPosition, setCurrentCursorPosition] = useState<number>(0);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = e.target.value;

        // Проверяем на ввод нескольких точек и длину 
        if (newValue.split('.').length > 2 ||
            newValue.length > 10) {
            e.preventDefault();
            return;
        }

        // Проверяем минус как знак отрицательного числа
        if (newValue.includes('-')) {
            // Минус может быть только в начале числа
            if (newValue.indexOf('-') !== 0) {
                e.preventDefault();
                return;
            }
            // Нельзя вводить несколько минусов
            if (newValue.split('-').length > 2) {
                e.preventDefault();
                return;
            }
        }

        // Убираем ведущий ноль
        if (newValue.startsWith('0') && newValue.length > 1 && !newValue.startsWith('0.')) {
            setDisplayValue(newValue.slice(1));
        } else {
            setDisplayValue(newValue);
        }

        // Сохраняем текущую позицию курсора
        if (inputRef.current) {
            setCurrentCursorPosition(inputRef.current.selectionStart || 0);
        }

        // Если вводится число после операции, сбрасываем флаг ожидания
        if (waitingForOperand) {
            setWaitingForOperand(false);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const key = e.key;



        // Обработка стрелок для перемещения курсора
        if (key === 'ArrowLeft' || key === 'ArrowRight') {
            if (inputRef.current) {
                const currentPos = inputRef.current.selectionStart || 0;
                const newPos = key === 'ArrowLeft'
                    ? Math.max(0, currentPos - 1)
                    : Math.min(displayValue.length, currentPos);

                setCurrentCursorPosition(newPos);
            }
            return;
        }

        // Разрешаем ввод цифр и точки
        if (/^[0-9.]$/.test(key)) {
            if (waitingForOperand) {
                setWaitingForOperand(false);
                setDisplayValue('');
            }
            return;
        }

        // Обрабатываем минус как знак отрицательного числа
        if (key === '-') {
            // Минус в начале первого числа
            if (displayValue === '0' && !waitingForOperand) {
                setDisplayValue('-');
                return;
            }
            // Минус в начале второго числа
            if (waitingForOperand) {
                setWaitingForOperand(false);
                setDisplayValue('-');
                return;
            }
        }

        // Предотвращаем ввод других символов
        if (!['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape', 'ArrowLeft', 'ArrowRight'].includes(key)) {
            e.preventDefault();
        }

        // Обрабатываем специальные клавиши
        if (['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape'].includes(key)) {
            e.preventDefault();

            // Проверяем последовательность ввода
            if (key === '=' || key === 'Enter') {
                if (!pendingOperator) {
                    onError('Ошибка: ожидается ввод операции');
                    setDisplayValue('0');
                    return;
                }
                if (waitingForOperand) {
                    onError('Ошибка: ожидается ввод второго числа');
                    setDisplayValue('0');
                    return;
                }
            } else if (['+', '-', '*', '/'].includes(key)) {
                // Если текущее значение это '0' или '-', то пользователь не ввел первое число
                if ((displayValue === '0' || displayValue === '-') && !waitingForOperand) {
                    onError('Ошибка: ожидается ввод первого числа');
                    setDisplayValue('0');
                    return;
                }

                // Если ожидаем второе число, но пользователь ввел другой оператор,
                // то заменяем текущий оператор на новый
                if (waitingForOperand) {
                    setPendingOperator(key);
                    return;
                }

                // Устанавливаем новый оператор
                setPendingOperator(key);
                setWaitingForOperand(true);
            }

            onKeyPress(key);
        }
    };

    // Обработка клика мышью для установки позиции курсора
    const handleClick = () => {
        if (inputRef.current) {
            setCurrentCursorPosition(inputRef.current.selectionStart || 0);
        }
    };

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
            inputRef.current.setSelectionRange(currentCursorPosition, currentCursorPosition);
        }
    }, [currentCursorPosition, displayValue]);

    useEffect(() => {
        if (cursorPosition !== undefined && inputRef.current) {
            inputRef.current.focus();
            inputRef.current.setSelectionRange(cursorPosition, cursorPosition);
            setCurrentCursorPosition(cursorPosition);
        }
    }, [cursorPosition]);

    return (
        <input
            ref={inputRef}
            type="text"
            className="inputValue"
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            onClick={handleClick}
            value={displayValue}
        />
    );
}

export default Display