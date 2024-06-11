import React from "react";
import {ResourceCountDto} from "../../../../interfaces/ResourceCountDto";
import "./resource-cards.css"

const resourceImages = {
    "Ore" : "/images/cards/resources-cards/ore.jpg",
    "Wheat" : "/images/cards/resources-cards/wheat.jpg",
    "Brick" : "/images/cards/resources-cards/brick.jpg",
    "Wood" : "/images/cards/resources-cards/wood.jpg",
    "Sheep" : "/images/cards/resources-cards/sheep.jpg"
}

interface ResourceCardsProps {
    resourceCount: ResourceCountDto;
}

const ResourceCards: React.FC<ResourceCardsProps> = ({ resourceCount }) => {
    return (
        <div className="resource-cards">
            {Object.entries(resourceCount).map(([resource, count]) => (
                <div key={resource} className="resource-group">
                    {Array.from({ length: count }).map((_, index) => (
                        <img
                            key={index}
                            src={resourceImages[resource as keyof ResourceCountDto]}
                            alt={resource}
                            className="resource-card"
                        />
                    ))}
                </div>
            ))}
        </div>
    );
};

export default ResourceCards;
