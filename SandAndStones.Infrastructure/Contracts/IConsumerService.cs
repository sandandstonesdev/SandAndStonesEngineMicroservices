﻿namespace SandAndStones.Infrastructure.Contracts
{
    public interface IConsumerService
    {
        void ProcessMessage(string message);
    }
}
