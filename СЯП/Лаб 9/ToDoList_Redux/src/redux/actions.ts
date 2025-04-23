import { ADD_TODO, TOGGLE_TODO, EDIT_TODO, DELETE_TODO, CRUD, ToDoTask } from "./types"

export const AddTask = (text: string): CRUD => ({
    type: ADD_TODO,
    task:
    {
        text,
        isDone: false
    },

})

export const ToggleTask = (currentID: number): CRUD => ({
    type: TOGGLE_TODO,
    id: currentID,
})

export const EditTask = (editTask: ToDoTask): CRUD => ({
    type: EDIT_TODO,
    task: editTask,
})


export const DeleteTask = (currentID: number): CRUD => ({
    type: DELETE_TODO,
    id: currentID
})


