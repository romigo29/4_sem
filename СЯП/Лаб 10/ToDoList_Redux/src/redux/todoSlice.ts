import { createSlice, PayloadAction} from '@reduxjs/toolkit'

export interface ToDoTask {
    id: number
    text: string
    isDone: boolean
}

const initialState: ToDoTask[] = [
    { id: 1, text: "Съесть мороженное", isDone: false },
    { id: 2, text: "Пробежаться", isDone: true }
]

export const todoSlice = createSlice({
    name: 'todos',
    initialState,
    reducers: {
        addTask: (state, action: PayloadAction<string>) => {
            state.push({
                id: Date.now(),
                text: action.payload,
                isDone: false
            })
        },
        toggleTask: (state, action: PayloadAction<number>) => {
            const todo = state.find(todo => todo.id === action.payload)
            if (todo) {
                todo.isDone = !todo.isDone
            }
        },
        editTask: (state, action: PayloadAction<ToDoTask>) => {
            const todo = state.find(todo => todo.id === action.payload.id)
            if (todo) {
                todo.text = action.payload.text
            }
        },
        deleteTask: (state, action: PayloadAction<number>) => {
            return state.filter(todo => todo.id !== action.payload)
        }
    },

})

export const { addTask, toggleTask, editTask, deleteTask } = todoSlice.actions

export default todoSlice.reducer 