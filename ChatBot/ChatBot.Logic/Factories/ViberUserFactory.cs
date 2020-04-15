using System;
using ChatBot.Data.Entities;
using ChatBot.Models.Callbacks.Viber;

namespace ChatBot.Logic.Factories
{
    public static class ViberUserFactory
    {
        public static ViberUserEntity ToEntity(SubscribedCallback callback)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return new ViberUserEntity
            {
                Avatart = callback.User.Avatart,
                Country = callback.User.Country,
                CreatedTimestamp = now,
                UpdatedTimestamp = now,
                IsSubscribed = true,
                Language = callback.User.Language,
                Name = callback.User.Name,
                ViberId = callback.User.Id
            };
        }

        public static ViberUserEntity ToEntity(ReceiveMessageFromUserCallback callback)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

           return new ViberUserEntity
            {
                Avatart = callback.Sender.Avatart,
                Country = callback.Sender.Country,
                CreatedTimestamp = now,
                UpdatedTimestamp = now,
                IsSubscribed = true,
                Language = callback.Sender.Language,
                Name = callback.Sender.Name,
                ViberId = callback.Sender.Id
            };
        }
    }
}
