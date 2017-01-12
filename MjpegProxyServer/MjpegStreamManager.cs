using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjpegProxyServer
{
    public class MjpegStreamManager
    {
        public Dictionary<string, MjpegStream> streamList = new Dictionary<string, MjpegStream>();
        public MjpegStreamManager()
        {
        }
        public void addStream(string name, string url)
        {
            MjpegStream stream = new MjpegStream(url);
            streamList.Add(name, stream);
        }
        public MjpegStream getMjpegStream(string name, MjpegWebSocketBehavior msb)
        {
            MjpegStream stream = null;            
            stream = streamList[name];
            stream.connectionList[msb.ID] =  msb;

            return stream;
        }
        public void removeConnectionReference(string id)
        {
            foreach(var stream in streamList)
            {
                stream.Value.stop();
                stream.Value.connectionList.Remove(id);
            }
        }

    }
}
