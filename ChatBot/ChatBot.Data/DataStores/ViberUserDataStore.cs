using System;
using ChatBot.Data.Entities;

namespace ChatBot.Data
{
    public class ViberUserDataStore : BaseDataStore<Guid, ViberUserEntity>
    {
        public ViberUserDataStore(ApplicationDbContext db) : base(db, db.ViberUsers)
        {
        }
    }
}
