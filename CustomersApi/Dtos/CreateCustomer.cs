using System.ComponentModel.DataAnnotations;

namespace CustomersApi.Dtos
{
    public class CreateCustomer
    {
        //[Required(ErrorMessage ="El nombre esta requerido")]
        public string firstName { get; set; }

        //[Required(ErrorMessage = "El apellido esta requerido")]

        public string lastName { get; set; }

        //[Required(ErrorMessage = "El nombre esta requerido")]

        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
}
