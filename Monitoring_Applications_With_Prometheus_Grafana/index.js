const express = require("express");
const app = express();
const promClient = require("prom-client");
const promClientRegister = promClient.register;

const counter = new promClient.Counter({
  name: "sample_app_total_requests",
  help: "Request Counter",
  labelNames: ["statusCode"],
});

const gauge = new promClient.Gauge({
  name: "sample_app_free_bytes",
  help: "Free Bytes Gauge",
});

const histogram = new promClient.Histogram({
  name: "sample_app_request_time_seconds",
  help: "API Response Time",
  buckets: [0.1, 0.2, 0.3, 0.4, 0.5],
});

const summary = new promClient.Summary({
  name: "sample_app_summary_request_time_seconds",
  help: "API Response Time - Summary",
  percentiles: [0.5, 0.9, 0.99],
});

app.get("/", async (req, res) => {
  counter.labels("200").inc();

  gauge.set(100 * Math.random());

  const responseTime = Math.random();
  histogram.observe(responseTime);

  summary.observe(responseTime);

  res.send("Hello World from NodeJS");
});

app.get("/metrics", async function (req, res) {
  res.set("Content-Type", promClientRegister.contentType);
  res.end(await promClientRegister.metrics());
});

app.listen(5000);
