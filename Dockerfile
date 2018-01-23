

FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /
COPY *.sln ./
COPY ./ImVehicleCore/*.csproj ./ImVehicleCore/
COPY ./Infrastructure/*.csproj ./Infrastructure/
COPY ./SearchParser/*.csproj ./SearchParser/

WORKDIR /
COPY ./Web/*.csproj ./Web/
RUN dotnet restore 

COPY . ./
WORKDIR ./Web
RUN dotnet publish  -c Release -o /out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /out
COPY --from=build-env  /out .
ENTRYPOINT ["dotnet", "Web.dll"]
