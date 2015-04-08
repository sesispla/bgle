$path = Split-Path -Parent $MyInvocation.MyCommand.Path

$files = Get-ChildItem -Path "$path\.." -Filter *.csproj -Recurse
foreach($f in $files){
	$name = $f.name
	$directory = $f.Directory
	Copy-Item "$path\sample.csproj.nuspec" "$directory\$name.nuspec"
}