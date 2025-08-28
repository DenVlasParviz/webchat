# Real-time WebChat

Web application for real-time chat. Supports user authentication, instant messaging, and chat history storage.

---

## Contents
- [Overview](#overview)
- [Requirements](#requirements)
- [Database](#database-and-notes)
- [Screenshots](#screenshots)

---

## Overview
The backend is built with **ASP.NET Core** using **SignalR** for real-time bidirectional communication and **ASP.NET Core Identity** for authentication. The frontend is developed with **Vue.js**. All data is stored in a **PostgreSQL** database.

## Requirements
- Node.js >= 16  
- npm  
- .NET  
- PostgreSQL

## Local installation and running
1. Install dependencies:
```bash
# server
cd server
npm install

# client
cd ../client
npm install
```
Update the `appsettings.json` file with the appropriate values:
```json
{
  "ConnectionStrings": {
    "ChatDatabase": "Host=localhost;Port=5432;Database=webchatdb;Username=your_user;Password=your_password"
  },
  "JwtSettings": {
    "Key": "your_super_secret_jwt_key"
  }
}
```
---

## Database and notes
- PostgreSQL is used to store user data (with Identity) and message history  
- To get the latest message, you need to create a view in Views:
```bash
CREATE OR REPLACE VIEW "LastMessages" AS
SELECT m1."ConversationId", m1."Id", m1."SenderId", m1."Text", m1."Timestamp"
FROM "Messages" m1
WHERE m1."Timestamp" = (
    SELECT MAX(m2."Timestamp")
    FROM "Messages" m2
    WHERE m2."ConversationId" = m1."ConversationId"
);
``` 

## Screenshots

<img width="1905" height="879" alt="firefox_a8HUFAs5d6" src="https://github.com/user-attachments/assets/bc02af67-5483-45a1-9ea9-000fa7c095f2" />
