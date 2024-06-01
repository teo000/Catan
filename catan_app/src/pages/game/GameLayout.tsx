import {GameMap} from "./board/GameMap";
import {SettlementSpots} from "./board/settlements/SettlementSpots";
import {ActionBar, ButtonActions} from "./actions/actionButton/ActionBar";
import React, {useEffect, useState} from "react";
import {RoadSpots} from "./board/roads/RoadSpots";
import {ComputeSettlementSpotsInfo} from "./board/settlements/ComputeSettlementSpotsInfo";
import {Settlements} from "./board/settlements/Settlements";
import {ComputeRoadSpotsInfo} from "./board/roads/ComputeRoadSpotsInfo";
import {Roads} from "./board/roads/Roads";
import {TurnTimerLabel} from "./actions/turnTimerLabel/TurnTimerLabel";
import {GameSession} from "../../interfaces/GameSession";
import useFetch from "../../hooks/useFetch";
import {LobbyResponse} from "../../responses/LobbyResponse";
import {usePlayer} from "../../context/PlayerProvider";
import DiceLayout from "./actions/dice/DiceLayout";
import {ResourcesDiv} from "./actions/resourcesDiv/ResourcesDiv";
import {getEmptyResourceCount} from "../../interfaces/ResourceCount";
import Overlay from "./misc/overlay/Overlay";
import {TradeBank} from "./actions/tradeWindow/TradeBank";
import {TradePlayer} from "./actions/tradeWindow/TradePlayer";
import {TradeList} from "./actions/pendingTrades/TradesList";
import {Cities} from "./board/cities/Cities";


interface GameLayoutProps {
    gameSession : GameSession
}

const GameLayout: React.FC<GameLayoutProps> = ({gameSession}) => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/Game');

    const [visibleSettlementSpots, setVisibleSettlementSpots] = useState<number[]>([]);
    const [visibleRoadSpots, setVisibleRoadSpots] = useState<number[]>([]);
    const [visibleCitySpots, setVisibleCitySpots] = useState<number[]>([]);

    const [activeButton, setActiveButton] = useState<ButtonActions>(ButtonActions.None);

    const settlementSpotInfo = ComputeSettlementSpotsInfo();
    const roadSpotInfo = ComputeRoadSpotsInfo();

    const {player} = usePlayer();

    const isAbandoned = (gameSession.gameStatus === 'Abandoned')
    const isWon = (gameSession.gameStatus === 'Finished')

    const [isTradeBankOpen, setIsTradeBankOpen] = useState(false);
    const [isTradePlayerOpen, setIsTradePlayerOpen] = useState(false);

    useEffect(() => {
        if (player?.id !== gameSession.turnPlayer.id) {
            setActiveButton(ButtonActions.None)
            setVisibleRoadSpots([]);
            setVisibleSettlementSpots([]);
        }
    }, [gameSession.turnPlayer.id, player?.id]);

    const handleOpenTradeBank = () => {
        setIsTradeBankOpen(true);
    };

    const handleOpenTradePlayer = () => {
        setIsTradePlayerOpen(true);
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
    };

    const handlePlaceSettlementButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceSettlement) {
            if (activeButton === ButtonActions.PlaceSettlement)
                setVisibleSettlementSpots([])
            else {
                const newVisibleSettlements = determineVisibleSettlementSpots();
                setVisibleSettlementSpots(newVisibleSettlements);
            }
            setVisibleRoadSpots([]);
            setVisibleCitySpots([]);
        } else {
            setVisibleSettlementSpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
    };

    const determineVisibleSettlementSpots = (): number[] => {
        // aici vom face ca doar settlement-urile conectate la vreun drum sa fie vizibile

        const settlementPositionsSet = new Set<number>(  Array.from({ length: 55 }, (_, index) => index));
        const settlementPositions = gameSession.map.settlements.map(s => s.position);
        console.log("settlements: " + settlementPositions)
        for (const pos of settlementPositions)
            settlementPositionsSet.delete(pos);

        return Array.from(settlementPositionsSet);
    };

    const handleCityClick = async (id: number) => {
        const requestData = {gameId: gameSession.id, playerId: player?.id, position: id};

        try {
            const response = await request('/city', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to place settlement: Invalid response format', response);
            }
            console.log(response);
            setVisibleCitySpots(prevState => {
                const newState = [...prevState];
                const index = newState.indexOf(id);
                if (index > -1) {
                    newState.splice(index, 1);
                }
                return newState;
            });
        } catch (err) {
            console.error('Failed to place settlement', err);
        }
    };

    const handlePlaceCityButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceCity) {
            if (activeButton === ButtonActions.PlaceCity)
                setVisibleCitySpots([])
            else {
                const newVisibleCities = determineVisibleCitySpots();
                setVisibleCitySpots(newVisibleCities);
            }
            setVisibleRoadSpots([]);
            setVisibleSettlementSpots([]);
        } else {
            setVisibleCitySpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
    };

    const determineVisibleCitySpots = (): number[] => {
        // aici vom face ca doar settlement-urile conectate la vreun drum sa fie vizibile

        const settlementPositions = gameSession.map.settlements
            .filter(s => s.playerId === player?.id)
            .map(s => s.position);

        console.log("settlements: " + settlementPositions)

        return Array.from(settlementPositions);
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
            setVisibleCitySpots([]);
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

    console.log(gameSession);

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
                    <SettlementSpots
                        settlementSpotInfo={settlementSpotInfo}
                        visibleSettlementSpots={visibleCitySpots}
                        onSettlementClick={handleCityClick}
                    />
                    <RoadSpots
                        roadSpotInfo={roadSpotInfo} // fa chestia asta globala statica sau ceva de genul
                        visibleRoadSpots={visibleRoadSpots}
                        onRoadClick={handleRoadClick}
                    />
                </div>
                <Settlements settlementSpotInfo={settlementSpotInfo}
                             settlements={gameSession.map.settlements}
                             players={gameSession.players}/>
                <Roads roadSpotInfo={roadSpotInfo}
                       roads={gameSession.map.roads}
                       players={gameSession.players}/>
                <Cities settlementSpotInfo={settlementSpotInfo}
                        cities={gameSession.map.cities}
                        players={gameSession.players}/>
            </div>
            <div className="gameplay-div">
                <div className="actions-chat-container">
                    <ActionBar activeButton={activeButton}
                               handlePlaceSettlementButtonClick={handlePlaceSettlementButtonClick}
                               handlePlaceRoadButtonClick={handlePlaceRoadButtonClick}
                               handlePlaceCityButtonClick={handlePlaceCityButtonClick}
                               handleTradeBankButtonClick={handleOpenTradeBank}
                               handleTradePlayerButtonClick={handleOpenTradePlayer}
                    />
                    {/*<div className="chat-div">*/}
                    {/*    Trades will appear here...*/}
                    {/*</div>*/}
                    <TradeList trades={gameSession.trades} players={gameSession.players}/>
                </div>
                <ResourcesDiv resourceCount={resourceCount}/>
            </div>
            {isAbandoned && <Overlay winner={null} message="Game has been abandoned" />}
            {isWon && gameSession.winner &&
                <Overlay winner={gameSession.winner.name}
                         message="Game finished!"
                         // message={gameSession.winner.id === player.id ?
                         // "You won!" : `${gameSession.winner.name} wins`}

                />
            }
            <TradeBank isOpen={isTradeBankOpen} setIsOpen={setIsTradeBankOpen}/>
            <TradePlayer players={gameSession.players} isOpen={isTradePlayerOpen} setIsOpen={setIsTradePlayerOpen}/>
        </div>
    );
}


export {GameLayout};