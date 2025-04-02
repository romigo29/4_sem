import './History.css'
import './App.css'


function History({history} : {history: string[]}) {
    return (
    <div className="history">
        {history.length == 0 ? "нет вычислений" :
        history.map( (item, index) => (
            <div>
                {index + 1}. {item}
            </div>
        ))}
    </div>
    )
}

export default History;