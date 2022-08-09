# EmployeesMicroserviesSystem
This is Microservices Project built with .NETCore(6.0), Angular(13.0), Ocelot Api Gateways, RabbitMQ, SQL Server Databases,docker and Kubernetes Deployment
# Project Archtiture
![plot](./ProjectArchtiture/ProjectArchtiture.jpeg)

# Description
The Project is Microservices Application built with Devolopment and Production Environment Consists Of :

1- Angular(13.0) Client Application 

2- .NETCore(6.0) For Backend Services talking to Each With httpclient Synchronous and RabbitMQ for Asynchronous Messaging 

3- 2 Ocelot Api Gate in front of ingress nginx controller to route to the backend services one of them use authentication and authorization and the other not 

4- service for identity manager to manage users and roles for the applications and send it to the gateways to authenticate users with its sql server database 

5- service To Access The Employees DB With no authentication gateway To retrieve Employees 

6- Service To Publish Messages With Crud Event Throw The Gateway For authenticated and authorized Users To the RabbitMQ For Other Service Related To The Employees DataBase (Sql Server) To Do the Update To The Database and send Update Event To The Queue For the notification service to retrieve

7- SignalR real time communication service connects to the angular application throw the Ocelot Api Gateway (authenticated users) 
To send Success and Error Updates (Events) and Messages (Payloads)

8- DockerFiles for each service and .yaml deployment files for each service and database and pipelines 

9- Self Signed Certificate for the local secure https connection for the deployed application with 2 diffrent domains talking to each other one for the client and one for the rest backend services 

10- the project is ready to run in production mode with the right kubernetes platform or in Devolopment Mode with local Dev runners with internal Commands:```

``` dotnet run ``` - for the .Net services 

``` ng serve ``` - for the angular application
