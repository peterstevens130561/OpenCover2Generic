# Readme
## Abstract
OpenCoverWrapper.Console is a wrapper around opencover which provides:
1. Running unit test assemblies in parallel
2. Timeout functionality at assembly level
3. Delivers coverage and results in generic format; SQ OpenCover does not support branches


## Example

The following PowerShell example shows how the wrapper will be used

``
function RunUnitTests($CoverageFile,$TestResultsFile) {
    $VSTestExe="$($env:VSSDK140Install)..\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

    $UnitTests=Get-ChildItem  -Path "$(Get-Location)" -Exclude "*obj*" -Filter  "*.UnitTest.dll" -Recurse | ?{ $_.fullname -match "\\bin\\?" }
    $UnitTestsParameter= "-testassembly:" + ($UnitTests -join ",")
    $TargetArgs='/InIsolation /Platform:X64 /TestCaseFilter:""TestCategory=UnitTest|TestCategory=MM|TestCategory=HDF5|TestCategory=ESIEDECODE|TestCategory=Fwdb"" /Logger:VsTestSonarQubeLogger'
    $OpenCover="$env:LOCALAPPDATA\Apps\OpenCover\OpenCover.Console.exe"
    $Wrapper="E:\Program Files\Tools\OpenCoverWrapper.Console.exe"
    echo "Starting tests"
    &$Wrapper "$UnitTestsParameter" -chunk:2 -jobtimeout:15 -parallel:6 -opencover:$OpenCover -output:"$CoverageFile" "-target:$VSTestExe" "-targetargs:$TargetArgs"  "-testresults:$TestResultsFile"

}``

## Command line arguments
### General
Command line arguments always start with `-` when the argument expects a value the argument ends with `:` and is immediately followed with the value
for example
```
-chunks:2
```
The arguments are case insensitive

`<list...>` is a sequence of something. Seperated by `,`, or by repeating the argument
`<path>` path to a file


### Mandatory
`-testassembly:<list of paths>` Is the list of paths to the assemblies containing the tests. The paths may be absolute and relative
`-opencover:<path>` Is the path to OpenCover.Console.Exe
`-target:<path>` See opencover, path to VsTest.Console.Exe
`-targetargs:<string>` See opencover, arguments to pass on to -target, mind escaping the "
`-testresults:<path>` Path to use for the testresults
`-output:<path>` Path for the coverage results
### Options
`-parallel:<int>` Number of jobs to run in parallel. By default 1. Select a number lower than the number of available cores.
`-jobtimeout:<int>` Timeout in minutes of individual jobs, by default no timeout. If a job has a timeout then the wrapper will exit with code 1, 
and will write the failing assembly on the console
`-chunk:<int>` number of assemblies in one job, by default 1. In cases where there are many small assemblies, then the time for OpenCover and VsTest to start
may impact the overall duration, in that case you may want to use this.


## Notes
The wrapper requires the SonarQubeLogger to be installed on system. In a later release the logger will be included in the
wrapper.


