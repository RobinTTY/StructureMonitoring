import { Room } from './room';

export class Floor {
    id: number;
    alias: string;
    description: string;
    link: string;
    rooms: Array<Room>;
}
