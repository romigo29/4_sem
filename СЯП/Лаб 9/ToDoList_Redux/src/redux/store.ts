import { legacy_createStore as createStore } from 'redux'
import { todoReducer } from './reducer'

export const store = createStore(todoReducer)

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch