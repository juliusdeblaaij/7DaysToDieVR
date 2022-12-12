using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XUiC_ArchetypesWindowGroup;
using ServerTools;

namespace _7DaysToDieVR
{
    public class API : IModApi
    {
        private string ServerChatName = "Server";

        public void InitMod(Mod _modInstance)
        {
            //This registers a handler for when the server handles a chat message.
            ModEvents.ChatMessage.RegisterHandler(ChatMessage);
        }

        //This method will then be called every time a ChatMessage is sent.
        private bool ChatMessage(ClientInfo clientInfo, EChatType _type, int _senderId, string message, string mainName, bool _localizeMain, List<int> _recipientEntityIds)
        {
            //We make sure there is an actual message and a client, and also ignore the message if it's from the server.
            if (!string.IsNullOrEmpty(message) && clientInfo != null && mainName != ServerChatName)
            {
                //We check to see if the message starts with a /
                if (message.StartsWith("/"))
                {
                    //we then remove that / to get the rest of the message.
                    message = message.Replace("/", "");

                    if (message == "hello")
                    {
                        ChatHook.ChatMessage(clientInfo, $"Hello {clientInfo.playerName}", -1, Config.Server_Response_Name, EChatType.Global, null);
                        //We return false to prevent any other listeners from processing this message.
                        return false;
                    }
                }
            }
            //Returning true allows other listeners to process this message.
            return true;
        }
    }
}
