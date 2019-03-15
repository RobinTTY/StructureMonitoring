import { Document, Schema, Model, model } from "mongoose";
import { ITelemetryData } from "../Interfaces/ITelemetryData";

export interface ITelemetryDataModel extends ITelemetryData, Document {
}

export const TelemetryDataSchema: Schema = new Schema({
    objectId: String,
    timeStamp: Number,
    temperature: Number,
    humidity: Number,
    airPressure: Number,
});

export const TelemetryData: Model<ITelemetryDataModel> = model<ITelemetryDataModel>("TelemetryData", TelemetryDataSchema);