$keyboard = (Get-WmiObject -Class Win32_Keyboard).Status 
Write-Output "{keyboard: $keyboard"

$monitor = (Get-WmiObject -Class Win32_DesktopMonitor).Status
Write-Output "monitor: $monitor"

$mouse = (Get-WmiObject -Class Win32_PointingDevice).Status
Write-Output "mouse: $mouse}"