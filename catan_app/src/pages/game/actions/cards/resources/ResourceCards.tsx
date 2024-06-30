import React, {useState} from "react";
import {ResourceCountDto} from "../../../../../interfaces/ResourceCountDto";
import "./resource-cards.css"
import '@fortawesome/fontawesome-free/css/all.min.css';
import useFetch from "../../../../../hooks/useFetch";
import {usePlayer} from "../../../../../contexts/PlayerProvider";
import {BaseResponse} from "../../../../../responses/BaseResponse";


const resourceImages = {
    "Ore" : "/images/cards/resources-cards/ore.jpg",
    "Wheat" : "/images/cards/resources-cards/wheat.jpg",
    "Brick" : "/images/cards/resources-cards/brick.jpg",
    "Wood" : "/images/cards/resources-cards/wood.jpg",
    "Sheep" : "/images/cards/resources-cards/sheep.jpg"
}

interface ResourceCardsProps {
    resourceCount: ResourceCountDto;
    mustDiscard: boolean;
}

const ResourceCards: React.FC<ResourceCardsProps> = ({ resourceCount, mustDiscard }) => {
    const { request } = useFetch<BaseResponse>('/api/v1/Game/discard-half');
    const {player, gameId} = usePlayer();

    const [discardCounts, setDiscardCounts] = useState<Partial<ResourceCountDto>>({});

    const handleIncrement = (resource: keyof ResourceCountDto) => {
        setDiscardCounts(prev => ({
            ...prev,
            [resource]: Math.min((prev[resource] || 0) + 1, resourceCount[resource])
        }));
    };

    const handleDecrement = (resource: keyof ResourceCountDto) => {
        setDiscardCounts(prev => ({
            ...prev,
            [resource]: Math.max((prev[resource] || 0) - 1, 0)
        }));
    };

    const handleDiscard = async () => {
        const requestData = {gameId, playerId: player?.id, resourceCount: discardCounts};

        try {
            const response = await request('', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to discard half: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to discard half', err);
        }
    };

    const totalSelected = Object.values(discardCounts).reduce((a, b) => a + (b || 0), 0);
    const halfOfTotalCards = Math.floor( Object.values(resourceCount).reduce((a, b) => a + b, 0) / 2 );

    return (
        <div className = "resource-container">
            <div className="resource-cards">
                {Object.entries(resourceCount).map(([resource, count]) => (
                    count >0 && <div key={resource} className="resource-group">
                        {Array.from({ length: count }).map((_, index) => (
                            <img
                                key={index}
                                src={resourceImages[resource as keyof ResourceCountDto]}
                                alt={resource}
                                className="resource-card"
                            />
                        ))}
                        {mustDiscard && resourceCount[resource as keyof ResourceCountDto] > 0 && (
                            <div className="discard-controls">
                                <button
                                    className = "resource-arrow-button"
                                    onClick={() => handleIncrement(resource as keyof ResourceCountDto)}
                                >
                                    <i className="fas fa-arrow-up"></i>
                                </button>
                                <span>{discardCounts[resource as keyof ResourceCountDto] || 0}</span>
                                <button
                                    className = "resource-arrow-button"
                                    onClick={() => handleDecrement(resource as keyof ResourceCountDto)}>
                                    <i className="fas fa-arrow-down"></i>
                                </button>
                            </div>
                        )}
                    </div>
                ))}

            </div>
            {mustDiscard && (
                <button
                    className="discard-action-button"
                    onClick={() => handleDiscard()}
                    disabled={totalSelected < halfOfTotalCards}
                >
                    Discard {halfOfTotalCards} cards
                </button>
            )}
        </div>
    );
};

export default ResourceCards;
