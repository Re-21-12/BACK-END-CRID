using CustomersApi.Dtos;
using CustomersApi.Repositories;

namespace CustomersApi.UseCases

{
    public interface IUpdateCustomerUseCase
    {
        //debera tener un metodo unico esta clase 
    Task<Customer?> Execute(Customer customer);
    }
    public class UpdateCustomerUseCase: IUpdateCustomerUseCase
    {
        //inyeccion de dependencias
        private readonly CustomerDataBaseContext _customerDataBaseContext;
        public UpdateCustomerUseCase(CustomerDataBaseContext customerDataBaseContext)
        {
            //inyeccion de dependencias
            _customerDataBaseContext = customerDataBaseContext;
        }
        public async Task<Customer?> Execute(Customer customer)
        {
            var entity = await _customerDataBaseContext.Get(customer.Id);
            if (entity == null)
             return null;
            //aparte
            entity.firstName = customer.firstName;
            entity.lastName = customer.lastName;
            entity.email = customer.email;
            entity.phone = customer.phone;
            entity.address = customer.address;
            await _customerDataBaseContext.Update(entity);
            return entity.ToDto();
        }
    }
}
