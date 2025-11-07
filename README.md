💬 Reenbit Chat

Realtime Chat Application — Reenbit .NET Camp Test Task

🚀 Overview

Realtime чат-додаток, створений у межах завдання Reenbit .NET Camp, що дозволяє користувачам:

Підключатися до кімнат (rooms)

Відправляти та отримувати повідомлення в реальному часі

Переглядати історію збережених повідомлень

Додаток побудовано на стеку .NET 8 + Angular + Azure.

🧩 Tech Stack
Backend

ASP.NET Core 8 Web API

Entity Framework Core (SQL Server)

SignalR (In-App / Azure SignalR ready)

Azure SQL Database

Swagger UI

Frontend

Angular 18

TypeScript

Tailwind CSS

SignalR Client (@microsoft/signalr)

Azure Static Web Apps

☁️ Deployment
Service	Platform	Status
Backend API	Azure App Service	✅ Deployed
Database	Azure SQL	✅ Deployed
Frontend	Azure Static Web App	✅ Deployed

API Base URL:
https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net/api

SignalR Hub URL:
https://reenbitchat-server-c0adandqbxdcczbw.westeurope-01.azurewebsites.net/hubs/chat

📖 Features

✅ Реальний час через SignalR
✅ Підтримка кількох кімнат
✅ Збереження історії повідомлень у SQL
✅ Обробка помилок + reconnect
✅ Готовий до деплою в Azure

🧠 Future Improvements

 Додати Sentiment Analysis через Azure Cognitive Services

 Додати авторизацію користувачів

 Розширити управління кімнатами

 Unit-тести (xUnit / Jasmine)

🧑‍💻 How to run locally
# Backend
cd src/ReenbitChat.Web
dotnet run

# Frontend
cd src/ReenbitChat.Web/ClientApp
npm install
npm start