# Setup
$api = "http://localhost:64444/api/data/v1"

Set-Variable -Name "assertssucceeded" -Value 0 -Scope Script -Force
Set-Variable -Name "assertsfailed" -Value 0 -Scope Script -Force
Set-Variable -Name "assertsdetailed" -Value $false -Scope Script -Force

function Get
{
    param(
        [Parameter(Position=0, Mandatory=$true)]
	    [string]$url,
        [Parameter(Position=1, Mandatory=$true)]
	    [int]$expectedStatusCode,
        [Parameter(Position=2, Mandatory=$true)]
	    [string]$name
    )
    
    $uri = "$api$url"
    
    Write-Host "`tTest " -NoNewline -ForegroundColor Gray
    Write-Host "'$name'" -NoNewline -ForegroundColor Cyan
    Write-Host " using 'GET $url'" -ForegroundColor Gray

    $raw = Invoke-WebRequest -Method Get -Uri "$uri"

    $statusCode = $raw.StatusCode

    if($statusCode -eq $expectedStatusCode) {
        $json = $raw.Content | ConvertFrom-Json
    
        Write-Output $json
    } 
    else
    {
        Write-Host "`tERROR: '$uri' returned '$statusCode' but '$expectedStatusCode' was excepted." -ForegroundColor Red
    }
}

function Assert
{  
    param(
        [Parameter(Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [ScriptBlock]$Expression,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [psobject]$input
    )

    process
    {  
        $res = $Expression.Invoke()
        
        if($res -eq $true)
        {
            Set-Variable -Name "assertssucceeded" -Value ($assertssucceeded + 1) -Scope Script -Force
            
            if($assertsdetailed)
            {      
                if($_.Path -ne $null -and $_.Id -ne $null)
                {
                    Write-Host "`t`tTesting Path '$($_.Path)', Id '$($_.Id)'" -ForegroundColor Gray
                }

                Write-Host "`t`tExpression >> $Expression << evaluated as $res." -ForegroundColor Green
            }
        }
        else
        {
            Set-Variable -Name "assertsfailed" -Value ($assertsfailed + 1) -Scope Script -Force
            
            Write-Host "`t`tExpression >> $Expression << evaluated as $res." -ForegroundColor Red
            Write-Host "`t`t----------------------------------------------------------------"
            
            Write-Host "`t`tId: $($_.Id)" -ForegroundColor Red
            Write-Host "`t`tKey: $($_.Key)" -ForegroundColor Red
            Write-Host "`t`tPath: $($_.Path)" -ForegroundColor Red
            Write-Host "`t`tTemplateId: $($_.TemplateId)" -ForegroundColor Red
            Write-Host "`t`tTemplateName: $($_.TemplateName)" -ForegroundColor Red
            Write-Host "`t`tHasChildren: $($_.HasChildren)" -ForegroundColor Red
            Write-Host "`t`tFields: " -ForegroundColor Red
            
            foreach($field in $_.Fields)
            {
                Write-Host "`t`t`tKey `"$($field.Key)`", Id `"$($field.Id)`", Value `"$($field.Value)`"" -ForegroundColor Red
            }
            
            Write-Host "`t`t----------------------------------------------------------------"

            #Throw (New-Object System.Management.Automation.PipelineStoppedException)
        }

        Write-Output $_
    }
}

function TestSuite {
    param(
        [Parameter(Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [ScriptBlock]$Expression,
        [Parameter(Position=1, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [switch]$ShowDetailedAssertions
    )

    begin 
    {
        if($ShowDetailedAssertions)
        {
            Set-Variable -Name "assertsdetailed" -Value $true -Scope Script -Force
        }

        $ticks = 0
    }
    process
    {
       $timer = Measure-Command { $Expression.Invoke() }
       $ticks = $timer.TotalSeconds
    }
    end
    {
        if($assertsfailed -eq 0)
        {
            Write-Host "`nTest completed in $ticks seconds, $assertssucceeded asserts succeeded." -ForegroundColor Green
            
            Exit 0
        }
        else
        {
            Write-Host "`nTest failed in $ticks seconds, $assertsfailed asserts failed and $assertssucceeded succeeded." -ForegroundColor Red
            
            Exit 1           
        }
     }
}

function TestFixture {
    param(
        [Parameter(Position=0, Mandatory=$true)]
	    [string]$name,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [ScriptBlock]$Expression
        
    )

    begin 
    {            
        Write-Host "Fixture " -NoNewline -ForegroundColor Gray
        Write-Host "'$name'" -NoNewline -ForegroundColor Cyan
        Write-Host ":" -ForegroundColor Gray
        $ticks = 0
    }
    process
    {
       $timer = Measure-Command { $Expression.Invoke() }
       $ticks = $timer.TotalSeconds
    }
    end
    {
        Write-Host "Fixture completed in $ticks seconds" -ForegroundColor Gray
    }
}