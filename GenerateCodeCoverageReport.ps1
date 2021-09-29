
<#
.SYNOPSIS
Run the reportgenerator tool to combine and generate a code coverage report
#>

Set-Variable -Name ReportPath -Value ./CodeCoverage/Report

if(Test-Path $ReportPath) { Remove-Item $ReportPath -Recurse }
reportgenerator -reports:./**/*cobertura.xml -targetdir:$ReportPath -reporttypes:HtmlInline_AzurePipelines