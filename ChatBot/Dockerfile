# Stage 1 
FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /app

COPY . .

RUN dotnet restore .
RUN dotnet publish . -c Release -o /dist

# Stage 2
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=builder /dist .


ENTRYPOINT ["dotnet", "ChatBot.Api.dll"]
