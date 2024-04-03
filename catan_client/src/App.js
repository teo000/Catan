import logo from './logo.svg';
import './App.css';
import {useEffect, useState} from "react";
const HEX_WIDTH=104;
const HEX_HEIGHT=120;

const ResourcePaths = {
    BRICK: 'images/map/brick.png',
    WOOD: 'images/map/wood.png',
    SHEEP: 'images/map/sheep.png',
    WHEAT: 'images/map/wheat.png',
    ORE: 'images/map/ore.png',
    DESERT: 'images/map/desert.png',
};

function HexagonTile({top, left, resource}) {
    console.log(resource)
    const resourcePath = ResourcePaths[resource.toUpperCase()];
    console.log(resourcePath)
    return (
        <img
            className="hexagon"
            src={resourcePath}
            alt="hexagon"
            style={{
                top: `${top}px`,
                left: `${left}px`
            }}
        />
    )
}

function Road({rotation, marginRight, top, left}){
    return (
        <div
            className="road"
            style={{
                transform: `rotate(${rotation}deg)`,
                marginRight: `${marginRight}px`,
                top: `${top}px`,
                left: `${left}px`
            }}
        />
    );
}

function HexagonRow({count, top, resources}){
    const hexagons = [];
    let marginLeft = 0;
    if (count === 3){
        marginLeft = 110;
    } else if(count === 4) {
        marginLeft = 55;
    }
    let left = marginLeft + 5
    console.log(resources)
    hexagons.push(<Road left={left-10} top={top+30}/>);
    for (let i = 0; i < count ; i++) {
        hexagons.push(<HexagonTile key={i} left={left} top={top} resource={resources[i]}/>);
        left +=100
        hexagons.push(<Road left={left} top={top+30}/>);
        left+=10
    }
    return <div className="hexagon-row" style={{ marginLeft: `${marginLeft}px` }}>{hexagons}</div>;
}

function RoadRow({count, top, isTopRow = true}){
    function drawTopRow(count, top){
        let roads = [];
        for (let i = 0; i < count ; i++) {
            roads.push(<Road rotation = {60} marginRight={50} left={left} top={top-15}/>);
            left+= 70
            roads.push(<Road rotation = {120} marginRight={50} left={left} top={top-15}/>);
            left += 40
        }
        return roads
    }
    function drawBottomRow(count, top){
        let roads = [];
        for (let i = 0; i < count ; i++) {
            roads.push(<Road rotation = {120} marginRight={50} left={left+5} top={top-20}/>);
            left+= 70
            roads.push(<Road rotation = {60} marginRight={50} left={left+3} top={top-20}/>);
            left += 40
        }
        return roads
    }
    let marginLeft = 0;
    if (count === 3){
        marginLeft = 110;
    } else if(count === 4) {
        marginLeft = 55;
    }
    let left = marginLeft + 15;

    let roads=[]
    if (isTopRow) {
        roads = drawTopRow(count, top);
    } else {
        roads = drawBottomRow(count, top)
    }

    return <div className="road-row" style={{ marginLeft: `${marginLeft}px` }}>{roads}</div>;
}

function Map(){
    const [resources, setResources] = useState(null)
    const hexHeight = 95;

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const response = await fetch('https://localhost:7251/game/');

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const result = await response.json();
            setResources(result.map(item => item.resource));
            console.log(resources)
        } catch (error) {
            console.error('Error fetching data:', error.message);
        }
    };

    return (

        <>
            {resources ? (
                <>
                    <RoadRow count={3} top={0}/>
                    <HexagonRow count={3} top={0} resources={resources.slice(0, 3)}/>
                    <RoadRow count={3} top={0}/>

                    <HexagonRow count={4} top={hexHeight} resources={resources.slice(3, 7)}/>
                    <RoadRow count={4} top={hexHeight}/>

                    <RoadRow count={5} top={2*hexHeight} />
                    <HexagonRow count={5} top={2*hexHeight} resources={resources.slice(7, 12)}/>
                    <RoadRow count={5} top={3*hexHeight} isTopRow={false}/>

                    <HexagonRow count={4} top={3*hexHeight} resources={resources.slice(12, 16)}/>
                    <RoadRow count={4} top={4*hexHeight} isTopRow={false}/>

                    <HexagonRow count={3} top={4*hexHeight} isTopRow={false} resources={resources.slice(16, 19)}/>
                    <RoadRow count={3} top={5*hexHeight} isTopRow={false}/>
                </>
            ) : (
                <p>Loading...</p>
            )}
        </>
    );
}
export default Map;
