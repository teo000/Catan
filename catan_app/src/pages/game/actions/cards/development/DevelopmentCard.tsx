import {DevelopmentCardDto} from "../../../../../interfaces/DevelopmentCardDto";

interface CardProps {
    devCard: DevelopmentCardDto;
    isActive: boolean;
}
const IMG_PATHS : Record<string, string> = {
    "VICTORY_POINT" : "/images/development/year_of_plenty.png",
    "KNIGHT" : "/images/development/year_of_plenty.png"
}

export const DevelopmentCard : React.FC<CardProps> = ({devCard,isActive}) => {
    return (
        <div className={`development-card-container ${isActive ? 'active' : ''}`}>
            <img
                className="development-card"
                src={IMG_PATHS[devCard.developmentType]}
                alt={devCard.developmentType}
            />
        </div>
    );
}