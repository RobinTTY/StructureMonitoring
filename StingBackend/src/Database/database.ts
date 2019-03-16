import  mongoose  from "mongoose";

export class Database {
    public connectionString: string;

    constructor() {
        this.connectionString = "";
    }

    public Connect(): void {
        mongoose.connect(this.connectionString);
    }

    public Disconnect(): void {
        mongoose.disconnect();
    }
}