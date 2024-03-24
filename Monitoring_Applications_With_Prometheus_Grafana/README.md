### Return number of requests that took more than X ms

increase(sample_app_request_time_seconds_bucket{le="0.3"}[1m])

> le="0.3" indicates 300ms and the 1m between brackets is the period, 1 minute.

### Configuring Data Source for Grafana

When configuring the data source (Prometheus) for Grafana, remember that if you're running it from a container, you need to use the IP of your machine and not **localhost**
