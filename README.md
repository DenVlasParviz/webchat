# Real-time WebChat

Веб-додаток для чату в реальному часі. Підтримує аутентифікацію користувачів, обмін миттєвими повідомленнями та збереження історії чатів.

---

## Зміст
- [Огляд](#огляд)
- [Функціонал](#функціонал)
- [Вимоги](#вимоги)
- [База даних](#база-даних-та-примітки)
- [Скріншоти](#скріншоти)


---

## Огляд
Бекенд побудовано на **ASP.NET Core** з використанням **SignalR** для двостороннього зв'язку в реальному часі та **ASP.NET Core Identity** для аутентифікації. Фронтенд розроблено на **Vue.js**. Усі дані зберігаються в базі даних **PostgreSQL**.

## Функціонал
- Реєстрація/логін користувачів (за допомогою ASP.NET Core Identity).
- Відправлення та отримання миттєвих повідомлень в реальному часі.
- Зберігання історії чатів у базі даних PostgreSQL.
- Відображення списку чатів.



## Вимоги
- Node.js >= 16  
- npm 
- .NET  
- PostgreSQL

## Локальна інсталяція та запуск
1. Встановити залежності:
```bash
# сервер
cd server
npm install

# клієнт
cd ../client
npm install
```
Оновіть файл `appsettings.json` з відповідними даними:
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

## База даних та примітки
- PostgreSQL використовується для зберігання даних про користувачів (з Identity) та історії повідомлень
- Щоб отримувати останнє повідомлення, потрібно створити view в Views
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

## Скріншоти

<img width="1905" height="879" alt="firefox_a8HUFAs5d6" src="https://github.com/user-attachments/assets/bc02af67-5483-45a1-9ea9-000fa7c095f2" />

