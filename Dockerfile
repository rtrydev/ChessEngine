FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY ChessEngine/*.csproj ./ChessEngine/
COPY ChessAPI/*.csproj ./ChessAPI/

#
RUN dotnet restore 
#
# copy everything else and build app
COPY ChessEngine/. ./ChessEngine/
COPY ChessAPI/. ./ChessAPI/

#
WORKDIR /app
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
#
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "ChessAPI.dll"]

