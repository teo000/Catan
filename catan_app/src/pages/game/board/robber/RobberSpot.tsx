interface SettlementSpotProps {
    left: number,
    top: number,
    index: number,
    onClick: (id: number) => void
}

function RobberSpot({left, top, index, onClick} : SettlementSpotProps){

    return (
        <div className='robber-spot'
             onClick = {() => onClick(index)}
             style={{
                 left: `${left}px`,
                 top: `${top}px`
             }}>
        </div>
    )
}
export {RobberSpot};