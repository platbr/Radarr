FROM ubuntu:xenial as builder

ENV DEBIAN_FRONTEND noninteractive
ARG MONO_VERSION=5.20
ARG MONO_URL=stable-xenial/snapshots/$MONO_VERSION

RUN apt-get update && apt-get -y install curl dirmngr apt-transport-https lsb-release ca-certificates && \
    curl -sL https://deb.nodesource.com/setup_10.x | bash && \
    rm -rf /var/lib/apt/lists/*

RUN curl -sL https://dl.yarnpkg.com/debian/pubkey.gpg | apt-key add - && \
    echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list && \
    apt-get update && apt-get -y install yarn && \
    rm -rf /var/lib/apt/lists/*

RUN curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    curl https://packages.microsoft.com/config/ubuntu/18.04/prod.list >  /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg  /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get install -y dotnet-sdk-3.1 && \
    rm -rf /var/lib/apt/lists/*

RUN apt-get update && \
    apt-get install -y unzip git make && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /build

ARG BUILD_NUMBER="3.10.0.001"
ARG BRANCH="platbr"

# COPY platbr/dotnet-restore.tar .
# RUN tar xf dotnet-restore.tar && dotnet restore src/Radarr.sln

COPY . /build
RUN dotnet restore src/Radarr.sln
RUN ./build.sh netcoreapp3.1 linux-x64 --all

FROM lsiobase/ubuntu:focal
ARG BUILD_NUMBER="3.10.0.001"
ARG BRANCH="platbr"
LABEL maintainer="platbr"

RUN mkdir -p /app/radarr/bin
COPY --from=builder /build/_artifacts/linux-x64/netcoreapp3.1/Radarr /app/radarr/bin

RUN echo "UpdateMethod=docker\nBranch=${BRANCH}\nPackageVersion=${BUILD_NUMBER}\nPackageAuthor=platbr" > /app/radarr/package_info && \
    rm -rf /app/radarr/bin/Radarr.Update

# add local files
COPY platbr/docker-root/ /

# ports and volumes
EXPOSE 8989
VOLUME /config
