$SonarMsBuildRunner="MSBuild.SonarQube.Runner.Exe"
$MSBuild="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$BuildWrapper="build-wrapper.exe"
$OpenCover="C:/users/stevpet/AppData/Local/Apps/OpenCover/OpenCover.Console.exe"
$VsTestConsole=
$Repository="http://bhihoutfsapp:8086/tfs"


$CppOutDir=Join-Path -Path "$(Get-Location)" -ChildPath .cppoutdir
$files=ls "*.sln"


function CleanUpEncoding($Path) {
    function ConvertANSI2UTF8($Path) {
        function Is-UTF8($Path)
        {
            [byte[]]$byte = get-content -Encoding byte -ReadCount 4 -TotalCount 4 -Path $Path
            $result= $byte[0] -eq 0xef -and $byte[1] -eq 0xbb -and $byte[2] -eq 0xbf
            return $result
        }

        $attributes=(Get-Item -Path $Path).Attributes
        (Get-Item -Path $Path).Attributes = "Normal"
        if(((Get-Item $Path).Length -gt 4 ) -and !(Is-UTF8 -Path $Path)) {
            $lines=Get-Content -Path $Path -Encoding Ascii
            $lines | Out-File -FilePath $Path -Encoding utf8
            echo "Converted $Path"
       
        }
        (Get-Item -Path $Path).Attributes = $attributes
    }

    Get-ChildItem -Path $Path -Filter "*.cs" -Recurse -File | ForEach-Object {
        ConvertANSI2UTF8 -Path $_.FullName
    }
}

function RunUnitTests($CoverageFile,$TestResultsFile) {
    $VSTestExe="$($env:VSSDK140Install)..\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

    $UnitTests=Get-ChildItem  -Path "$(Get-Location)" -Exclude "*obj*" -Filter  "*.UnitTest.dll" -Recurse | ?{ $_.fullname -match "\\bin\\?" }
    $TargetArgs="/InIsolation /Platform:X64 /TestCaseFilter:""TestCategory=UnitTest|TestCategory=MM"" /Logger:VsTestSonarQubeLogger $($UnitTests)"
    $OpenCover="$env:LOCALAPPDATA\Apps\OpenCover\OpenCover.Console.exe"
    &$OpenCover -output:"$CoverageFile" -register:user "-target:$VSTestExe" "-targetargs:$TargetArgs"  >../testresults.log
    $TestResultsXml=((Get-Content -Path ../testresults.log | Select-String "VsTestSonarQubeLogger.TestResults") -csplit "=")[1] 
    mv $TestResultsXml $TestResultsFile
}


$files | ForEach-Object {
    $Solution=Get-Item $_

    cd $Solution.Directory.FullName
    CleanUpEncoding -Path .
    $ProjectName=$Solution.Name -replace ".sln",""
    $Key="Tool-${ProjectName}"
    $TestResultsFile="$(Get-Location)\.sonarqube\testresults.xml"
    $CoverageFile="$(Get-Location)\.sonarqube\opencover.xml"
    &$SonarMsBuildRunner begin /k:$Key /v:"main" /n:"$ProjectName"  /d:sonar.scm.forceReloadAll=true /d:sonar.scm.provider=git  /d:sonar.cfamily.build-wrapper-output=$CppOutDir /d:sonar.visualstudio.solution=$_.Name /d:sonar.resharper.mode=skip /d:sonar.cs.opencover.reportsPaths=$CoverageFile /d:sonar.genericcoverage.unitTestReportPaths="$TestResultsFile"
    &${BuildWrapper} --out-dir  $CppOutDir $MSBuild $Solution.Name /t:Rebuild /v:q
    RunUnitTests -CoverageFile $CoverageFile -TestResultsFile $TestResultsFile
    &$SonarMsBuildRunner end 

}




