import React from "react";
import {ResourceCountDto} from "../../../../interfaces/ResourceCountDto";
import "./resourcesDiv.css"

export function ResourcesDiv({resourceCount} : {resourceCount : ResourceCountDto}){
    // console.log(resourceCount);
    return <div className="resources-div">
        <div className="resources-container">
            <p> Brick: {resourceCount["Brick"]} </p>
            <p> Sheep: {resourceCount["Sheep"]} </p>
            <p> Ore: {resourceCount["Ore"]} </p>
            <p> Wheat: {resourceCount["Wheat"]} </p>
            <p> Wood: {resourceCount["Wood"]} </p>
        </div>
    </div>
}