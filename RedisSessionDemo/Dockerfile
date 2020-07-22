FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine as rt-image
WORKDIR /app
COPY --from=builder /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "RedisSessionDemo.dll"]

