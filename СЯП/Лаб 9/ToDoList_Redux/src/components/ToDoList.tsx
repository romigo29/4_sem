import { useSelector, useDispatch } from 'react-redux'
import { useState } from 'react'
import { TODOS } from '../redux/types'
import { AppDispatch } from '../redux/store'
import Button from './Button'
import { AddTask } from '../redux/actions'
import ToDoItem from './ToDoItem'
import styles from '../App.module.css'

const ToDoList = () => {

    const todoList = useSelector((list: TODOS) => list)
    const dispatch = useDispatch<AppDispatch>()
    const [text, setText] = useState('')

    const handleChangeTask = (e: React.ChangeEvent<HTMLInputElement>) => {
        setText(e.target.value)
    }

    const handleAddTask = () => {
        if (text.trim() == '') return

        dispatch(AddTask(text));
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
