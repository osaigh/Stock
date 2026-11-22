<#
.SYNOPSIS
    Builds and runs the Stock Trader application locally.

.DESCRIPTION
    Starts Stock.IdentityServer, Stock.API and Stock.MarketUpdater together with the client(s) you choose,
    waits until they are ready and opens the web client in your browser.
    Press Ctrl+C to stop everything again. The output of every process is written to the logs folder.

    Sign in with the test account: username 'bob', password 'password'.

.PARAMETER Client
    The client(s) to start: React (default), Angular, Wpf or None. Several can be given, separated by commas.

.PARAMETER NoBrowser
    Do not open the web client in the browser.

.PARAMETER NoBuild
    Skip building the .NET projects (use this when they were just built).

.EXAMPLE
    .\run.ps1
    Runs the servers and the React client.

.EXAMPLE
    .\run.ps1 -Client Angular,Wpf
    Runs the servers, the Angular client and the WPF client.
#>
[CmdletBinding()]
param(
    [string[]]$Client = @('React'),
    [switch]$NoBrowser,
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

$root = $PSScriptRoot
$logDirectory = Join-Path $root 'logs'

$identityServerUrl = 'http://localhost:44702'
$apiUrl = 'http://localhost:44700'
$webClients = @{
    React   = @{ Directory = 'Stock.WebClient\ClientApp'; Url = 'http://localhost:44602'; Port = 44602 }
    Angular = @{ Directory = 'Stock.WebClientAngular\ClientApp'; Url = 'http://localhost:44902'; Port = 44902 }
}

# "-Client React,Angular" arrives as one string when the script is started through run.cmd
$clients = @($Client | ForEach-Object { $_ -split ',' } | ForEach-Object { $_.Trim() } | Where-Object { $_ })
foreach ($name in $clients) {
    if ($name -notin 'React', 'Angular', 'Wpf', 'None') {
        throw "Unknown client '$name'. Use React, Angular, Wpf or None."
    }
}
$selectedWebClients = @('React', 'Angular' | Where-Object { $clients -contains $_ })
$runWpfClient = $clients -contains 'Wpf'

$started = New-Object System.Collections.Generic.List[object]

function Write-Step([string]$message) {
    Write-Host "==> $message" -ForegroundColor Cyan
}

function Assert-Command([string]$name, [string]$hint) {
    if (-not (Get-Command $name -ErrorAction SilentlyContinue)) {
        throw "'$name' was not found. $hint"
    }
}

function Test-PortInUse([int]$port) {
    $listeners = [System.Net.NetworkInformation.IPGlobalProperties]::GetIPGlobalProperties().GetActiveTcpListeners()
    return [bool]($listeners | Where-Object { $_.Port -eq $port })
}

function Start-App([string]$name, [string]$filePath, [string[]]$arguments, [string]$workingDirectory) {
    $log = Join-Path $logDirectory "$name.log"
    $errorLog = Join-Path $logDirectory "$name.error.log"
    $process = Start-Process -FilePath $filePath -ArgumentList $arguments -WorkingDirectory $workingDirectory `
        -RedirectStandardOutput $log -RedirectStandardError $errorLog -NoNewWindow -PassThru
    $app = [pscustomobject]@{ Name = $name; Process = $process; Log = $log; ErrorLog = $errorLog }
    $started.Add($app)
    return $app
}

function Get-LogTail($app) {
    $lines = @()
    foreach ($file in $app.Log, $app.ErrorLog) {
        if (Test-Path $file) { $lines += Get-Content $file -Tail 25 }
    }
    $lines = $lines | ForEach-Object { if ($_.Length -gt 300) { $_.Substring(0, 300) + '...' } else { $_ } }
    return ($lines -join [Environment]::NewLine)
}

function Wait-ForUrl($app, [string]$url, [int]$timeoutSeconds = 120) {
    $deadline = (Get-Date).AddSeconds($timeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        if ($app.Process.HasExited) {
            throw "$($app.Name) stopped unexpectedly. Last output:$([Environment]::NewLine)$(Get-LogTail $app)"
        }
        try {
            Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 5 | Out-Null
            return
        }
        catch {
            Start-Sleep -Milliseconds 500
        }
    }
    throw "$($app.Name) did not answer on $url within $timeoutSeconds seconds. See $($app.Log)"
}

function Stop-Apps {
    foreach ($app in $started) {
        if (-not $app.Process.HasExited) {
            # /T also stops the child processes (dotnet run and npm both start one)
            & taskkill.exe /PID $app.Process.Id /T /F 2>&1 | Out-Null
        }
    }
}

try {
    # Prerequisites
    Assert-Command 'dotnet' 'Install the .NET 10 SDK from https://dotnet.microsoft.com/download'
    Push-Location $root
    try {
        $sdkVersion = (& dotnet --version 2>&1 | Out-String).Trim()
        if ($LASTEXITCODE -ne 0) {
            throw "The .NET 10 SDK is required (see global.json). Install it from https://dotnet.microsoft.com/download$([Environment]::NewLine)$sdkVersion"
        }
    }
    finally {
        Pop-Location
    }
    if ($selectedWebClients) {
        Assert-Command 'npm' 'Install Node.js (which includes npm) from https://nodejs.org/'
    }

    $ports = @(44700, 44702) + @($selectedWebClients | ForEach-Object { $webClients[$_].Port })
    if ($runWpfClient) { $ports += 44801 }
    $portsInUse = @($ports | Where-Object { Test-PortInUse $_ })
    if ($portsInUse) {
        throw "Port(s) $($portsInUse -join ', ') are already in use. Is the application already running?"
    }

    New-Item -ItemType Directory -Force -Path $logDirectory | Out-Null

    # Build
    $projects = @('Stock.IdentityServer', 'Stock.API', 'Stock.MarketUpdater')
    if ($runWpfClient) { $projects += 'Stock.WpfClient.Desktop' }

    if (-not $NoBuild) {
        foreach ($project in $projects) {
            Write-Step "Building $project"
            & dotnet build (Join-Path $root $project) --nologo --verbosity quiet
            if ($LASTEXITCODE -ne 0) { throw "Building $project failed." }
        }
    }

    foreach ($name in $selectedWebClients) {
        $directory = Join-Path $root $webClients[$name].Directory
        if (-not (Test-Path (Join-Path $directory 'node_modules'))) {
            Write-Step "Installing the npm packages of the $name client (this can take a few minutes)"
            Push-Location $directory
            try {
                & npm install --no-audit --no-fund
                if ($LASTEXITCODE -ne 0) { throw "npm install failed for the $name client." }
            }
            finally {
                Pop-Location
            }
        }
    }

    # Servers
    Write-Step "Starting Stock.IdentityServer on $identityServerUrl"
    $identityServer = Start-App 'Stock.IdentityServer' 'dotnet' @('run', '--no-build', '--project', 'Stock.IdentityServer') $root

    Write-Step "Starting Stock.API on $apiUrl"
    $api = Start-App 'Stock.API' 'dotnet' @('run', '--no-build', '--project', 'Stock.API') $root

    Wait-ForUrl $identityServer "$identityServerUrl/.well-known/openid-configuration"
    Wait-ForUrl $api "$apiUrl/swagger/v1/swagger.json"

    Write-Step 'Starting Stock.MarketUpdater'
    $marketUpdater = Start-App 'Stock.MarketUpdater' 'dotnet' @('run', '--no-build', '--project', 'Stock.MarketUpdater') $root
    $servers = @($identityServer, $api, $marketUpdater)

    # Clients
    foreach ($name in $selectedWebClients) {
        $webClient = $webClients[$name]
        Write-Step "Starting the $name client on $($webClient.Url)"
        $app = Start-App "$name-client" 'cmd.exe' @('/c', 'npm', 'start') (Join-Path $root $webClient.Directory)
        Wait-ForUrl $app $webClient.Url 180
    }

    $wpfClient = $null
    if ($runWpfClient) {
        Write-Step 'Starting the WPF client'
        $wpfClient = Start-App 'Stock.WpfClient.Desktop' 'dotnet' @('run', '--no-build', '--project', 'Stock.WpfClient.Desktop') $root
    }

    Write-Host ''
    Write-Host 'Stock Trader is running.' -ForegroundColor Green
    Write-Host "  Identity server : $identityServerUrl"
    Write-Host "  API (Swagger)   : $apiUrl/swagger"
    foreach ($name in $selectedWebClients) {
        Write-Host ("  {0,-16}: {1}" -f "$name client", $webClients[$name].Url)
    }
    if ($runWpfClient) {
        Write-Host '  WPF client      : started in its own window'
    }
    Write-Host "  Logs            : $logDirectory"
    Write-Host ''
    Write-Host "Sign in with username 'bob' and password 'password'."
    Write-Host 'Press Ctrl+C to stop.' -ForegroundColor Yellow

    if (-not $NoBrowser) {
        foreach ($name in $selectedWebClients) {
            Start-Process $webClients[$name].Url
        }
    }

    # Keep running until Ctrl+C or until one of the processes stops by itself.
    # Closing the WPF client does not stop the servers.
    while ($true) {
        Start-Sleep -Seconds 1
        $stopped = @($started | Where-Object { $_ -ne $wpfClient -and $_.Process.HasExited })
        if ($stopped) {
            # Ctrl+C reaches every process in this console, so when all the servers stop together it is a normal stop
            Start-Sleep -Seconds 2
            if (-not @($servers | Where-Object { -not $_.Process.HasExited })) {
                break
            }

            $app = $stopped[0]
            throw "$($app.Name) stopped unexpectedly. Last output:$([Environment]::NewLine)$(Get-LogTail $app)"
        }
    }
}
catch {
    Write-Host ''
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
finally {
    if ($started.Count -gt 0) {
        Write-Host ''
        Write-Host 'Stopping...' -ForegroundColor Yellow
        Stop-Apps
    }
}
