$binDirs = Get-ChildItem -Recurse * | ? { $_.PSIsContainer} | % { $_.FullName} `
 | Select-String bin

$objDirs = Get-ChildItem -Recurse * | ? { $_.PSIsContainer} | % { $_.FullName} `
 | Select-String obj

$dirs = $binDirs + $objDirs
foreach ($dir in $dirs) {
    #echo $dir
    Remove-Item -Recurse $dir
}
