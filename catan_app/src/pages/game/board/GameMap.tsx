import {useEffect, useState} from "react";
import {HexTileRow} from "./hexTiles/HexTileRow";
import {HexTileDto} from "./hexTiles/HexTile";
import {Robber} from "./robber/Robber";

const generateTestData = (): HexTileDto[] => {
    const testData: HexTileDto[] = [];

    const resources = ['Wood', 'Wheat', 'Sheep', 'Brick', 'Ore', 'Desert'];
    const numberTokens = [2, 3, 4, 5, 6, 8, 9, 10, 11, 12];

    for (let i = 0; i < 19; i++) {
        const randomResource = resources[Math.floor(Math.random() * resources.length)];
        const randomNumberToken = numberTokens[Math.floor(Math.random() * numberTokens.length)];
        testData.push({ resource: randomResource, number: randomNumberToken });
    }

    return testData;
};

const testData: HexTileDto[] = generateTestData();
function GameMap({hexTiles} : {hexTiles:HexTileDto[]}){
    const [hexTileData, setHexTileData] = useState<HexTileDto[]>([])

    useEffect(() => {
        if (hexTileData.length === 0)
            setHexTileData(hexTiles);
    }, []);

    // console.log(hexTileData)

    return (
        <div className='map'>
            {hexTileData.length > 0 ? (
                <>
                    <HexTileRow rowNumber={1} hexTileData={hexTileData.slice(0, 3)}/>
                    <HexTileRow rowNumber={2} hexTileData={hexTileData.slice(3, 7)}/>
                    <HexTileRow rowNumber={3} hexTileData={hexTileData.slice(7, 12)}/>
                    <HexTileRow rowNumber={4} hexTileData={hexTileData.slice(12, 16)}/>
                    <HexTileRow rowNumber={5} hexTileData={hexTileData.slice(16, 19)}/>
                </>
            ) : (
                <p>Loading...</p>
            )}
        </div>
    );
}
export {GameMap};