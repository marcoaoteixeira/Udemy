const express = require("express");
const app = express();
const promClient = require("prom-client");
const promClientRegister = promClient.register;

const UPDATE_METRICS_INTERVAL = 50;
const AVERAGE_ONLINE_USERS = 500;
const ONLINE_USERS_FLUCTUATION = 100;
const ERROR_RATE = 6;

const requestCounter = new promClient.Counter({
  name: "sample_app_counter_request_total",
  help: "Counter",
  labelNames: ["statusCode"],
});

const onlineUserGauge = new promClient.Gauge({
  name: "sample_app_online_users_total",
  help: "Total Online Users - Gauge",
});

const requestDurationHistogram = new promClient.Histogram({
  name: "sample_app_histogram_request_duration_seconds",
  help: "Request Duration - Histogram",
});

// https://stackoverflow.com/questions/25582882/javascript-math-random-normal-distribution-gaussian-bell-curve
const randomBoxMullerDistribution = (min, max, skew) => {
  let u = 0;
  let v = 0;

  //Converting [0,1) to (0,1)
  while (u === 0) {
    u = Math.random();
  }

  while (v === 0) {
    v = Math.random();
  }

  let num = Math.sqrt(-2.0 * Math.log(u)) * Math.cos(2.0 * Math.PI * v);

  num = num / 10.0 + 0.5; // Translate to 0 -> 1

  // resample between 0 and 1 if out of range
  if (num > 1 || num < 0) {
    num = randomBoxMullerDistribution(min, max, skew);
  }

  num = Math.pow(num, skew); // Skew
  num *= max - min; // Stretch to fill range
  num += min; // offset to min

  return num;
};

const sleep = (ms) => {
  return new Promise((resolve) => {
    setTimeout(resolve, ms);
  });
};

app.get("/", (_req, res) => {
  res.send("Hello World from NodeJS");
});

let resetOnlineUsers = false;
app.get("/reset-online-users", (req, res) => {
  resetOnlineUsers = req.query.reset === "true";
  res.send(`Reset Online Users - OK (${resetOnlineUsers})`);
});

const increaseCounters = () => {
  //console.log("Running counters...");

  // Increment request counter
  const statusCode = Math.random() < ERROR_RATE / 100 ? "500" : "200";
  requestCounter.labels(statusCode).inc();

  // Update online users gauge
  const totalOnlineUsers = resetOnlineUsers
    ? 0
    : AVERAGE_ONLINE_USERS -
      Math.round(ONLINE_USERS_FLUCTUATION * Math.random());
  onlineUserGauge.set(totalOnlineUsers);

  // Update request response time
  const responseTime = randomBoxMullerDistribution(0, 3, 4);
  requestDurationHistogram.observe(responseTime);
};

setInterval(increaseCounters, UPDATE_METRICS_INTERVAL);

app.get("/metrics", async function (_req, res) {
  res.set("Content-Type", promClientRegister.contentType);
  res.end(await promClientRegister.metrics());
});

app.listen(5000);
