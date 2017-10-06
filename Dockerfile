#FROM mono:4.8.0.495-onbuild
#RUN ls /usr/src/app/build/
#WORKDIR /usr/src/app/build/
#EXPOSE 6021
#CMD [ "mono",  "/usr/src/app/build/MjpegProxyServer.exe" ]

FROM mono:4.8.0.495
#RUN mkdir -p release 
WORKDIR /usr/src/app/build/
ADD ["./","./"]
RUN xbuild /p:Configuration=Debug MjpegProxyServer.sln
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Debug
RUN echo *******************************************************************************
RUN xbuild /p:Configuration=Debug MjpegProxyServer.sln
RUN ls /usr/src/app/build/MjpegProxyServer/bin/Release
#CMD [ "mono",  "/usr/src/app/build/debug/MjpegProxyServer.exe" ]
CMD [ "mono",  "/usr/src/app/build/MjpegProxyServer/bin/Release/MjpegProxyServer.exe" ]
EXPOSE 6021
#RUN apt-get update && apt-get install mono-4.0-service -y
#CMD [ "mono-service",  "/usr/src/app/build/MjpegProxyServer.exe" , "--no-daemon" ]
