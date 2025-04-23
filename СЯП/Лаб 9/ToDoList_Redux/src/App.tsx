import styles from './App.module.css'
import ToDoList from './components/ToDoList'

function App() {
  return (
    <div className={styles.root}>
      <ToDoList />
    </div>
  )
}

export default App
