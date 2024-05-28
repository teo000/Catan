import {GameMap} from "./GameMap";
import {SettlementSpots} from "./settlements/SettlementSpots";
import {ActionBar, ButtonActions} from "./actionButton/ActionBar";
import React, {useState} from "react";
import {RoadSpots} from "./roads/RoadSpots";
import {ComputeSettlementSpotsInfo} from "./settlements/ComputeSettlementSpotsInfo";
import {Settlements} from "./settlements/Settlements";
import {ComputeRoadSpotsInfo} from "./roads/ComputeRoadSpotsInfo";
import {Roads} from "./roads/Roads";
import {TurnTimerLabel} from "./turnTimerLabel/TurnTimerLabel";
import {GameSession} from "../interfaces/GameSession";
import useFetch from "../hooks/useFetch";
import {LobbyResponse} from "../responses/LobbyResponse";
import {usePlayer} from "./PlayerProvider";
import DiceLayout from "./dice/DiceLayout";
import {ResourcesDiv} from "./resourcesDiv/ResourcesDiv";
import {getEmptyResourceCount} from "../interfaces/ResourceCount";
import Overlay from "./overlay/Overlay";
import Modal from 'react-modal';
import {TradeBank} from "./tradeWindow/TradeBank";



interface GameLayoutProps {
    gameSession : GameSession
}

const GameLayout: React.FC<GameLayoutProps> = ({gameSession}) => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/Game');

    const [visibleSettlementSpots, setVisibleSettlementSpots] = useState<number[]>([]);
    const [visibleRoadSpots, setVisibleRoadSpots] = useState<number[]>([]);

    const [activeButton, setActiveButton] = useState<ButtonActions>(ButtonActions.None);

    const [settlements, setSettlements] = useState<number[]>([]);
    const [roads, setRoads] = useState<number[]>([]);

    const [diceRoll, setDiceRoll] = useState(gameSession.dice);


    const settlementSpotInfo = ComputeSettlementSpotsInfo();
    const roadSpotInfo = ComputeRoadSpotsInfo();

    const {player} = usePlayer();

    const isAbandoned = (gameSession.gameStatus === 'Abandoned')

    const [isTradeBankOpen, setIsTradeBankOpen] = useState(false);

    const handleOpenTradeBank = () => {
        setIsTradeBankOpen(true);
    };

    const handleSettlementClick = async (id: number) => {
        const requestData = {gameId: gameSession.id, playerId: player?.id, position: id};

        try {
            const response = await request('/settlement', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to place settlement: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to place settlement', err);
        }

        setSettlements(prevState => {
            const newState = [...prevState];
            newState.push(id);
            return newState;
        });
    };

    const handleRoadClick = async (id: number) => {
        const requestData = {gameId: gameSession.id, playerId: player?.id, position: id};

        try {
            const response = await request('/road', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to place road: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to place road', err);
        }

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
        return Array.from({ length: 55 }, (_, index) => index);
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
        return Array.from({ length: 73 }, (_, index) => index);

    };

    if (!player)
        return <p> Something went wrong ... </p>

    // console.log(gameSession);

    let playerState = gameSession.players.find(p => p.id === player.id);
    let resourceCount = playerState ? playerState.resourceCount : getEmptyResourceCount()

    return (
        <div className="gameLayout">
            <div className='board-div'>
                <TurnTimerLabel playerName={gameSession.turnPlayer.name} time={gameSession?.turnEndTime}/>

                <img
                    className='board-background'
                    src='/images/water_background.png'
                    alt='background'
                />
                <GameMap hexTiles={gameSession.map.hexTiles}/>
                <DiceLayout gameSessionId={gameSession.id} diceRoll={gameSession.dice} turnPlayer={gameSession.turnPlayer}></DiceLayout>
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
                    <Settlements settlementSpotInfo={settlementSpotInfo}
                                 settlements={gameSession.map.settlements}
                                 players={gameSession.players}/>
                </div>
                <div className='roads'>
                    <Roads roadSpotInfo={roadSpotInfo}
                           roads={gameSession.map.roads}
                           players={gameSession.players}></Roads>
                </div>
            </div>
            <div className="gameplay-div">
                <div className="actions-chat-container">
                    <ActionBar activeButton={activeButton}
                               handlePlaceSettlementButtonClick={handlePlaceSettlementButtonClick}
                               handlePlaceRoadButtonClick={handlePlaceRoadButtonClick}
                               handleTradeBankButtonClick={handleOpenTradeBank}
                    />
                    <div className="chat-div">
                        Trades will appear here...
                    </div>
                </div>
                <ResourcesDiv resourceCount={resourceCount}/>
            </div>
            {isAbandoned && <Overlay winner={null} message="Game has been abandoned" />}
            <TradeBank isOpen={isTradeBankOpen} setIsOpen={setIsTradeBankOpen}/>
        </div>
    );
}


export {GameLayout};