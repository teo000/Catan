import Modal from "react-modal";
import React, {useEffect, useState} from "react";
import "./tradeWindow.css"
import useFetch from "../../../../hooks/useFetch";
import {usePlayer} from "../../../../context/PlayerProvider";
import {BaseResponse} from "../../../../responses/BaseResponse";
import {PlayerDto} from "../../../../interfaces/PlayerDto";

interface TradeBankProps {
    players: PlayerDto[];
    isOpen: boolean;
    setIsOpen: (isOpen: boolean) => void;
}

const resourceOptions = ['Brick', 'Wood', 'Sheep', 'Wheat', 'Ore'];

export const TradePlayer: React.FC<TradeBankProps> = ({ players, isOpen, setIsOpen }) => {
    const { data, error, loading, request } = useFetch<BaseResponse>('/api/v1/Trade');

    const {player, gameId} = usePlayer();
    const otherPlayers = players.filter(p => p.id !== player?.id);

    const [giveResource, setGiveResource] = useState(resourceOptions[0]);
    const [receiveResource, setReceiveResource] = useState(resourceOptions[0]);
    const [receiveCount, setReceiveCount] = useState(1);
    const [giveCount, setGiveCount] = useState(1);
    const [message, setMessage] = useState("");
    const [playerToGive, setPlayerToGive] = useState(otherPlayers[0].name);


    const handleConfirm = async () => {
        const selectedPlayer = otherPlayers.find(p => p.name === playerToGive)

        const requestData = {
            gameId,
            playerToGiveId: player?.id,
            resourceToGive: giveResource,
            countToGive: giveCount,
            playerToReceiveId: selectedPlayer?.id,
            resourceToReceive: receiveResource,
            countToReceive: receiveCount
        };

        console.log(requestData)
        console.log(players);

        try {
            const response = await request('/initiate', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to trade: Invalid response format', response);
                if (response)
                    setMessage(response.validationErrors)
            }
            if (response.success)
                setMessage("Trade is pending.");
            console.log(response);
        } catch (err) {
            console.error('Failed to trade player', err);
        }
    };

    const handleCancel = () => {
        setIsOpen(false);
    };

    return (
        <Modal
            isOpen={isOpen}
            onRequestClose={handleCancel}
            contentLabel="Trade with a player"
            ariaHideApp={false}
        >
            <h2>Trade with a player</h2>
            <div className="trade-resource">
                <label htmlFor="playerToGive">Player:</label>
                <select
                    id="playerToGive"
                    value={playerToGive}
                    onChange={(e) => setPlayerToGive(e.target.value)}
                >
                    {otherPlayers.map(p => p.name).map((name) => (
                        <option key={name} value={name}>
                            {name}
                        </option>
                    ))}
                </select>
            </div>

            <div className="trade-resource">
                <label htmlFor="giveResource">Give Resource:</label>

                <select
                    id="giveResource"
                    value={giveResource}
                    onChange={(e) => setGiveResource(e.target.value)}
                >
                    {resourceOptions.map((resource) => (
                        <option key={resource} value={resource}>
                            {resource}
                        </option>
                    ))}
                </select>
                <input
                    type="number"
                    value={giveCount}
                    onChange={(e) => setGiveCount(Number(e.target.value))}
                    min="1"
                />
            </div>
            <div className="trade-resource">
                <label htmlFor="receiveResource">Receive Resource:</label>
                <select
                    id="receiveResource"
                    value={receiveResource}
                    onChange={(e) => setReceiveResource(e.target.value)}
                >
                    {resourceOptions.map((resource) => (
                        <option key={resource} value={resource}>
                            {resource}
                        </option>
                    ))}
                </select>
                <input
                    type="number"
                    value={receiveCount}
                    onChange={(e) => setReceiveCount(Number(e.target.value))}
                    min="1"
                />
            </div>
            <p>
                {message}
            </p>
            <button className="modal-button" onClick={handleConfirm}>OK</button>
            <button className="modal-button" onClick={handleCancel}>Close</button>
        </Modal>
    );
};