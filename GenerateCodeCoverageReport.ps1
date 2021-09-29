
<#
.SYNOPSIS
Run the reportgenerator tool to combine and generate a code coverage report
#>

Set-Variable -Name ReportPath -Value ./CodeCoverage/Report

if(Test-Path $ReportPath) { Remove-Item -Path CodeCoverage/Report -Recurse }
reportgenerator -reports:./**/coverage.cobertura.xml -targetdir:./CodeCoverage/Report -reporttypes:HtmlInline_AzurePipelines