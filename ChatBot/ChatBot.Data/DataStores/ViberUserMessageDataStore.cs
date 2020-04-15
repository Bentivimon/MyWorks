using System;
using ChatBot.Data.Entities;

namespace ChatBot.Data
{
    public class ViberUserMessageDataStore : BaseDataStore<Guid, ViberUserMessageEntity>
    {
        public  ViberUserMessageDataStore(ApplicationDbContext db) : base(db, db.ViberUserMessages)
        {
        }
    }
}
