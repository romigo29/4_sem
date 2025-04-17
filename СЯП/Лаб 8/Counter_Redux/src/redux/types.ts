export interface CounterState {
  count: number
}

export const INCREMENT = 'INCREMENT'
export const DECREMENT = 'DECREMENT'
export const RESET = 'RESET'

interface IncrementAction {
  type: typeof INCREMENT
}

interface DecrementAction {
  type: typeof DECREMENT
}

interface ResetAction {
  type: typeof RESET
}

export type CounterActionTypes = IncrementAction | DecrementAction | ResetAction

