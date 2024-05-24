import {GameMap} from "./GameMap";
import {SettlementSpots} from "./settlements/SettlementSpots";
import {ActionBar, ButtonActions} from "./action-button/ActionBar";
import {useState} from "react";
import {RoadSpot} from "./roads/RoadSpot";
import {RoadSpots} from "./roads/RoadSpots";

function GameLayout(){
    const [visibleSettlementSpots, setVisibleSettlementSpots] = useState<number[]>([]);
    const [visibleRoadSpots, setVisibleRoadSpots] = useState<number[]>([]);

    const [activeButton, setActiveButton] = useState<ButtonActions>(ButtonActions.None);

    const [settlements, setSettlements] = useState<number[]>([]);
    const [roads, setRoads] = useState<number[]>([]);

    const [isSettlement, setIsSettlement] = useState<boolean[]>(new Array(54).fill(false));
    const [isRoad, setIsRoad] = useState<boolean[]>(new Array(72).fill(false));

    const handleSettlementClick = (id: number) => {
        setIsSettlement(prevState => {
            const newState = [...prevState];
            newState[id] = true;
            return newState;
        });
    };
    const handleRoadClick = (id: number) => {
        setIsRoad(prevState => {
            const newState = [...prevState];
            newState[id] = true;
            return newState;
        });
    };
    const handlePlaceSettlementButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceSettlement) {
            if (activeButton === ButtonActions.PlaceSettlement)
                setVisibleSettlementSpots([])
            else {
                const newVisibleSettlements = determineVisibleSettlements();
                setVisibleSettlementSpots(newVisibleSettlements);
            }
            setVisibleRoadSpots([]);
        } else {
            setVisibleSettlementSpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
    };

    const determineVisibleSettlements = (): number[] => {
        // aici vom face ca doar settlement-urile conectate la vreun drum sa fie vizibile
        return [1, 2, 3];
    };

    const handlePlaceRoadButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceRoad) {
            if (activeButton === ButtonActions.PlaceRoad)
                setVisibleRoadSpots([])
            else {
                const newVisibleRoads = determineVisibleRoads();
                setVisibleRoadSpots(newVisibleRoads);
            }
            setVisibleSettlementSpots([]);
        } else {
            setVisibleRoadSpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
    };

    const determineVisibleRoads = (): number[] => {
        // aici vom face ca doar road-urile conectate la vreun settlement sa fie vizibile
        return [4, 5, 6];
    };


    return (
        <div className="gameLayout">
            <div className='board-div'>
                <img
                    className='board-background'
                    src='images/water_background.png'
                    alt='background'
                />
                <GameMap />
                <div className='spots'>
                    <SettlementSpots
                        visibleSettlements={visibleSettlementSpots}
                        isSettlement={isSettlement}
                        onSettlementClick={handleSettlementClick}
                    />
                    <RoadSpots
                        visibleRoads={visibleRoadSpots}
                        isRoad={isRoad}
                        onRoadClick={handleRoadClick}
                    />
                </div>
            </div>
            <div className="gameplay-div">
                <div className="actions-chat-container">
                    <ActionBar activeButton={activeButton}
                               handlePlaceSettlementButtonClick={handlePlaceSettlementButtonClick}
                               handlePlaceRoadButtonClick={handlePlaceRoadButtonClick}
                    />
                    <div className="chat-div">
                        Trades will appear here...
                    </div>
                </div>

                <div className="cards-div"></div>
            </div>
            {/*<DiceLayout />*/}

        </div>
    );
}


export {GameLayout};