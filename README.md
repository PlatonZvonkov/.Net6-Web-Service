# .Net6-Web-Service
Real-world life mailing microservice that was developed during my time in SULPAK

Задача: Web-сервис для формирования и отправки писем адресатам и логирование результата в реляционную БД.

Решение: ASP.Net Core, .Net6, EFCore, Automapper, MailKit, SQLite, xUnit, Moq, Swagger

Сервис принимает по адресу url:\\api\mails, либо POST куда можно послать тело запроса в виде 
  
{  
subject:"string",  
body:"string",  
recipients:["string"]   
}
  
либо GET и выдаёт лог попыток послать письма, успешных и неудачных.

Для проверки в конфиге настроен SMTP etherial.email, номожно добавить любой адрес почты и настройки email сервера.
Также можно попробовать интеграционные тесты для проверки работы контроллеров и слоя подключения к базе.
