import { CRUD, TODOS, ADD_TODO, TOGGLE_TODO, EDIT_TODO, DELETE_TODO } from "./types";

const Todos: TODOS = [
    { id: 1, text: "Съесть мороженное", isDone: false },
    { id: 2, text: "Пробежаться", isDone: true }
];


export const todoReducer = (state = Todos, action: CRUD): TODOS => {
    switch (action.type) {
        case ADD_TODO: {
            return [
                ...state,
                {
                    id: Date.now(),
                    text: action.task.text,
                    isDone: action.task.isDone,
                }
            ];
        }

        case TOGGLE_TODO: {
            return state.map(todo =>
                todo.id == action.id ?
                    { ...todo, isDone: !todo.isDone } : todo
            );
        }

        case EDIT_TODO: {
            return state.map(todo =>
                todo.id == action.task.id ?
                    { ...todo, text: action.task.text } : todo
            );
        }

        case DELETE_TODO: {
            return state.filter(todo =>
                todo.id != action.id
            )
        }

        default:
            return state

    }
}