import Login from "./Login";
import Register from "./Register";
import "./authentication.css"
export const Authentication : React.FC = () =>{
    return (
        <div className="authentication-page">
            <div className="forms-container">
                <Login/>
                <div className="separator"></div>
                <Register/>
            </div>
        </div>
    )
}