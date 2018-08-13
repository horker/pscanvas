#requires -PSEdition Desktop

Set-StrictMode -Version 4

# Open an invisible root window to keep a message loop in the different thread

[Horker.Canvas.WpfWindow]::OpenRootWindow()

# Initialize the user settings variable

$settings = New-Object Horker.Canvas.UserSettings

$global:PSCANVAS_SETTINGS = $settings
[Horker.Canvas.UserSettings]::Default = $settings
