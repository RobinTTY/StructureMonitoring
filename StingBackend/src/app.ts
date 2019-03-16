import express, { Router } from "express";

class App {
    // refers to the Express instance
    public app: express.Application;

    // Run configuration methods
    constructor() {
        this.app = express();
        this.configure();
        this.routes();
      }

      // populate req.body with content from JSON sources
      private configure(): void {
        this.app.use(express.json());
      }

      // declare routes
      routes(): void {
        const router = express.Router();

        router.get("/", (req, res) => {
          res.status(200).send({
            message: "hello world!"
          });
        });

        this.app.use("/", router);
      }
}

export default new App().app;