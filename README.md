
**Docker Multi-Layer Demo**

This repository demonstrates a simple multi-language, multi-layer application composed of a React + TypeScript frontend, an ASP.NET Core Web API backend, a PostgreSQL database, and an Nginx reverse proxy — all run with Docker and Docker Compose.

**Overview**
- **Frontend**: React + TypeScript app served in Docker (see [frontend/package.json](frontend/package.json)).
- **Backend**: ASP.NET Core Web API (see [backend/Program.cs](backend/Program.cs)).
- **Database**: PostgreSQL container with persistent volume `pgdata`.
- **Proxy**: Nginx used to serve the frontend and proxy API requests (see [nginx/nginx.conf](nginx/nginx.conf)).

**Prerequisites**
- **Docker**: Install Docker Engine.
- **Docker Compose**: Use the bundled `docker compose` or the standalone `docker-compose` CLI.
- Ports used: `8080` (Nginx/frontend) and `5432` (Postgres).

**Quick Start**
1. From the repository root, build and start all services:

```bash
docker compose up --build
```

2. Open the app in your browser:

- Frontend (served by Nginx): `http://localhost:8080`
- API (proxied through Nginx): `http://localhost:8080/api` (the backend listens on `http://+:5000` inside the backend container)

3. To stop and remove containers and the Postgres volume:

```bash
docker compose down -v
```

**Services (docker-compose.yml)**
- **Postgres**: Image `postgres:18.3-bookworm`, credentials are defined in `docker-compose.yml`.
- **Backend**: Built from [backend/Dockerfile](backend/Dockerfile). The backend container depends on Postgres and reads the connection string from environment variables supplied by compose.
- **Frontend**: Built from [frontend/Dockerfile](frontend/Dockerfile).
- **Nginx**: Uses [nginx/nginx.conf](nginx/nginx.conf) and exposes port `8080` to the host.

**Development**
- Run the frontend locally (fast iteration):

```bash
cd frontend
npm install
npm run dev
```

- Run the backend locally:

```bash
cd backend
dotnet run
```

- When developing locally you can point the frontend to the backend URL (see frontend config) or run everything via `docker compose` for an environment closer to production.

**Database**
- The Postgres container uses a named volume `pgdata` to persist data across restarts (defined in [docker-compose.yml](docker-compose.yml)).
- To open a psql shell in the running Postgres container:

```bash
docker compose exec postgres psql -U postgres -d docker_demo_db
```

**Useful commands**
- Build and start in detached mode: `docker compose up -d --build`
- View logs: `docker compose logs -f` or `docker compose logs -f backend`
- Rebuild one service: `docker compose build backend`

**Troubleshooting**
- If ports are in use, stop the occupying process or change port mappings in `docker-compose.yml`.
- If a container won't start, inspect logs: `docker compose logs <service>`.
- If the backend cannot connect to Postgres, verify the connection string in `docker-compose.yml` and that Postgres finished initializing before the backend attempts to connect.

**Files of interest**
- **Compose**: [docker-compose.yml](docker-compose.yml)
- **Backend**: [backend/Program.cs](backend/Program.cs), [backend/Dockerfile](backend/Dockerfile)
- **Frontend**: [frontend/package.json](frontend/package.json), [frontend/Dockerfile](frontend/Dockerfile)
- **Nginx**: [nginx/nginx.conf](nginx/nginx.conf)

**License**
- This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
