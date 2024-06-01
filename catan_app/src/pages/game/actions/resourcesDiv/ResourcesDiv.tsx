import React from "react";
import {ResourceCount} from "../../../../interfaces/ResourceCount";
import "./resourcesDiv.css"

export function ResourcesDiv({resourceCount} : {resourceCount : ResourceCount}){
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