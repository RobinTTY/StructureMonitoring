import { Document, Schema, Model, model } from "mongoose";
import { ITelemetryData } from "../Interfaces/ITelemetryData";

export interface ITelemetryDataModel extends ITelemetryData, Document {
    ObjectId(): String;
    Timestamp(): Number;
    Temperature(): Number;
    Humidity(): Number;
    AirPressure(): Number;
}

export const TelemetryDataSchema: Schema = new Schema({
    objectId: String,
    timeStamp: Number,
    temperature: Number,
    humidity: Number,
    airPressure: Number,
});

TelemetryDataSchema.methods.ObjectId = function(): string {
    return (this.objectId);
};

TelemetryDataSchema.methods.Timestamp = function(): string {
    return (this.timeStamp);
};

TelemetryDataSchema.methods.Temperature = function(): string {
    return (this.temperature);
};

TelemetryDataSchema.methods.Humidity = function(): string {
    return (this.humidity);
};

TelemetryDataSchema.methods.AirPressure = function(): string {
    return (this.airPressure);
};

export const TelemetryData: Model<ITelemetryDataModel> = model<ITelemetryDataModel>("TelemetryData", TelemetryDataSchema);