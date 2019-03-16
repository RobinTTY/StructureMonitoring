import app from "./app";

const port = process.env.port || 5000;

app.listen(port, function() {
    console.log("Express server listening on port " + port);
  });