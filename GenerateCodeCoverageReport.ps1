
<#
.SYNOPSIS
Run the reportgenerator tool to combine and generate a code coverage report
#>

param (
    # Path to test results xml file
    [Parameter(Mandatory, Position=0)]
    [string]
    $TestFilePath,

    # Path to result folder
    [Parameter(Mandatory, Position=1)]
    [String]
    $ResultFilePath
)


if(Test-Path $ResultFilePath) { Remove-Item $ResultFilePath -Recurse }
Write-Output "Executing from path $PWD"
reportgenerator -reports:$TestFilePath -targetdir:$ResultFilePath -reporttypes:HtmlInline_AzurePipelines