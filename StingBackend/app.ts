import debug from "debug";
import express from "express";

import routes from "./routes/index";
import users from "./routes/user";

const app = express();

app.use("/", routes);
app.use("/users", users);

app.set("port", process.env.PORT || 3000);

app.get("/", (req, res) => {
    res.send("Hello World!");
});

const server = app.listen(app.get("port"), function () {
    debug("Express server listening on port " + server.address().port);
});