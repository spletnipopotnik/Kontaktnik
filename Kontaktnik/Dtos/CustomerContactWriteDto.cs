namespace Kontaktnik.Dtos
{
    public class CustomerContactWriteDto
    {
        public string DataValue { get; set; }

        public int ContactTypeId { get; set; }
       
        public Guid CustomerId { get; set; }
    }
}
