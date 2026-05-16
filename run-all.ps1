$PSScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$services = @(
    "MediBook.AuthService\MediBook.AuthService",
    "MediBook.AppointmentService",
    "MediBook.ProviderService",
    "MediBook.AvailabilityService",
    "MediBook.NotificationService",
    "MediBook.PaymentService",
    "MediBook.RecordService",
    "MediBook.ReviewService"
)

foreach ($service in $services) {
    $projectPath = Join-Path $PSScriptRoot $service
    Write-Host "Starting $service at $projectPath..." -ForegroundColor Cyan
    if (Test-Path $projectPath) {
        Start-Process dotnet -ArgumentList "run --project `"$projectPath`"" -NoNewWindow
    } else {
        Write-Error "Project path not found: $projectPath"
    }
}

Write-Host "All services started in the background." -ForegroundColor Green
