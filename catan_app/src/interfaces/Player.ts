import {ResourceCount} from "./ResourceCount";

export interface Player {
    id: string;
    name: string;
    isActive: boolean;
    resourceCount: ResourceCount;
    settlements: any[]; // Adjust type as needed
    roads: any[]; // Adjust type as needed
}