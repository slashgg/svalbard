#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 61174
EXPOSE 44309

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ["Svalbard.Fakes/Svalbard.Fakes.csproj", "Svalbard.Fakes/"]
RUN dotnet restore "Svalbard.Fakes/Svalbard.Fakes.csproj"
COPY . .
WORKDIR "/src/Svalbard.Fakes"
RUN dotnet build "Svalbard.Fakes.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Svalbard.Fakes.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Svalbard.Fakes.dll"]