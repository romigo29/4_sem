import { useState } from 'react'
import './App.css'
import Display from './Display'
import Button from './Button'
import History from './History'

function App() {

  const [displayValue, setDisplayValue] = useState<string>('0')
  const [waitingForOperand, setWaitingForOperand] = useState<boolean>(false)
  const [pendingOperator, setPendingOperator] = useState<string>('')
  const [storedValue, setStoredValue] = useState<number | null>(null)
  const [history, setHistory] = useState<Array<string>>([])

  const formatHistoryItem = (a: number, op: string, b: number, result: number | string): string => {
    return `${a} ${op} ${b} = ${result}`;
  };

  const handleError = (message: string) => {
    setHistory(prev => [...prev, message]);
  }

  const clearDisplay = () => {
    setDisplayValue('0');
    setWaitingForOperand(false);
    setPendingOperator('');
    setStoredValue(null);
  }

  const clearHistory = () => {
    setHistory([]);
  }

  const removeLastSymbol = () => {
    if (displayValue.length > 1) {
      setDisplayValue(displayValue.slice(0, -1))
    } else {
      setDisplayValue('0')
    }
  }

  const addNumber = (num: string) => {

    if (displayValue.length > 10) {
      return;
    }

    let newDisplayValue = displayValue;

    if (waitingForOperand) {
      newDisplayValue = '';
      setWaitingForOperand(false);
    }

    if (newDisplayValue === '0' && num !== '0') {
      setDisplayValue(num);
    } else if (newDisplayValue !== '0') {
      setDisplayValue(newDisplayValue + num);
    }
  }

  const addOperation = (operator: string) => {
    const operand = parseFloat(displayValue);

    if (waitingForOperand) {
      handleError("Введите второе число");
      return;
    }

    if (storedValue === null) {
      setStoredValue(operand);
    } else if (pendingOperator) {
      const result: number | null = calculate(operand, pendingOperator);

      setHistory(prev => {
        return [...prev, formatHistoryItem(storedValue!, pendingOperator, operand, result!)];
      });
      setStoredValue(result);
      result === null ? setDisplayValue("Ошибка: деление на 0") : setDisplayValue(String(result));
    }

    setWaitingForOperand(true);
    setPendingOperator(operator);
  }

  const handleCalculation = () => {
    const operand = parseFloat(displayValue);
    if (pendingOperator && storedValue != null) {
      let result: number | null | string = calculate(operand, pendingOperator)

      if (result == null) {
        setDisplayValue('0');
        result = "Ошибка: деление на 0. Сброс";
      }
      else {
        setDisplayValue(String(result));
      }

      setHistory(prev => {
        return [...prev, formatHistoryItem(storedValue!, pendingOperator, operand, result!)];
      });

      setStoredValue(result === "Ошибка: деление на 0. Сброс" ? null : parseFloat(String(result)));
      setPendingOperator('');
    }
    else {
      setStoredValue(operand);
    }

    setWaitingForOperand(true);
  }

  const calculate = (rightOperand: number, operator: string) => {

    let result: number = 0;

    switch (operator) {
      case '+': {
        result = storedValue! + rightOperand;
        break;
      }
      case ('-'): {
        result = storedValue! - rightOperand;
        break;
      }
      case ('*'): {
        result = storedValue! * rightOperand;
        break;
      }
      case ('/'): {
        if (rightOperand == 0) return null;
        result = storedValue! / rightOperand;
        break;
      }
    }

    return result
  }

  const toggleSign = () => {
    setDisplayValue(prev => {
      if (prev === '0') return '0';
      return prev.startsWith('-') ? prev.substring(1) : '-' + prev;
    });
  }

  const doteSign = () => {
    setDisplayValue(prev => {
      return prev.includes('.') ? prev : prev + '.'
    })
  }

  const handleKeyPress = (key: string) => {
    switch (key) {
      case '+':
      case '-':
      case '*':
      case '/':
        addOperation(key);
        break;
      case '=':
      case 'Enter':
        handleCalculation();
        break;
      case 'Escape':
        clearDisplay();
        break;
      case 'Backspace':
        removeLastSymbol();
        break;
      default:
        if (/^[0-9.]$/.test(key)) {
          addNumber(key);
        }
    }
  };

  return (
    <>
      <div className='calculator'>
        <div className='history-container'>
          <History history={history} />
        </div>
        <div className='display'>
          <Display
            displayValue={displayValue}
            setDisplayValue={setDisplayValue}
            onKeyPress={handleKeyPress}
            waitingForOperand={waitingForOperand}
            pendingOperator={pendingOperator}
            onError={handleError}
            setWaitingForOperand={setWaitingForOperand}
            setPendingOperator={setPendingOperator}
          />
        </div>

        <div className='buttons'>
          <div className='buttonLine'>
            <Button title="C" onClick={clearDisplay} />
            <Button title="CH" onClick={clearHistory} />
            <Button title="⌫" onClick={removeLastSymbol} />
            <Button title="/" onClick={() => addOperation("/")} />
          </div>
          <div className='buttonLine'>
            <Button title="7" onClick={() => addNumber("7")} />
            <Button title="8" onClick={() => addNumber("8")} />
            <Button title="9" onClick={() => addNumber("9")} />
            <Button title="*" onClick={() => addOperation("*")} />
          </div>
          <div className='buttonLine'>
            <Button title="4" onClick={() => addNumber("4")} />
            <Button title="5" onClick={() => addNumber("5")} />
            <Button title="6" onClick={() => addNumber("6")} />
            <Button title="-" onClick={() => addOperation("-")} />
          </div>
          <div className='buttonLine'>
            <Button title="1" onClick={() => addNumber("1")} />
            <Button title="2" onClick={() => addNumber("2")} />
            <Button title="3" onClick={() => addNumber("3")} />
            <Button title="+" onClick={() => addOperation("+")} />
          </div>
          <div className='buttonLine'>
            <Button title="+/-" onClick={toggleSign} />
            <Button title="0" onClick={() => addNumber("0")} />
            <Button title="." onClick={doteSign} />
            <Button title="=" onClick={handleCalculation} />
          </div>
        </div>
      </div>
    </>
  )
}

export default App
