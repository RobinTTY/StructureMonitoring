import { Room } from './room';

export class Floor {
    id: number;
    alias: string;
    name: string;
    link: string;
    rooms: Array<Room>;
}
