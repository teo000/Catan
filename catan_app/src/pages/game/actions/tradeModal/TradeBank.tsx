import Modal from "react-modal";
import React, {useEffect, useState} from "react";
import "./trade-window.css"
import useFetch from "../../../../hooks/useFetch";
import {LobbyResponse} from "../../../../responses/LobbyResponse";
import {usePlayer} from "../../../../context/PlayerProvider";
import {BaseResponse} from "../../../../responses/BaseResponse";
import {ResourceCountDto} from "../../../../interfaces/ResourceCountDto";

interface TradeBankProps {
    isOpen: boolean;
    setIsOpen: (isOpen: boolean) => void;
    tradeCount: ResourceCountDto;
}

const resourceOptions = ['Brick', 'Wood', 'Sheep', 'Wheat', 'Ore'];

export const TradeBank: React.FC<TradeBankProps> = ({ isOpen, setIsOpen, tradeCount }) => {
    const {request } = useFetch<BaseResponse>('/api/v1/Trade');

    const [giveResource, setGiveResource] = useState(resourceOptions[0]);
    const [receiveResource, setReceiveResource] = useState(resourceOptions[0]);
    const [receiveCount, setReceiveCount] = useState(1);
    const [giveCount, setGiveCount] = useState(receiveCount * 4);
    const [message, setMessage] = useState("");

    const {player, gameId} = usePlayer();

    const resourceCountToTrade = Object.fromEntries(Object.entries(tradeCount));

    useEffect(() => {
        setGiveCount(receiveCount * resourceCountToTrade[giveResource]);
    }, [giveResource, receiveCount, resourceCountToTrade]);

    const handleConfirm = async () => {
        const requestData = {
            gameId,
            playerId: player?.id,
            resourceToReceive: receiveResource,
            resourceToGive: giveResource,
            count: receiveCount
        };

        console.log(giveCount)

        try {
            const response = await request('/bank', 'post', requestData);
            if (response === null || !response.success){
                console.error('Failed to trade: Invalid response format', response);
                if (response)
                    setMessage(response.validationErrors)
            }
            if (response.success)
                setMessage("Trade was successful!")
            console.log(response);
        } catch (err) {
            console.error('Failed to trade bank', err);
        }

        // setIsOpen(false);
    };

    const handleCancel = () => {
        setIsOpen(false);
    };

    return (
        <Modal
            isOpen={isOpen}
            onRequestClose={handleCancel}
            contentLabel="Trade with Bank"
            ariaHideApp={false}
            className="trade-modal"
            style={{ overlay: { backgroundColor: "rgba(1, 1, 1, 0.5)", zIndex: 3} }}
        >
            <h2>Trade with Bank</h2>
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
                    readOnly
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