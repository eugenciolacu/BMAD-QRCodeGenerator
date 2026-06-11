# QR Code Generator App

## Docker Commands

### First-time build & start
```bash
docker compose up -d --build
```

### Start (without rebuilding)
```bash
docker compose up -d
```

### Stop containers (keep data)
```bash
docker compose stop
```

### Stop and remove containers (keep volumes)
```bash
docker compose down
```

### Stop, remove containers and delete all data (volumes)
```bash
docker compose down -v
```

### Rebuild a single service
```bash
docker compose build webapp
docker compose up -d webapp
```

---

## Logs

### Follow live logs for all services
```bash
docker compose logs -f
```

### Follow live logs for a specific service
```bash
docker logs -f webapp
docker logs -f sqlserver
```

### Last N lines
```bash
docker logs --tail 100 webapp
docker logs --tail 100 sqlserver
```

---

## Container status

### Check running containers
```bash
docker compose ps
```

### Check container health
```bash
docker inspect --format='{{.State.Health.Status}}' sqlserver
```

---

## Volumes

### List volumes
```bash
docker volume ls
```
You should see:
- `qr-code-generator-app_sqlserver_data`
- `qr-code-generator-app_app_logs`

### Inspect a volume
```bash
docker volume inspect qr-code-generator-app_sqlserver_data
```

### Delete a specific volume (data will be lost)
```bash
docker volume rm qr-code-generator-app_sqlserver_data
```

---

## Build cache

### Check disk usage
```bash
docker system df
```

### Prune dangling build cache only
```bash
docker builder prune
```

### Prune all build cache
```bash
docker builder prune -a
```

### Prune everything unused (cache, stopped containers, unused images & networks)
> Does not affect running containers or named volumes.
```bash
docker system prune -a
```

---

## Application

The web app is available at: **http://localhost:8080**

SQL Server is available at: **localhost:1433**





# Playwright in docker container

## E2E Testing (Playwright)

### Run tests (in the running container)

```bash
docker exec playwright-e2e npx playwright test
```

Tips:
- **Inside Docker**: Use `--ui` to see the dashboard at http://localhost:9323.
- **On Windows**: Run `npx playwright test --ui` (locally) to see an actual browser window open on your screen.
- See the browser window: add ```--headed```
- Run a single project/browser: ```--project=chromium```
- Open testing UI: ```--ui```

### Run a specific test file

```bash
docker exec playwright-e2e npx playwright test tests/example.spec.ts
```

### View Playwright Reports

The reports and test results are synced to your Windows machine via volumes:
- HTML Report: index.html
- Asset Traces: test-results

## for --headed 

Not easily possible with Docker, can be run as ```--ui```

This is a classic "Docker limitation" that everyone runs into when learning.

The reason --headed (visible browser) does not work is that Docker containers do not have a display (Monitor/Screen). They are "headless" Linux environments.

Playwright has a specific UI mode that runs a web server. You can access it from your Windows browser.

- Add a port mapping to your playwright service
  ```bash
  ports:
  - "9323:9323"
  ```
- Run it with the ```--host``` flag
  ```bash
  docker exec playwright-e2e npx playwright test --ui --ui-host=0.0.0.0 --ui-port=9323
  ```
- Open your browser to:
  ```bash
  http://localhost:9323
  ```

## for --headed, working solution 

If needed to see browser then playwright project can be run on Windows:

- Start containers except automation test container
- Rub tests from windows
  ```bash
  npx playwright test --headed
  ```