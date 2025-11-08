💬 Reenbit Chat

Reenbit Chat — це реальний час чат-додаток, створений на .NET 8 (Web API) та Angular 18, із використанням SignalR для двосторонньої комунікації та (опціонально) Azure Cognitive Services для аналізу тону повідомлень.
Деплой виконано на Azure App Service.

🚀 Live Demo

🌐 Frontend (Angular): https://victorious-glacier-082ff5403.3.azurestaticapps.net

⚙️ Backend (Web API): https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net

🔍 Swagger: /swagger

🧩 Features
🔸 Real-time Chat (SignalR)

Двостороння комунікація в реальному часі через SignalR.

Кожен користувач може приєднатися до кімнати та миттєво бачити повідомлення інших.

🔸 Sentiment Analysis (optional)

Реалізовано через SentimentService (mock або Azure Cognitive Text API).

Кожне повідомлення підсвічується відповідно до його емоційного тону:

🟢 позитивне

⚪ нейтральне

🔴 негативне

🔸 Data Storage

Збереження історії повідомлень у Azure SQL Database через Entity Framework Core.

Підтримка завантаження історії при повторному вході в кімнату.

🔸 UI Enhancements

Візуальне підсвічування повідомлень за настроєм.

Відображення системних повідомлень (“user joined”, “user left”).

🧠 Tech Stack
Layer	Technology
Backend	ASP.NET Core 8, SignalR, EF Core, Azure SQL
Frontend	Angular 18, TypeScript, TailwindCSS
Deployment	Azure Web App, Azure Static Web App
Optional	Azure Cognitive Services (Text Analytics)
🛠️ Local Setup
Backend
git clone https://github.com/7LOD/ReenbitChat.git
cd src/ReenbitChat.Web
dotnet restore
dotnet ef database update
dotnet run


Runs on: https://localhost:7131

Frontend
cd src/ReenbitChat.Web/ClientApp
npm install
ng serve -o


Runs on: http://localhost:4200
```
🧪 Test Task Summary
Requirement	Status
✅ Real-time chat with SignalR	✔ Done
✅ Azure Web App + deployment	✔ Done
✅ Data stored in Azure SQL	✔ Done
✅ Optional Sentiment Analysis	✔ Implemented (Mock / Azure ready)
✅ UI enhancements (color/sentiment)	✔ Done
✅ Source code + GitHub + Docs	✔ Done
📂 Repository Structure
src/
 ├── ReenbitChat.Domain
 ├── ReenbitChat.Application
 ├── ReenbitChat.Infrastructure
 └── ReenbitChat.Web
     ├── Controllers
     ├── Hubs
     ├── Services
     └── ClientApp (Angular)

👨‍💻 Author

Vasyl Ukhal
🔗 GitHub: 7LOD

📧 ukhal.vasyl@gmail.com