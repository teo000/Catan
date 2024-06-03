import {MapDto} from "../interfaces/MapDto";

export interface MapResponse {
    map: MapDto;
    success: boolean;
    message: string;
    validationErrors: any | null;
}