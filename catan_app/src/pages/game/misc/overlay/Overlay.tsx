import "./overlay.css"

interface OverlayProps {
    winner: string | null;
    message: string;
}

const Overlay: React.FC<OverlayProps> = ({ winner, message }) => {
    return (
        <div className="overlay">
            <div className="overlay-content">
                <h1>{message}</h1>
                {winner && <h2>The winner is: {winner}</h2> }
            </div>
        </div>
    );
};

export default Overlay;
