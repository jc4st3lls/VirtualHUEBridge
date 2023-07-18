#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV TZ=Europe/Madrid
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN apt-get update && apt-get install tzdata -y
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ZeroHue/ZeroHue.csproj", "ZeroHue/"]
RUN dotnet restore "ZeroHue/ZeroHue.csproj"
COPY . .
WORKDIR "/src/ZeroHue"
RUN dotnet build "ZeroHue.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZeroHue.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZeroHue.dll"]
