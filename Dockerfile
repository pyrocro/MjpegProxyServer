#FROM mono:4.8.0.495-onbuild
#RUN ls /usr/src/app/build/
#WORKDIR /usr/src/app/build/
#EXPOSE 6021
#CMD [ "mono",  "/usr/src/app/build/MjpegProxyServer.exe" ]

FROM mono:4.8.0.495
#RUN mkdir -p release 
WORKDIR /usr/src/app/build/
ADD ["./MjpegProxyServer/bin/Debug/*","/usr/src/app/build/debug/"]
RUN ls /usr/src/app/build/debug
ADD ["./MjpegProxyServer/bin/Release/*","/usr/src/app/build/release/"]
RUN ls /usr/src/app/build/release
CMD [ "mono",  "/usr/src/app/build/debug/MjpegProxyServer.exe" ]
EXPOSE 6021
#RUN apt-get update && apt-get install mono-4.0-service -y
#CMD [ "mono-service",  "/usr/src/app/build/MjpegProxyServer.exe" , "--no-daemon" ]
