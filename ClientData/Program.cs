﻿namespace ClientData
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            WebSocketClientAPI client = new();
            await client.ConnectAsync();
            await client.SendBuyBoatMessageAsync(1);
        }
    }
}
