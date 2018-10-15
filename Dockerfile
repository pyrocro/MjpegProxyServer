FROM mono:latest as builder
#RUN mkdir -p release 
WORKDIR /usr/src/app/build/
COPY ["./","./"]
RUN ls /usr/src/app/build/
RUN ls /usr/src/app/build/MjpegProxyServer/
RUN nuget restore MjpegProxyServer.sln

RUN msbuild MjpegProxyServer.sln /p:Configuration=Debug
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

#RUN msbuild MjpegProxyServer.sln /p:Configuration=Release
#RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

#FROM scratch
FROM mono:latest
ENV MJPEG_URL=http://ymc.redirectme.com/turtlecam
WORKDIR /app/
COPY --from=builder /usr/src/app/build/MjpegProxyServer/bin/ ./
RUN pwd
RUN ls ./

#CMD [ "sh",  "-c", "mono /usr/src/app/build/MjpegProxyServer/bin/Debug/MjpegProxyServer.exe" ]
CMD [ "mono","/app/Debug/MjpegProxyServer.exe" ]

EXPOSE 6021


