import Posts from "./components/Posts";
import './App.css'

function App() {
  return (
    <div className="container">
      <header className="header">
        <h1>Менеджер постов</h1>
      </header>
      <Posts />
    </div>
  );
}

export default App;