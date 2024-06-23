import {GameMap} from "./board/GameMap";
import {SettlementSpots} from "./board/settlements/SettlementSpots";
import {ActionBar, ButtonActions} from "./actions/buttons/ActionBar";
import React, {useEffect, useState} from "react";
import {RoadSpots} from "./board/roads/RoadSpots";
import {ComputeSettlementSpotsInfo} from "./board/settlements/ComputeSettlementSpotsInfo";
import {Settlements} from "./board/settlements/Settlements";
import {ComputeRoadSpotsInfo} from "./board/roads/ComputeRoadSpotsInfo";
import {Roads} from "./board/roads/Roads";
import {TurnTimerLabel} from "./actions/turnTimerLabel/TurnTimerLabel";
import {GameSessionDto} from "../../interfaces/GameSessionDto";
import useFetch from "../../hooks/useFetch";
import {LobbyResponse} from "../../responses/LobbyResponse";
import {usePlayer} from "../../context/PlayerProvider";
import DiceLayout from "./actions/dice/DiceLayout";
import ResourceCards from "./actions/cards/resources/ResourceCards";
import {getEmptyResourceCount} from "../../interfaces/ResourceCountDto";
import Overlay from "./misc/overlay/Overlay";
import {TradeBank} from "./actions/tradeModal/TradeBank";
import {TradePlayer} from "./actions/tradeModal/TradePlayer";
import {Cities} from "./board/cities/Cities";
import {ChatDiv} from "./actions/chat-div/ChatDiv";
import "./game-layout.css"
import {Robber} from "./board/robber/Robber";
import {RobberSpots} from "./board/robber/RobberSpots";
import {useMessage} from "./hooks/useMessage";
import {MessageLabel} from "./actions/messageLabel/MessageLabel";
import {Harbors} from "./board/harbors/Harbors";
import useVisibleSpots from "./hooks/useVisibleSpots";
import {Cards} from "./actions/cards/Cards";

interface GameLayoutProps {
    gameSession : GameSessionDto
}

const GameLayout: React.FC<GameLayoutProps> = ({gameSession}) => {
    const { data, error, loading, request } = useFetch<LobbyResponse>('/api/v1/Game');

    const [visibleSettlementSpots, setVisibleSettlementSpots] = useState<number[]>([]);
    const [visibleRoadSpots, setVisibleRoadSpots] = useState<number[]>([]);
    const [visibleCitySpots, setVisibleCitySpots] = useState<number[]>([]);

    const [activeButton, setActiveButton] = useState<ButtonActions>(ButtonActions.None);
    const [buttonsDisabled, setButtonsDisabled] = useState(false);

    const {player} = usePlayer();

    const [isTradeBankOpen, setIsTradeBankOpen] = useState(false);
    const [isTradePlayerOpen, setIsTradePlayerOpen] = useState(false);

    const message = useMessage(gameSession);

    const {getVisibleSettlementSpots, getVisibleRoadSpots} = useVisibleSpots(gameSession);
    
    useEffect(() =>{
        if((gameSession.round === 1 || gameSession.round ===2 ) && player?.id === gameSession.turnPlayer.id) {
            setButtonsDisabled(true);
            if (gameSession.map.settlements > gameSession.map.roads){
                setVisibleSettlementSpots([]);
                setVisibleRoadSpots(getVisibleRoadSpots());
            }
            else{
                setVisibleRoadSpots([]);
                setVisibleSettlementSpots(getVisibleSettlementSpots());
            }
        }
        else if (player?.id !== gameSession.turnPlayer.id || !gameSession.dice.rolledThisTurn) {
            setButtonsDisabled(true);
            setVisibleRoadSpots([]);
            setVisibleSettlementSpots([]);
        }
        else {
            setButtonsDisabled(false);

        }
    }, [gameSession.round, gameSession.turnPlayer.id, gameSession.dice.rolledThisTurn, gameSession.map.settlements, gameSession.map.roads], );

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
        setVisibleSettlementSpots([]);
        setActiveButton(ButtonActions.None);
    };

    const handlePlaceSettlementButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceSettlement) {
            if (activeButton === ButtonActions.PlaceSettlement)
                setVisibleSettlementSpots([])
            else {
                const newVisibleSettlements = getVisibleSettlementSpots();
                setVisibleSettlementSpots(newVisibleSettlements);
            }
            setVisibleRoadSpots([]);
            setVisibleCitySpots([]);
        } else {
            setVisibleSettlementSpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
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
            console.error('Failed to place city', err);
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
        setVisibleRoadSpots([]);
        setActiveButton(ButtonActions.None);
    };

    const handlePlaceRoadButtonClick = (action: ButtonActions) => {
        if (action === ButtonActions.PlaceRoad) {
            if (activeButton === ButtonActions.PlaceRoad)
                setVisibleRoadSpots([])
            else {
                const newVisibleRoads = getVisibleRoadSpots();
                setVisibleRoadSpots(newVisibleRoads);
            }
            setVisibleSettlementSpots([]);
            setVisibleCitySpots([]);
        } else {
            setVisibleRoadSpots([]);
        }
        setActiveButton(action === activeButton ? ButtonActions.None : action);
    };

    const handleDevelopmentButtonClick = async () => {
        console.log("BAAAAAI");
        const requestData =
            {
                gameId: gameSession.id,
                playerId: player?.id,
                moveType:"BuyDevelopmentCard"
            };
        console.log("development button click")

        try {
            const response = await request('/make-move', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to buy development card: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to buy development card.', err);
        }

    };

    const onRobberClick = async (position:number) => {
        const requestData = {gameId: gameSession.id, playerId: player?.id, position};

        try {
            const response = await request('/thief', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to move thief: Invalid response format', response);
            }
            console.log(response);
        } catch (err) {
            console.error('Failed to move thief', err);
        }

    };



    if (!player) {
        return <p> Something went wrong ... </p>
    }

    console.log(gameSession);

    const isAbandoned = (gameSession.gameStatus === 'Abandoned')
    const isWon = (gameSession.gameStatus === 'Finished')


    const playerState = gameSession.players.find(p => p.id === player.id);
    const resourceCount = playerState ? playerState.resourceCount : getEmptyResourceCount()
    const lastDiceRoll = gameSession.dice.values[0] + gameSession.dice.values[1];

    if (!playerState){
        return <p> Something went wrong ... </p>
    }

    const isMyTurn = (gameSession.turnPlayer.id === player?.id);

    const diceIsClickable = isMyTurn && !gameSession.dice.rolledThisTurn &&
        !(gameSession.round === 1  || gameSession.round === 2)


    return (
        <div className="gameLayout">
            <div className='board-div'>
                <TurnTimerLabel
                    playerName={gameSession.turnPlayer.name}
                    time={gameSession?.turnEndTime}
                    disabled={buttonsDisabled}
                />
                <DiceLayout
                    gameSessionId={gameSession.id}
                    diceRoll={gameSession.dice}
                    isClickable={diceIsClickable}
                />
                <MessageLabel message={message}/>

                <img
                    className='board-background'
                    src='/images/water_background.png'
                    alt='background'
                />
                <div className="board">
                    <GameMap hexTiles={gameSession.map.hexTiles} />
                    <Robber hexTile={gameSession.map.thiefPosition}/>

                    <div className='spots'>
                        <SettlementSpots
                            visibleSettlementSpots={visibleSettlementSpots}
                            onSettlementClick={handleSettlementClick}
                        />
                        <SettlementSpots
                            visibleSettlementSpots={visibleCitySpots}
                            onSettlementClick={handleCityClick}
                        />
                        <RoadSpots
                            visibleRoadSpots={visibleRoadSpots}
                            onRoadClick={handleRoadClick}
                        />
                        <RobberSpots
                            visible={lastDiceRoll === 7 && !gameSession.thiefMovedThisTurn }
                            onRobberSpotClick={onRobberClick}
                            currentSpot={gameSession.map.thiefPosition}
                        />
                    </div>
                    <Settlements settlements={gameSession.map.settlements}
                                 players={gameSession.players}/>
                    <Roads roads={gameSession.map.roads}
                           players={gameSession.players}/>
                    <Cities cities={gameSession.map.cities}
                            players={gameSession.players}/>
                    <Harbors harbors={gameSession.map.specialHarbors}/>
                </div>
            </div>
            <div className="gameplay-div">
                <div className="actions-chat-container">
                    <ActionBar disabled={buttonsDisabled}
                               activeButton={activeButton}
                               handlePlaceSettlementButtonClick={handlePlaceSettlementButtonClick}
                               handlePlaceRoadButtonClick={handlePlaceRoadButtonClick}
                               handlePlaceCityButtonClick={handlePlaceCityButtonClick}
                               handleTradeBankButtonClick={handleOpenTradeBank}
                               handleTradePlayerButtonClick={handleOpenTradePlayer}
                               handleBuyDevelopmentButtonClick={handleDevelopmentButtonClick}
                    />

                    <ChatDiv trades={gameSession.trades} players={gameSession.players}/>
                </div>
                <Cards resourceCount={resourceCount}
                       mustDiscard={lastDiceRoll === 7 && !playerState.discardedThisTurn}
                       developmentCards={playerState.developmentCards}
                />

            </div>
            {isAbandoned && <Overlay winner={null} message="Game has been abandoned" />}
            {isWon && gameSession.winner &&
                <Overlay winner={gameSession.winner.name}
                         message="Game finished!"
                         // message={gameSession.winner.id === player.id ?
                         // "You won!" : `${gameSession.winner.name} wins`}

                />
            }
            <TradeBank isOpen={isTradeBankOpen} setIsOpen={setIsTradeBankOpen} tradeCount={playerState.tradeCount} />
            <TradePlayer players={gameSession.players} isOpen={isTradePlayerOpen} setIsOpen={setIsTradePlayerOpen}/>
        </div>
    );
}


export {GameLayout};