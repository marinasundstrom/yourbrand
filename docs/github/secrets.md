# GitHub Secrets

In order for workflow to run and deploy the services, you need to add these secrets to your GitHub repository.

Azure deployment:

* ``AZURE_CREDENTIALS`` - The JSON output in step "Generate Azure Credentials"
* ``REGISTRY_USERNAME`` - Azure Client Id
* ``REGISTRY_PASSWORD`` - Azure Client Secret
* ``RESOURCE_GROUP`` - the name of your Azure Resource Group
* ``REGISTRY_LOGIN_SERVER`` - the host name of your Azure Container Registry

Database migrations:

* ``CARTSDBCONNECTION`` - Connection string with password
* ``CATALOGDBCONNECTION`` - Connection string with password