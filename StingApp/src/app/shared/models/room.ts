import { Thresholds } from './thresholds';

export class Room {
    id: number;
    alias: string;
    x: number;
    y: number;
    device: string;
    name: string;
    link: string;
    thresholds: Thresholds;
}
