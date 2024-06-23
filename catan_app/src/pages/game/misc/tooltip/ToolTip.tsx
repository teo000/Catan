import "./tooltip.css"
interface TooltipProps {
    text: string;
}

export const Tooltip: React.FC<TooltipProps> = ({ text }) => {
    return <div className="tooltip">{text}</div>;
};