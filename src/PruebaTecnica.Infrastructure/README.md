# PruebaTecnica.Infrastructure

Implementar aquí:

- **DbContext** y configuraciones de EF Core (mapeos, migraciones).
- Implementaciones concretas de las interfaces definidas en Application (por ejemplo `IRepository<T>`).
- Acceso a PostgreSQL, sistema de archivos u otros recursos externos.

**Regla:** Esta capa es la única que debe referenciar y usar `DbContext` y `DbSet<T>`.
