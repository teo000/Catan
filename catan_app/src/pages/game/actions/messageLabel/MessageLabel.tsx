import {useEffect, useState} from "react";
import "./message-label.css"

export const MessageLabel = ({ message } : {message : string}) => {
    const [currentMessage, setCurrentMessage] = useState(message);
    const [key, setKey] = useState(0);

    useEffect(() => {
        setCurrentMessage(message);
        setKey(prevKey => prevKey + 1);
    }, [message]);

    return (
        <div className = "message-label-container">
            { message !== "" &&
            <p key={key} className="message-label">
                {currentMessage}
            </p>}
        </div>
    );
}

