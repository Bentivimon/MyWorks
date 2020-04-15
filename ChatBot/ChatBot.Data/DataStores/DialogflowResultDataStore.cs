using System;
using ChatBot.Data.Entities;

namespace ChatBot.Data.DataStores
{
    public class DialogflowResultDataStore : BaseDataStore<Guid, DialogflowResultEntity>
    {
        public DialogflowResultDataStore(ApplicationDbContext db) : base(db, db.DialogflowResults)
        {
        }
    }
}
