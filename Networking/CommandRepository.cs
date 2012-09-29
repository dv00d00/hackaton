namespace AirHockey.Recognition.Client.Networking
{
    public class CommandRepository
    {
        public InitCommand GetInitCommand()
        {
            return new InitCommand();
        }
    }
}