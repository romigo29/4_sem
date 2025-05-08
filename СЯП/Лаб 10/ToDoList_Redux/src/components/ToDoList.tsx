import { useState} from 'react'
import { useAppSelector, useAppDispatch } from '../redux/hooks'
import Button from './Button'
import { addTask} from '../redux/todoSlice'
import ToDoItem from './ToDoItem'
import styles from '../App.module.css'

const ToDoList = () => {
    const todoList = useAppSelector(state => state.todos)
    const dispatch = useAppDispatch()
    const [text, setText] = useState('')

    const handleChangeTask = (e: React.ChangeEvent<HTMLInputElement>) => {
        setText(e.target.value)
    }

    const handleAddTask = () => {
        if (text.trim() == '') return

        dispatch(addTask(text));
        setText('')
    }

    return (
        <div className={styles.todoContainer}>
            <h1 className={styles.heading}>To-Do List</h1>
            <div className={styles.inputContainer}>
                <input
                    className={styles.inputTasks}
                    type='text'
                    placeholder='Добавить задачу'
                    value={text}
                    onChange={(e) => handleChangeTask(e)}
                    autoFocus
                />
                <Button onClick={handleAddTask}>Добавить</Button>
            </div>
            <div className={styles.taskList}>
                {todoList.map(todo => (
                    <ToDoItem key={todo.id} todo={todo} />
                ))}
            </div>
        </div>
    )
}

export default ToDoList
