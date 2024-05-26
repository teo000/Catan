import {GameMap} from "./GameMap";
import {SettlementSpots} from "./settlements/SettlementSpots";
import {ActionBar, ButtonActions} from "./actionButton/ActionBar";
import React, {useCallback, useEffect, useState} from "react";
import {RoadSpots} from "./roads/RoadSpots";
import {ComputeSettlementSpotsInfo} from "./settlements/ComputeSettlementSpotsInfo";
import {Settlements} from "./settlements/Settlements";
import {ComputeRoadSpotsInfo} from "./roads/ComputeRoadSpotsInfo";
import {Roads} from "./roads/Roads";
import useFetch from "../hooks/useFetch";
import {LobbyResponse} from "../responses/LobbyResponse";
import {useLocation} from "react-router-dom";

const GameLayout = React.memo(() => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/Lobby');

    const location = useLocation();
    const currentPath = location.pathname;
    const joinCode = currentPath.substring(currentPath.lastIndexOf('/') + 1);

    const [visibleSettlementSpots, setVisibleSettlementSpots] = useState<number[]>([]);
    const [visibleRoadSpots, setVisibleRoadSpots] = useState<number[]>([]);

    const [activeButton, setActiveButton] = useState<ButtonActions>(ButtonActions.None);

    const [settlements, setSettlements] = useState<number[]>([]);
    const [roads, setRoads] = useState<number[]>([]);

    const settlementSpotInfo = ComputeSettlementSpotsInfo();
    const roadSpotInfo = ComputeRoadSpotsInfo();


    const handleSettlementClick = (id: number) => {
        setSettlements(prevState => {
            const newState = [...prevState];
            newState.push(id);
            return newState;
        });
    };

    const handleRoadClick = (id: number) => {
        setRoads(prevState => {
            const newState = [...prevState];
            newState.push(id);
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

    const fetchGameState = useCallback(async () => {
        await request(`/${joinCode}`,'get');
    }, [request]);

    useEffect(() => {
        const intervalId = setInterval(fetchGameState, 1000); // Fetch every second
        return () => clearInterval(intervalId); // Cleanup interval on unmount
    }, [fetchGameState]);

    if (error) return <p>Error: {error}</p>;


    return (
        <div className="gameLayout">
            <div className='board-div'>
                <label className="turn-label">It's X's turn</label>
                <img
                    className='board-background'
                    src='/images/water_background.png'
                    alt='background'
                />
                <GameMap />
                <div className='spots'>
                    <SettlementSpots
                        settlementSpotInfo={settlementSpotInfo} // fa chestia asta globala statica sau ceva de genul
                        visibleSettlementSpots={visibleSettlementSpots}
                        onSettlementClick={handleSettlementClick}
                    />
                    <RoadSpots
                        roadSpotInfo={roadSpotInfo} // fa chestia asta globala statica sau ceva de genul
                        visibleRoadSpots={visibleRoadSpots}
                        onRoadClick={handleRoadClick}
                    />

                </div>
                <div className='settlements'>
                    <Settlements settlementSpotInfo={settlementSpotInfo} settlementIds={settlements}></Settlements>
                </div>
                <div className='roads'>
                    <Roads roadSpotInfo={roadSpotInfo} roadIds={roads}></Roads>
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
})


export {GameLayout};