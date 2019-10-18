import { Floor } from '../models/floor';

export class Building {
    id: number;
    link: string;
    street: string;
    city: string;
    postcode: number;
    floors: Array<Floor>;
}
