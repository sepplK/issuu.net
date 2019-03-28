@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

dotnet build issuu.sln -c Release

%nuget% pack "Client.nuspec" -NoPackageAnalysis -verbosity detailed -Version %version% -p Configuration="%config%"