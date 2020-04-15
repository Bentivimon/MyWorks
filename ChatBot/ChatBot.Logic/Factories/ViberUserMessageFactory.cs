using System;
using ChatBot.Data.Entities;
using ChatBot.Models.Callbacks.Viber;

namespace ChatBot.Logic.Factories
{
    public static class ViberUserMessageFactory
    {
        public static ViberUserMessageEntity ToEntity(ReceiveMessageFromUserCallback callback, Guid userId) =>
            new ViberUserMessageEntity
            {
                Message = callback.Message.Text,
                MessageType = callback.Message.Type,
                UserId = userId
            };
    }
}
