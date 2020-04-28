find . -name '*.csproj' -o -name '*.sln' -o -name 'nuget.config' | sort | tar cf platbr/dotnet-restore.tar -T - 2> /dev/null
docker build -f platbr/Dockerfile . -t platbr/radarr:latest