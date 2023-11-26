# azure-code-coverage
WebApi to provide code-coverage data for azure devops builds.

# Inspiration
I looked for a way to display my code coverage in a widget but I didn't want my code or results sent to some endpoint in the world, so I decided to create my own Api to create me my own badges. The Dotnet7 app can be hostet by yourself so your data is safe as well. Just provide the app with an PAT from azure and you are good to go.

*Uses Azure DevOps Services REST API 7.2

# Usage
You can host your own status badge creator for code coverage

Use it in your README or as an widget in Azure

`[![Coverage Status](https://{host}/api/CodeCoverage/Status?organization={organization}&project={project}&definitionId={pipelineId}&branchName={branchName})](https://dev.azure.com/{organization}/{project}/_build/latest?definitionId={pipelineId}&branchName={branchName})`

### Bad coverage
![coverage-pipeline-bad](Images/coverage-pipeline-bad.png)
![coverage-badge-bad](Images/coverage-badge-bad.png) 

### Good coverage
![coverage-pipeline-good](Images/coverage-pipeline-good.png)
![coverage-badge-good](Images/coverage-badge-good.png) 

The percentage is rounded down because I don't want the badge to say 100% when in reality it only is 99.99% or something. Maybe in big projects this could lead to confusion. Maybe I will add a parameter for this in the future.

# Deployment
I use a docker-compose file like this

```yaml
version: "3.4"

services:
    api:
        image: jjs98/azure-code-coverage
        environment:
          Settings:AzureToken: {azureToken}
        ports:
          - 80:80
```

# References
Azure DevOps Services REST API 7.2

[Build - Latest - Get](https://learn.microsoft.com/en-us/rest/api/azure/devops/build/latest/get?view=azure-devops-rest-7.2)

[TestResults - Codecoverage - Get](https://learn.microsoft.com/en-us/rest/api/azure/devops/testresults/codecoverage/get?view=azure-devops-rest-7.2)