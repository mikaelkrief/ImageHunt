az container create --resource-group Pixhint --name pixhinttest --image nimbusparis/pixhintbot:test `
--dns-name-label pixhinttest `
--environment-variables ASPNETCORE_ENVIRONMENT=Test ImageHuntApi_Url=http://pixhint.com `
--location westeurope