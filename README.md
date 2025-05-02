# LibraryApp 
Test Task

OUTDATED: Чтобы использовать миграцию ввести в папке LibraryApp:
dotnet ef database update 
(SqlServer)
Миграция происходит автоматически.

Для запуска фронта в /ClientApp прописать в консоль:
npm run dev

Фронт - http://localhost:3000
        http://localhost:3000/login - Авторизоваться 
            Админ: admin@library.com 
                   12345 
            Пользователь: john.doe@example.com 
                          12345 
        http://localhost:3000/register - Регистрация 
        
        
Бэк - http://localhost:7001

Исправил docker-compose файл. Бэк теперь корректно соединяется с базой данных.
docker-compose up --build
