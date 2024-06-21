import React, {useState} from "react";
import {DevelopmentCardDto} from "../../../../../interfaces/DevelopmentCardDto";
import "./development.css"
import {DevelopmentCard} from "./DevelopmentCard";

interface DevelopmentProps{
    developmentCards : DevelopmentCardDto[];
}
export const DevelopmentCards: React.FC<DevelopmentProps> = ({ developmentCards}) => {
    const [currentIndex, setCurrentIndex] = useState(0);

    const handlePrevious = () => {
        setCurrentIndex((prevIndex) => (prevIndex > 0 ? prevIndex - 1 : developmentCards.length - 1));
    };

    const handleNext = () => {
        setCurrentIndex((prevIndex) => (prevIndex < developmentCards.length - 1 ? prevIndex + 1 : 0));
    };

    return (
        <div className="development-cards-carousel">
            <button onClick={handlePrevious} className="arrow-button left"> <i className="fas fa-arrow-left" /> </button>
            {developmentCards.map((devCard, index) => (
                <DevelopmentCard key={index} devCard={devCard} isActive={index === currentIndex} />
            ))}
            <button onClick={handleNext} className="arrow-button right"> <i className="fas fa-arrow-right" />  </button>
        </div>
    );


};
