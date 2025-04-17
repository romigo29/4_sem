import { CounterState, CounterActionTypes, INCREMENT, DECREMENT, RESET } from "./types";

const startState: CounterState = {
    count: 0
}

export const counterReducer = (state = startState, action: CounterActionTypes): CounterState => {
    switch (action.type) {
        case INCREMENT: {
            return {
                count: state.count + 1
            }
        }
        case DECREMENT: {
            return {
                count: state.count - 1
            }
        }

        case RESET: {
            return {
                count: 0
            }
        }
        
        default: return state
    }
}