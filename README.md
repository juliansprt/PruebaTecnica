# Prueba Técnica .NET (Vibe Coding & IA)

Repositorio base para la prueba técnica. Contiene:

- **Documentación**
  - [INSTRUCCIONES_CANDIDATO.md](INSTRUCCIONES_CANDIDATO.md): instrucciones y fases para el candidato.

- **Esqueleto de solución .NET 9** (Fase 1–3)
  - `src/PruebaTecnica.Domain`: entidades y lógica de dominio.
  - `src/PruebaTecnica.Application`: contratos (interfaces) y casos de uso.
  - `src/PruebaTecnica.Infrastructure`: implementaciones, DbContext y acceso a datos.
  - `src/PruebaTecnica.API`: API web (ASP.NET Core).

- **Código para auditoría (Fase 4)**
  - Carpeta [auditoria-fase4/](auditoria-fase4/): solución con antipatrones deliberados para que el candidato audite y refactorice con la IA.

Compilación del esqueleto:

```bash
dotnet build PruebaTecnica.slnx
```
