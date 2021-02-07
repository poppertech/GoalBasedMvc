  FROM microsoft/aspnetcore:2.0
  COPY bin/Release/netcoreapp2.0/publish/ App/
  WORKDIR /App
  ENTRYPOINT ["dotnet", "GoalBasedMvc.dll"]