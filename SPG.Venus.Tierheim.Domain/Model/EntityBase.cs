namespace SPG.Venus.Tierheim.Domain.Model
{
    public class EntityBase
    {
        public int Id { get; private set; } // PK

        public DateTime? LastChangeDate { get; set; }
    }
}

