Param(
[string]$InFile,
[string]$OutFile,
[string]$DBPASSWORD,
[string]$GOOGLEAPI_CLIENT_ID,
[string]$GOOGLEAPI_CLIENT_SECRET,
[string]$TELEGRAM_APIKEY
)
$dir = $MyInvocation.MyCommand.Path
$dir = $dir.SubString(0, $dir.LastIndexOf("\") + 1)
$content = [System.IO.File]::ReadAllText($dir + $InFile);
$content = $content.replace('DBPASSWORD', $DBPASSWORD);
$content = $content.replace('GOOGLEAPI_CLIENT_ID', $GOOGLEAPI_CLIENT_ID);
$content = $content.replace('GOOGLEAPI_CLIENT_SECRET', $GOOGLEAPI_CLIENT_SECRET);
$content = $content.replace('TELEGRAM_APIKEY', $TELEGRAM_APIKEY);

[System.IO.File]::WriteAllText($dir + $OutFile, $content);
