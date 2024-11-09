using System;

namespace Domain.Entities
{
    public class StandardNote : Note
    {
        public StandardNote(int id, string title, string text, DateTime createdOn, DateTime modifiedOn) : base(id, title, text, createdOn, modifiedOn)
        {

        }
    }
}
