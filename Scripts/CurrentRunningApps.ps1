Get-Process | Where-Object {$_.mainWindowTitle} | Format-Table Id, mainWindowtitle -AutoSize
