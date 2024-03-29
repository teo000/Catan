import logo from './logo.svg';
import './App.css';
const HEX_WIDTH=104;
const HEX_HEIGHT=120;
function HexagonTile({top, left}) {
    return (
        <div
            className="hexagon"
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

function HexagonRow({count, top}){
    const hexagons = [];
    let marginLeft = 0;
    if (count === 3){
        marginLeft = 110;
    } else if(count === 4) {
        marginLeft = 55;
    }
    let left = marginLeft + 5

    hexagons.push(<Road left={left-10} top={top+30}/>);
    for (let i = 0; i < count ; i++) {
        hexagons.push(<HexagonTile key={i} left={left} top={top} />);
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
    const hexHeight = 95;
    return (
        <>
            <RoadRow count={3} top={0}/>
            <HexagonRow count={3} top={0}/>
            <RoadRow count={3} top={0}/>

            <HexagonRow count={4} top={hexHeight}/>
            <RoadRow count={4} top={hexHeight}/>

            <RoadRow count={5} top={2*hexHeight} />
            <HexagonRow count={5} top={2*hexHeight}/>
            <RoadRow count={5} top={3*hexHeight} isTopRow={false}/>

            <HexagonRow count={4} top={3*hexHeight}/>
            <RoadRow count={4} top={4*hexHeight} isTopRow={false}/>

            <HexagonRow count={3} top={4*hexHeight} isTopRow={false}/>
            <RoadRow count={3} top={5*hexHeight} isTopRow={false}/>

        </>
    );
}
export default Map;
