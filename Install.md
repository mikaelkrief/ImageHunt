# Installation on Linux
## WebApi configuration
1. Install MySql (or MariaDb) server and create an empty database. Create a service user on the database and give all rights.
2. You need OAuth certificate (Google)
3. Create `appsettings.Production.json` like that: 


       {
         "ConnectionStrings": {
           "DefaultConnection": "Server=localhost;Database=ImageHunt;Uid=<service_db_user>;Pwd=<Service_db_password>;"
         },
         "GoogleApi": {
           "ClientId": "<google_client_id>",
           "ClientSecret": "<google_client_secret>"
         },
         "Telegram": {
           "APIKey": "<telegram_bot_token>"
         },
         "BotConfiguration": {
           "BotToken": "<telegram_bot_token>"
        },
         "ImageHuntApi": {
           "Url": "http://localhost:<webapi_listening_port>"
         } 
       }
## NgInx configuration
Open the file `/etc/nginx/site-available/default`
### Website & WebAPI
    server {
        listen 80;
        server_name <your_server_name>;
        index index.html;

        location /api {
            proxy_pass http://localhost:43454;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Real-IP  $remote_addr;
            proxy_set_header X-Forwarded-For $remote_addr;
        }
        location / {
            root <path of front end files>;
            try_files $uri $uri/ /index.html;
        }
    }
### Chatbot
    server {
        server_name <your_bot_server_name>;
        listen 80;

        location / {
            proxy_pass http://localhost:5500/api/messages;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Real-IP  $remote_addr;
            proxy_set_header X-Forwarded-For $remote_addr;
        }
    }

Since telegram webhook use https, you should modify your configuration. Use `Certbot` for that.

Test your nginx file:
`nginx -t`

Reload nginx config:
`nginx -s reload`

## Kestrel configuration
### Website and REST API
Open /etc/systemd/system/kestrel-ImageHunt.service

    [Unit]
    Description=ImageHunt server

    [Service]
    ExecStart=/usr/bin/dotnet /opt/ImageHunt/ImageHunt.dll
    WorkingDirectory=/opt/ImageHunt/
    Restart=always
    RestartSec=10                                          # Restart service after 10 seconds if dotnet service cras$
    SyslogIdentifier=ImageHuntApi
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=ASPNETCORE_URLS=http://localhost:43454

    [Install]
    WantedBy=multi-user.target

### Chatbot
Open /etc/systemd/system/kestrel-ImageHuntBot.service
    [Unit]
    Description=ImageHuntBot server

    [Service]
    ExecStart=/usr/bin/dotnet /opt/ImageHuntBot/ImageHuntBot.dll
    WorkingDirectory=/opt/ImageHuntBot/
    Restart=always
    RestartSec=10                                          # Restart service after 10 seconds if dotnet service cras$
    SyslogIdentifier=ImageHuntBot
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=ASPNETCORE_URLS=http://localhost:5500
