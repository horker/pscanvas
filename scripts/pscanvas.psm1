#requires -PSEdition Desktop

Set-StrictMode -Version 4

[Horker.Canvas.WpfWindow]::OpenRootWindow()

$settings = New-Object Horker.Canvas.UserSettings

$global:PSCANVAS_SETTINGS = $settings
[Horker.Canvas.UserSettings]::Default = $settings
