interface SettlementProps {
    left: number,
    top: number,
    index: number,
    color: string
}

function Settlement({left, top, index, color} : SettlementProps){
    return (
        <div className='settlement'
             style={{
                 left: `${left}px`,
                 top: `${top}px`,
                 backgroundColor: color
             }}>
        </div>
    )
}

export {Settlement};