export type ToDoTask = {
    id: number,
    text: string,
    isDone: boolean,
}

export type TODOS = Array<ToDoTask>


export const ADD_TODO = 'ADD_TODO'
export const TOGGLE_TODO = 'TOGGLE_TODO'
export const EDIT_TODO = 'EDIT_TODO'
export const DELETE_TODO = 'DELETE_TODO'

interface AddAction {
    type: typeof ADD_TODO,
    task:
    {
        text: string,
        isDone: boolean
    }
}

interface ToggleAction {
    type: typeof TOGGLE_TODO,
    id: number
}

interface EditAction {
    type: typeof EDIT_TODO
    task: ToDoTask
}

interface DeleteAction {
    type: typeof DELETE_TODO
    id: number
}


export type CRUD = AddAction | ToggleAction | EditAction | DeleteAction