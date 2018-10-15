FROM mono:5.2 as builder
#RUN mkdir -p release 
WORKDIR /usr/src/app/build/
ENV MJPEG_URL=http://ymc.redirectme.com/turtlecam
COPY ["./","./"]
RUN ls /usr/src/app/build/
RUN ls /usr/src/app/build/MjpegProxyServer/
RUN nuget restore MjpegProxyServer.sln

RUN msbuild MjpegProxyServer.sln /p:Configuration=Debug
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

#RUN msbuild MjpegProxyServer.sln /p:Configuration=Release
#RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug

FROM scratch
COPY --from=builder /usr/src/app/build/MjpegProxyServer/bin/ ./

#CMD [ "sh",  "-c", "mono /usr/src/app/build/MjpegProxyServer/bin/Debug/MjpegProxyServer.exe" ]
CMD [ "sh",  "-c", "mono /app/MjpegProxyServer.exe" ]

EXPOSE 6021


