import "./overlay.css"

interface OverlayProps {
    winner: string | null;
    message: string;
}

const Overlay: React.FC<OverlayProps> = ({ winner, message }) => {
    return (
        <div className="overlay">
            <div className="overlay-content">
                {winner ? <h1>Winner: {winner}</h1> : <h1>{message}</h1>}
            </div>
        </div>
    );
};

export default Overlay;
