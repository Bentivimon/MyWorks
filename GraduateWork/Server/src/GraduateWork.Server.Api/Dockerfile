# Stage 1 
FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /app

COPY . ./

RUN dotnet restore ./GraduateWork.Server.Api/GraduateWork.Server.Api.csproj
RUN dotnet publish ./GraduateWork.Server.Api/GraduateWork.Server.Api.csproj -c Release -o /dist

# Stage 2
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=builder /dist .

EXPOSE 80
ENTRYPOINT ["dotnet", "GraduateWork.Server.Api.dll"]