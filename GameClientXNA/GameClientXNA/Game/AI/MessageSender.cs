using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClientXNA.Game.AI
{
    class MessageSender
    {
        String message;
        TimeSpan sentTime;
        TimeSpan timeGap;
        bool isMsgSent;
        bool MsgSent{ get{ return isMsgSent; }}

        NetworkClient networkClient;

        public MessageSender(NetworkClient networkClient)
        {
            timeGap = new TimeSpan(0,0,0,0,990);
            this.networkClient = networkClient;
            isMsgSent = true;
        }

        public void update(TimeSpan time)
        {
            if((time-sentTime > timeGap) && (!isMsgSent))
            {
                sentTime = time;
                networkClient.Send(message);
                isMsgSent = true;
            }
        }

        public void setMessage(String msg)
        {
            message = msg;
            isMsgSent = false;
        }

        public void markSendFailed()
        {
            isMsgSent = false;
        }
    }
}
