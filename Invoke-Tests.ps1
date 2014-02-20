# Include test functions and setup
. .\Test-Setup.ps1

# Init
Clear-Host

TestSuite {
    # Tests
    TestFixture "When querying a single item" {
        GET "/item?query=/sitecore/content/home" 200 "All 'min' properties must be valid when payload is unspecified" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.TemplateName -eq "Sample Item" } | 
            Assert { $_.Key -eq "home" } | 
            Assert { $_.Path -eq "/sitecore/content/Home" } | 
            Assert { $_.HasChildren -eq $false} | Out-Null
        
        GET "/item?query={110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}" 200 "All 'min' properties must be valid when quering by ID" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.TemplateName -eq "Sample Item" } | 
            Assert { $_.Key -eq "home" } | 
            Assert { $_.Path -eq "/sitecore/content/Home" } | 
            Assert { $_.HasChildren -eq $false} | Out-Null

        GET "/item?query=/sitecore/content" 200 "HasChildren must be true when child items are present" | 
            Assert { $_.HasChildren -eq $true} | Out-Null
    
        GET "/item?query=/sitecore/system/languages/en&fields=__security" 200 "All 'min' propties must exist while also specifing specific fields" | 
            Assert { $_.Key -eq "en" } | 
            Assert { $_.Path -match "/sitecore/system/languages/en" } | 
            Assert { $_.Fields.Length -eq 1} |
            Assert { $_.Fields[0].Key -eq "__security" -and $_.Fields[0].Value -eq ""} | Out-Null

        GET "/item?query=/sitecore/content/home&payload=custom&fields=title" 200 "Using 'custom' payload, no other fields are present than the specified" | 
            Assert { $_.Id -eq $null } |
            Assert { $_.ParentId -eq $null } |
            Assert { $_.TemplateId -eq $null } |
            Assert { $_.TemplateName -eq $null } | 
            Assert { $_.Key -eq $null } | 
            Assert { $_.Path -eq $null } | 
            Assert { $_.HasChildren -eq $null} |
            Assert { $_.Fields[0].Value -eq "Sitecore"} | Out-Null

        GET "/item?query=/sitecore/content/home&payload=full" 200 "Using 'full' payload, all properties and non-system fields must be valid" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.TemplateName -eq "Sample Item" } | 
            Assert { $_.Key -eq "home" } | 
            Assert { $_.Path -eq "/sitecore/content/Home" } | 
            Assert { $_.HasChildren -eq $false} |
            Assert { $_.Fields.Length -eq 2} | Out-Null
    }

    TestFixture "When querying multiple items" {
        GET "/item?query=/sitecore/system/toolbox/*" 200 "All 'min' properties must be vaild" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.Path -match "/sitecore/system/toolbox/" } | Out-Null

        GET "/item?query=/sitecore/system/toolbox//*" 200 "All recursive items must have valid 'min' properties" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.Path -match "/sitecore/system/toolbox/" } | Out-Null

        GET "/item?query=/sitecore/content/home,/sitecore/system/languages/en" 200 "Two batches must have valid 'min' properties" | 
            Assert { $_.Id -ne $null } |
            Assert { $_.ParentId -ne $null } |
            Assert { $_.TemplateId -ne $null } |
            Assert { $_.TemplateName -ne $null } | 
            Assert { $_.Key -ne $null } | 
            Assert { $_.Path -match "/sitecore/"} | Out-Null
    }
    
    TestFixture "When querying a single item that does not exist" {
        GET "/item?query=/sitecore/content/test" 404 "Should be 404" | Out-Null
     }   
}