import express from "express";
import { Database } from "./Database/database";

class App {
    // refers to the Express instance
    public app: express.Application;
    public database: Database;

    // Run configuration methods
    constructor() {
        this.app = express();
        this.database = new Database();

        this.configure();
        this.routes();
      }

      // populate req.body with content from JSON sources
      private configure(): void {
        this.app.use(express.json());
        this.database.Connect();
      }

      // declare routes
      routes(): void {
        const router = express.Router();

        router.get("/", (req, res) => {
          res.status(200).send({
            message: "Sting API v0.1"
          });
        });

        this.app.use("/", router);
      }
}

export default new App().app;