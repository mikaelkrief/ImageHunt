Param(
[string]$InFile,
[string]$OutFile,
[string]$GOOGLE_CLIENT_ID,
[string]$GOOGLE_MAP_API_KEY
)
$dir = $MyInvocation.MyCommand.Path
$dir = $dir.SubString(0, $dir.LastIndexOf("\") + 1)
$content = [System.IO.File]::ReadAllText($dir + $InFile);
$content = $content.replace('GOOGLE_CLIENT_ID', $GOOGLE_CLIENT_ID);
$content = $content.replace('GOOGLEAPI_CLIENT_ID', $GOOGLE_MAP_API_KEY);

[System.IO.File]::WriteAllText($dir + $OutFile, $content);
