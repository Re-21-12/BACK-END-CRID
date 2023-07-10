using Microsoft.AspNetCore.Mvc;
using CustomersApi.Dtos;
using CustomersApi.Repositories;
using CustomersApi.UseCases;
using Microsoft.EntityFrameworkCore;

namespace CustomersApi.Controllers
{
    [ApiController] //se agrega para ser una API
    //public class CustomerController : Controller <- lo de los puntos se ve afectado por el api
    [Route("api/[controller]")] // se usa para usar rutas en la URL
    public class CustomerController : Controller
    {
        //provicional??? //inyeccion de dependencias
        private readonly CustomerDataBaseContext _customerDataBaseContext;
        private readonly IUpdateCustomerUseCase _updateCustomerUseCase;
        //ctor constructor rapido
        //inyectar nuestro context en nuestra API
        public CustomerController(CustomerDataBaseContext customerDataBaseContext, IUpdateCustomerUseCase updateCustomerUseCase)
        {
            _customerDataBaseContext = customerDataBaseContext;
            _updateCustomerUseCase = updateCustomerUseCase;
        }

        //pedir solo un usuario
        [HttpGet("{id}")] //<- este es un request params ?
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer(long id)
        {
            CustomerEntity result = await _customerDataBaseContext.Get(id);
            if(result != null)
            {
                Customer cutomerDto = result.ToDto();
                return Ok(cutomerDto);
            }
            return NotFound();
            // throw new NotImplementedException();
        }

        //pedir todos los usuarios
        [HttpGet()] //<- este es un request params ?
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Customer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //falta async?
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerDataBaseContext.Customer
                .Select(x => x.ToDto())
                .ToListAsync();

            return new OkObjectResult(result);
        }
        //eliminar un usuario
        [HttpDelete("{id}")] //<- este es un request params ?
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var result = await _customerDataBaseContext.Delete(id);
            return new OkObjectResult(result);
        }

        //crear un nuevo id
            [HttpPost]
            [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Customer))]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[FromBody]
        public async Task<IActionResult> CreateCustomer( CreateCustomer customer)
            {
            //var result  = new Customer();
                CustomerEntity result = await _customerDataBaseContext.Add(customer);
                if (result != null)
                {
                    Customer createdCustomer = result.ToDto();
                //    return Created($"https://localhost:7139/api/customer/{createdCustomer.Id}", null);
                return Created($"https://localhost:7139/api/customer/{createdCustomer.Id}", createdCustomer);

            }

            return BadRequest("Cliente no creado");
            }

        //actualizar
        //[HttpPut("{id}")] <- de la otra manera
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            Customer? result = await _updateCustomerUseCase.Execute(customer);
            if(result == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(result);
        }

        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
