import { useState } from 'react'
import { useAppDispatch } from '../redux/hooks'
import { ToDoTask, toggleTask, editTask, deleteTask } from '../redux/todoSlice'

import Button from './Button'
import styles from '../App.module.css'

type Props = {
    todo: ToDoTask;
};


const ToDoItem = ({ todo }: Props) => {

    const dispatch = useAppDispatch()
    const [isEditing, setIsEditing] = useState(false)
    const [editText, setEditText] = useState(todo.text)


    const handleToggleTask = () => {
        dispatch(toggleTask(todo.id))
    }

    const handleEditTask = () => {
        if (isEditing) {
            dispatch(editTask({
                ...todo,
                text: editText
            }))
            setIsEditing(false)
        }
        else {
            setIsEditing(true)
        }
    }

    const handleDeleteTask = () => {
        dispatch(deleteTask(todo.id))
    }

    const handleChangeText = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEditText(e.target.value)
    }

    return (
        <div className={styles.todoItem}>
            <input
                type='checkbox'
                className={styles.checkbox}
                checked={todo.isDone}
                onChange={handleToggleTask}
            />

            {isEditing ? (
                <input
                    type='text'
                    className={styles.todoInput}
                    value={editText}
                    onChange={handleChangeText}
                    autoFocus
                />
            ) : (
                <span className={`${styles.todoText} ${todo.isDone ? styles.todoDone : ''}`}>
                    {todo.text}
                </span>
            )}

            <div className={styles.buttonsContainer}>
                <Button
                    onClick={handleEditTask}
                    className={styles.editButton}>
                    {isEditing ? 'Сохранить' : 'Редактировать'}
                </Button>
                <Button
                    onClick={handleDeleteTask}
                    className={styles.deleteButton}>
                    Удалить
                </Button>
            </div>
        </div>
    )
}

export default ToDoItem