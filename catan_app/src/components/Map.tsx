import {useEffect, useState} from "react";
import {HexTileRow} from "./HexTileRow";
import {HexTileData} from "./HexTile";


const generateTestData = (): HexTileData[] => {
    const testData: HexTileData[] = [];

    // Resource types and number tokens for testing
    const resources = ['Wood', 'Wheat', 'Sheep', 'Brick', 'Ore', 'Desert'];
    const numberTokens = [2, 3, 4, 5, 6, 8, 9, 10, 11, 12];

    for (let i = 0; i < 19; i++) {
        const randomResource = resources[Math.floor(Math.random() * resources.length)];
        const randomNumberToken = numberTokens[Math.floor(Math.random() * numberTokens.length)];
        testData.push({ resource: randomResource, numberToken: randomNumberToken });
    }

    return testData;
};

const testData: HexTileData[] = generateTestData();
function GameMap(){
    const [hexTileData, setHexTileData] = useState<HexTileData[]>([])

    useEffect(() => {
        setHexTileData(testData);
    }, []);

    console.log(hexTileData)

    return (
        <div className='Map'>
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