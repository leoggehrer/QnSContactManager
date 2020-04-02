#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["QnSContactManager.AspMvc/QnSContactManager.AspMvc.csproj", "QnSContactManager.AspMvc/"]
COPY ["QnSContactManager.Adapters/QnSContactManager.Adapters.csproj", "QnSContactManager.Adapters/"]
COPY ["QnSContactManager.Transfer/QnSContactManager.Transfer.csproj", "QnSContactManager.Transfer/"]
COPY ["QnSContactManager.Contracts/QnSContactManager.Contracts.csproj", "QnSContactManager.Contracts/"]
COPY ["CommonBase/CommonBase.csproj", "CommonBase/"]
COPY ["QnSContactManager.Logic/QnSContactManager.Logic.csproj", "QnSContactManager.Logic/"]
RUN dotnet restore "QnSContactManager.AspMvc/QnSContactManager.AspMvc.csproj"
COPY . .
WORKDIR "/src/QnSContactManager.AspMvc"
RUN dotnet build "QnSContactManager.AspMvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QnSContactManager.AspMvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QnSContactManager.AspMvc.dll"]