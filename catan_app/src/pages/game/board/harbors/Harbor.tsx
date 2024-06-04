interface HarborProps{
    top: number,
    left: number,
    rotation: number,
    type: string
}

function getImagePath(type:string){
    return "/images/harbors/" + type + ".png"
}

export function Harbor({top, left, rotation, type}: HarborProps){
    return (
        <img
            className="harbor"
            src={getImagePath(type)}
            alt={`${type} harbor`}
            style={{
                top:top,
                left:left,
                transform: `rotate(${rotation}deg)`
            }}
        />
    );
}