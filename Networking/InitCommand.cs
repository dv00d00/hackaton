namespace AirHockey.Recognition.Client.Networking
{
    public class InitCommand 
    {
        public byte[] Serialize()
        {
            var data = new byte[]
                {
                    ServerCommands.Init,
                    NetworkConstants.ClientType
                };

            return data;
        }
    }
}