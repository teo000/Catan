import "./cards.css"
import React from "react"
import {ResourceCountDto} from "../../../../interfaces/ResourceCountDto";
import {DevelopmentCardDto} from "../../../../interfaces/DevelopmentCardDto";
import ResourceCards from "./resources/ResourceCards";
import {DevelopmentCards} from "./development/DevelopmentCards";

interface CardsProps{
    resourceCount: ResourceCountDto;
    developmentCards: DevelopmentCardDto[];
    mustDiscard: boolean;
    knightOnClick: () => void;
}

export const Cards: React.FC<CardsProps> = ({ resourceCount, developmentCards, mustDiscard, knightOnClick }) => {
    return (
        <div className="cards">
            <ResourceCards resourceCount={resourceCount} mustDiscard={mustDiscard}></ResourceCards>
            <DevelopmentCards developmentCards={developmentCards} onClickKnight={knightOnClick}/>
        </div>
    );
};
