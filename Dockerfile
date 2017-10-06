FROM mono:latest
#RUN mkdir -p release 
WORKDIR /usr/src/app/build/
COPY ["./","./"]
RUN ls /usr/src/app/build/
RUN ls /usr/src/app/build/MjpegProxyServer/
RUN nuget restore MjpegProxyServer.sln

RUN msbuild MjpegProxyServer.sln /p:Configuration=Debug
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

RUN msbuild MjpegProxyServer.sln /p:Configuration=Release
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

CMD [ "sh",  "-c", "mono /usr/src/app/build/MjpegProxyServer/bin/Debug/MjpegProxyServer.exe" ]

EXPOSE 6021


