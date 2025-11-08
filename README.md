💬 Reenbit Chat
(SignalR + .NET 8 + Angular 18 + Azure)

Reenbit Chat — це реальний чат-додаток із підтримкою SignalR, Sentiment Analysis та Azure SQL.
Проєкт виконано як тестове завдання для Reenbit Trainee Camp: Back-End Development (.NET).

🚀 Live Demo

🔹 Frontend (Angular): https://victorious-glacier-082ff5403.3.azurestaticapps.net

🔹 Backend (Web API): https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net

🔹 Swagger: /swagger

⚠️ Note:
Backend (API) розгорнуто на Azure App Service з Azure SQL (Free Tier).
Якщо API тимчасово недоступний (404 або timeout) — зачекайте 20–30 секунд і оновіть сторінку.
Це нормальна поведінка безкоштовного тарифу Azure.

🧠 Features
💬 Real-time Chat

Побудовано на ASP.NET Core SignalR

Користувачі можуть приєднуватись до кімнат та обмінюватися повідомленнями в реальному часі

Системні повідомлення про приєднання користувачів

😊 Sentiment Analysis (optional)

Інтеграція з Azure Cognitive Services Text Analytics API

Кожне повідомлення класифікується як Positive, Neutral або Negative

Повідомлення підсвічуються в UI відповідним кольором

🗄️ Data Storage

Зберігання повідомлень у Azure SQL Database

Використано EF Core + Code First Migrations

🎨 UI (Angular + TailwindCSS)

Мінімалістичний адаптивний інтерфейс

Реальні оновлення через SignalR

Автопрокрутка до нових повідомлень

Візуальне виділення настрою повідомлень

☁️ Deployment

Backend: Azure App Service (.NET 8)

Frontend: Azure Static Web Apps

Database: Azure SQL

🧩 Technologies
Layer	Stack
Backend	.NET 8, ASP.NET Core, SignalR, EF Core, Azure Cognitive Services
Frontend	Angular 18, TypeScript, TailwindCSS
Database	Azure SQL
Hosting	Azure App Service, Azure Static Web Apps
👨‍💻 Author

Vasyl Ukhal
📧 ukhal.vasyl@gmail.com

🔗 GitHub: 7LOD