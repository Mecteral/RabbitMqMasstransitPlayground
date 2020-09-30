namespace Rabbit.Shared
{
    public static class RabbitConstants
    {
        public const string FrameWorkQueue = "FrameworkQueueEndpoint";
        public const string CoreWorkQueue = "CoreQueueEndpoint";
        public const string FrameWorkRpcQueue = "FrameworkRpcQueueEndpoint";
        public const string CoreRpcQueue = "CoreRpcQueueEndpoint";

        public const string RabbitHost = "localhost";
        public const string RabbitUserName = "guest";
        public const string RabbitPassword = "guest";
    }
}