import express from "express";

class App {
    // refers to the Express instance
    public app: express.Application;

    // Run configuration methods
    constructor() {
        this.app = express();
        this.configure();
      }

      // populate req.body with content from JSON sources
      private configure(): void {
        this.app.use(express.json());
      }
}

export default new App().app;