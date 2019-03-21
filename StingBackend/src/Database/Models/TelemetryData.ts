import * as mongoose from "mongoose";
import { prop, Typegoose } from "typegoose";


export class TelemetryData extends Typegoose {
    @prop()
    TelemetryDataId: number;
    @prop()
    timeStamp?: number;
    @prop()
    temperature: number;
    @prop()
    humidity: number;
    @prop()
    airPressure: number;
}

export const UserModel = new TelemetryData().getModelForClass(TelemetryData, {
    existingMongoose: mongoose,
    schemaOptions: {collection: "TelemetryData"}
})