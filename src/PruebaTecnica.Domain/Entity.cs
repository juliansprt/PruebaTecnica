namespace PruebaTecnica.Domain;

/// <summary>
/// Marcador base para entidades de dominio. Añadir entidades de negocio en esta capa.
/// Esta capa no debe referenciar Application, Infrastructure ni API.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
}
