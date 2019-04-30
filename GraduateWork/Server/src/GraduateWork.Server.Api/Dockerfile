FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Server/src/GraduateWork.Server.Api/GraduateWork.Server.Api.csproj", "Server/src/GraduateWork.Server.Api/"]
RUN dotnet restore "Server/src/GraduateWork.Server.Api/GraduateWork.Server.Api.csproj"
COPY . .
WORKDIR "/src/Server/src/GraduateWork.Server.Api"
RUN dotnet build "GraduateWork.Server.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GraduateWork.Server.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GraduateWork.Server.Api.dll"]