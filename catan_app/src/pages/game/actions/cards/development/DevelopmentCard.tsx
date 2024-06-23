import {DevelopmentCardDto} from "../../../../../interfaces/DevelopmentCardDto";
import {useState} from "react";

interface CardProps {
    devCard: DevelopmentCardDto;
    isActive: boolean;
    onClick: (() => void )| undefined
}

const IMG_PATHS : Record<string, string> = {
    "VICTORY_POINT" : "/images/development/victory_point.png",
    "KNIGHT" : "/images/development/knight.png"
}

export const DevelopmentCard : React.FC<CardProps> = ({devCard,isActive, onClick}) => {
    const [isClicked, setIsClicked] = useState(false);

    if (onClick === undefined)
        return (
            <div className={`development-card-container ${isActive ? 'active' : ''}`}>
                <img
                    className="development-card"
                    src={IMG_PATHS[devCard.developmentType]}
                    alt={devCard.developmentType}
                />
            </div>
        );

    const handleClick = () =>{
        setIsClicked(!isClicked);

        onClick();
    }

        return (
            <div className={`development-card-container   ${isActive ? 'active' : ''}  `  }>
                <img
                    className={`development-card clickable-dev-card ${isClicked ? 'clicked-dev-card' : ''}`}
                    onClick={handleClick}
                    src={IMG_PATHS[devCard.developmentType]}
                    alt={devCard.developmentType}
                />
            </div>
        );


}