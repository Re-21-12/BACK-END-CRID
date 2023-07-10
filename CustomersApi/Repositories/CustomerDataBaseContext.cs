using Microsoft.EntityFrameworkCore;
using CustomersApi.Dtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

// Se define un espacio de trabajo (namespace) para los repositorios de la API de clientes.
namespace CustomersApi.Repositories
{
    // Se crea una clase llamada CustomerDataBaseContext que hereda de DbContext.
    public class CustomerDataBaseContext : DbContext
    {
        // Constructor de la clase CustomerDataBaseContext.
        public CustomerDataBaseContext(DbContextOptions<CustomerDataBaseContext> options)
            : base(options) { }

        // Propiedad DbSet para la entidad CustomerEntity.
        public DbSet<CustomerEntity> Customer { get; set; }

        // Método asincrónico para obtener un cliente por su ID.
        public async Task<CustomerEntity> Get(long id)
        {
            return await Customer.FirstOrDefaultAsync(x => x.Id == id);
        }

        // Método asincrónico para agregar un nuevo cliente.
            public async Task<CustomerEntity> Add(CreateCustomer customerDto)
            {
                // Se crea una instancia de CustomerEntity a partir de los datos proporcionados.
                CustomerEntity entity = new CustomerEntity()
                {
                 //   Id = null,
                    address = customerDto.address,
                    email = customerDto.email,
                    firstName = customerDto.firstName,
                    lastName = customerDto.lastName,
                    phone = customerDto.phone,
                };

                // Se agrega la entidad al DbSet de Customer y se guarda en la base de datos.
                //marca la respuesta como una peticion del tipo put? -> mediante addAsync
                EntityEntry<CustomerEntity> response = await Customer.AddAsync(entity);
                await SaveChangesAsync(); //aca se confirma para subir a la BD

            // Se retorna el cliente recién agregado.
            //return await Get(response.Entity.Id);
            return response.Entity;  
        }

        public async Task<bool> Delete(long id)
        {
            CustomerEntity entity = await Get(id);
            if (entity != null)
            {
                Customer.Remove(entity);
                await SaveChangesAsync();
                return true;
            }
            return false; // Return false if the entity was not found.
        }

        public async Task<bool> Update(CustomerEntity customerEntity)
        {
            Customer.Update(customerEntity);
            await SaveChangesAsync();
            return true;
        }
    }

    // Clase que representa la entidad Customer.
    //la clase CustomerEntity actúa como una representación de los datos del cliente en el lado de la base de datos y permite interactuar con los datos de manera orientada a objetos en el código.
    public class CustomerEntity
    {
        [Key]
        //public long id?{get;set;}
        public long Id { get; set; }

        //[Required]
        public string firstName { get; set; }

        //[Required]
        public string lastName { get; set; }

        //[Required]
        public string email { get; set; }

        //[Required]
        public string phone { get; set; }

        //[Required]
        public string address { get; set; }

        // Método ToDto que convierte la entidad CustomerEntity en un objeto Customer (DTO).
        public Customer ToDto()
        {
            return new Customer()
            {
                //Id = Id ?? throw new Exception("El id no puede ser null"),
                Id = Id,
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone,
                address = address
            };
        }
    }
}
