# 💬 Reenbit Chat (SignalR + .NET 8 + Angular 18 + Azure)

**Reenbit Chat** — це реальний чат-додаток із підтримкою **SignalR**, **Sentiment Analysis**, та **Azure SQL**.  
Проєкт виконано як тестове завдання для **Reenbit Trainee Camp: Back-End Development (.NET)**.

---

## 🚀 Live Demo

- 🌐 **Frontend (Angular)**: [https://victorious-glacier-082ff5403.3.azurestaticapps.net](https://victorious-glacier-082ff5403.3.azurestaticapps.net)  
- ⚙️ **Backend (Web API)**: [https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net](https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net)  
- 📜 **Swagger**: `/swagger`

---

### ⚠️ Note
> The backend (API) is hosted on **Azure App Service** with an **Azure SQL Database** (Free Tier).  
> It may occasionally go into sleep mode — if the API returns a 404 or timeout, wait 20–30 seconds and refresh the page.

---

## 🧠 Features

### 🔹 Real-time Chat
- Built with **ASP.NET Core SignalR**
- Users can join chat rooms and exchange messages instantly
- System notifications for joining users

### 🔹 Sentiment Analysis (Optional Feature)
- Integrated with **Azure Cognitive Services Text Analytics API**
- Each message analyzed as **Positive / Neutral / Negative**
- Messages color-coded in the UI for clarity

### 🔹 Data Storage
- Messages stored in **Azure SQL Database**
- EF Core + Code First Migrations

### 🔹 UI (Angular + Tailwind)
- Clean, responsive interface
- Real-time updates via SignalR
- Auto-scroll to new messages
- Sentiment highlighting

### 🔹 Deployment
- Backend deployed on **Azure App Service (.NET 8)**
- Frontend deployed on **Azure Static Web Apps**
- Connected to **Azure SQL Database**

---

## 🧩 Technologies
- **Backend:** .NET 8, SignalR, EF Core, Azure Cognitive Services  
- **Frontend:** Angular 18, TypeScript, TailwindCSS  
- **Database:** Azure SQL  
- **Hosting:** Azure App Service, Azure Static Web Apps  

---

## 🧑‍💻 Author
**Vasyl Ukhal**  
📧 [ukhal.vasyl@gmail.com](mailto:ukhal.vasyl@gmail.com)  
🔗 [GitHub: 7LOD](https://github.com/7LOD)  
