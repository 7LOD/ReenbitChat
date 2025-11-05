# 💬 Reenbit Chat Application

Proof of Concept (**Stage #1**) — real-time чат на **.NET 8**, **Entity Framework Core**, **SignalR**, і **Angular 18 (Standalone)**.  
Проєкт розроблено в рамках Reenbit Internship Challenge.

---

## 🚀 Tech Stack

**Backend**
- ASP.NET Core 8 (Web API + SignalR)
- EF Core + SQL Server / Azure SQL
- CORS + Swagger UI

**Frontend**
- Angular 18 (Standalone Components)
- SignalR Client @microsoft/signalr
- Tailwind / CSS

**Cloud**
- Azure Web App (API)
- Azure SQL Database
- Azure SignalR Service
- Azure Static Web App (Frontend)

---

## 🧱 Solution Structure


ReenbitChat/
├── src/
│ ├── ReenbitChat.Domain/
│ ├── ReenbitChat.Application/
│ ├── ReenbitChat.Infrastructure/
│ └── ReenbitChat.Web/
│ ├── Hubs/
│ ├── Endpoints/
│ ├── Program.cs
│ └── ClientApp/
└── README.md
---

## ⚙️ Local Run

### 🗄️ Database
```bash
dotnet ef database update

🔌 Backend
dotnet run --project src/ReenbitChat.Web

Swagger → http://localhost:5000/swagger

💻 Frontend
cd src/ReenbitChat.Web/ClientApp
npm install
ng serve


Frontend → http://localhost:4200

☁️ Azure Deployment

API → Azure Web App

DB → Azure SQL Database

SignalR → Azure SignalR Service

Frontend → Azure Static Web App