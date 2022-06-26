# Music module

[![Build](https://github.com/is-tech-y24-1/Do-Svyazi.Music/actions/workflows/build.yml/badge.svg)](https://github.com/is-tech-y24-1/Do-Svyazi.Music/actions/workflows/build.yml)
![DotNet](https://img.shields.io/badge/dotnet%20version-net6.0-blue)

## Description
Данный модуль предоставляет доступ к музыкальному контенту:
- Публикация треков, создание плейлистов
- Добавление треков и плейлистов других пользователей в свою медиатеку
- Прослушивание музыкального контента, возможность формировать очереди из треков для прослушивания.

Наш сервис доступен пользователям из модуля [User](https://github.com/is-tech-y24-1/Do-Svyazi.User). Есть возможность добавить уникальный аватар, который будет отображаться только на музыкальной площадке.

Каждый вид контента может быть приватным или общедоступным. Для плейлистов / треков есть возможность добавления обложки. Прослушивание аудио осуществляется прямой отправкой файла с треком.

У нас есть список поддерживаемых форматов:

|Изображение| Трек |
|--------|--------|
|`.jpg`, `.png`, `.jpeg`, `.heic`| `.wav`, `.mp3`, `.aac`, `.ogg`, `.flac`, `.aiff`|

## Stack
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![ASP.NET](https://img.shields.io/badge/ASP.NET%20Core%206%20-blueviolet?style=for-the-badge&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core%206%20-informational?style=for-the-badge&logo=dotnet)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)

## Download project
```
git clone https://github.com/is-tech-y24-1/Do-Svyazi.Music.git
```

## Restore dependencies
```
dotnet restore
```

## Setup project
- Автарки, обложки и треки хранятся в локальной файловой системе. Путь до хранилища контента указывается в [appsettings.json](Source/Server/DS.Music.WebApi/appsettings.json) (секция: `StorageDirectories`)
- **Connection string** для подключения к базе данных конфигурируется также в [appsettings.json](Source/Server/DS.Music.WebApi/appsettings.json) (секция: `ConnectionStrings`)
- Логирование конфигурируется в `NLog.config`. Данный файл лежит в директории, куда устанавливаются nuget-пакеты, но ссылка на файл с конфигурациями также находится в `DS.Music.WebApi` проекте.


## Start project
```
dotnet run --project Server/DS.Music.WebApi
```

## Run unit tests
```
dotnet test --no-build
```

## License
[MIT](LICENSE)
