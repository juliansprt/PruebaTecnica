namespace PruebaTecnica.Application.Contracts;

/// <summary>
/// Contrato de persistencia para la capa de aplicación. Las implementaciones concretas
/// (que usan DbContext) deben vivir exclusivamente en Infrastructure.
/// </summary>
public interface IRepository<T> where T : class
{
    // Añadir métodos del contrato según el dominio. Ejemplo: GetByIdAsync, AddAsync, etc.
}
