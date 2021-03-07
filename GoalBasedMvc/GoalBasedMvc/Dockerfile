  FROM microsoft/aspnetcore:2.0
  COPY bin/Release/netcoreapp2.0/publish/ App/
  WORKDIR /App
  ENTRYPOINT ["dotnet", "GoalBasedMvc.dll"]

# Server=host.docker.internal,1433;Database=Goal-Based-Database;User Id=admin;Password=P@ssword1
# dotnet publish -c Release
# docker build -f C:/Users/bwynn/Documents/GoalBasedMvc/GoalBasedMvc/GoalBasedMvc/Dockerfile -t goalbasedmvc C:/Users/bwynn/Documents/GoalBasedMvc/GoalBasedMvc/GoalBasedMvc
# docker run -d -p 80:80 --name=goalbasedmvc goalbasedmvc
# docker exec -it goalbasedmvc bash
# docker tag goalbasedmvc:latest poppertech/goalbasedmvc:latest
# docker push poppertech/goalbasedmvc:latest