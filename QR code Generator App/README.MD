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
