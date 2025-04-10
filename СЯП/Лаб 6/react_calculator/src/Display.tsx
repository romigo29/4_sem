import './Display.css'
import {
    ERROR_OPERAND1,
    ERROR_OPERAND2,
    ERROR_OPERATOR,
    MAX_DISPLAY_LENGTH,
    ERROR_INVALID_NUMBER_FORMAT,
} from './constants'

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
    waitingForOperand,
    pendingOperator,
    onError,
    setWaitingForOperand,
    setPendingOperator
}: DisplayProps) {

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = e.target.value;

        if (newValue.length > MAX_DISPLAY_LENGTH) {
            return;
        }

        if (newValue.includes('e')) {
            const regex = /^-?\d+(\.\d+)?e[+-]\d+$/;
            if (!regex.test(newValue)) {
                onError(ERROR_INVALID_NUMBER_FORMAT);
                return;
            }
        }

        if (newValue.split('.').length > 2) {
            e.preventDefault();
            return;
        }

        if (newValue.includes('-')) {

            if (newValue.indexOf('-') != 0 ||
                newValue.split('-').length > 2 ||
                newValue.indexOf('0') == 1) {
                e.preventDefault();
                return;
            }
        }

        if (newValue.startsWith('0') && newValue.length > 1 && !newValue.startsWith('0.')) {
            setDisplayValue(newValue.slice(1));
        } else {
            setDisplayValue(newValue);
        }

        if (waitingForOperand) {
            setWaitingForOperand(false);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const key = e.key;

        if (displayValue.includes('e') && displayValue.length > 1) {
            if (!['Backspace', 'Delete', 'Escape', 'ArrowLeft', 'ArrowRight'].includes(key)) {
                e.preventDefault();
                onError(ERROR_INVALID_NUMBER_FORMAT);
                return;
            }
        }

        if (/^[0-9.]$/.test(key)) {
            if (waitingForOperand) {
                setWaitingForOperand(false);
                setDisplayValue('');
            }
            return;
        }

        if (key == '-') {
            if (displayValue == '0' && !waitingForOperand) {
                setDisplayValue('-');
                return;
            }
            if (waitingForOperand) {
                setWaitingForOperand(false);
                setDisplayValue('-');
                return;
            }
        }

        if (!['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape', 'ArrowLeft', 'ArrowRight'].includes(key)) {
            e.preventDefault();
        }

        if (['+', '-', '*', '/', '=', 'Enter', 'Backspace', 'Escape'].includes(key)) {
            e.preventDefault();

            if (key === '=' || key === 'Enter') {
                if (!pendingOperator) {
                    onError(ERROR_OPERATOR);
                    setDisplayValue('0');
                    return;
                }
                if (waitingForOperand) {
                    onError(ERROR_OPERAND2);
                    setDisplayValue('0');
                    return;
                }
            } else if (['+', '-', '*', '/'].includes(key)) {
                if ((displayValue == '0' || displayValue == '-') && !waitingForOperand) {
                    onError(ERROR_OPERAND1);
                    setDisplayValue('0');
                    return;
                }

                if (waitingForOperand) {
                    setPendingOperator(key);
                    return;
                }

                setPendingOperator(key);
                setWaitingForOperand(true);
            }

            onKeyPress(key);
        }
    };


    return (
        <input
            type="text"
            className="inputValue"
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            value={displayValue}
        />
    );
}

export default Display