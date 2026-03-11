# Auditoría Fase 4 – Código autogenerado con antipatrones

Esta carpeta contiene una solución .NET 9 que simula código generado por IA: compila, tiene pruebas unitarias que pasan, pero **incluye antipatrones deliberados** que el candidato debe detectar y refactorizar en la Fase 4 de la prueba técnica.

## Cómo ejecutar

```bash
cd auditoria-fase4
dotnet build
dotnet test
dotnet run --project src/AuditoriaFase4.API
```

La API expone `GET /api/orders` y `POST /api/orders`. No es necesario configurar base de datos: usa SQLite (`auditoria.db` en el directorio de la API).

## Uso en la prueba

El evaluador entrega esta solución (o un PR derivado) al candidato para la **Fase 4: Auditoría inversa**. El candidato debe:

1. Identificar los antipatrones (arquitectura, rendimiento, manejo de excepciones).
2. Emitir juicios Aceptar/Modificar/Rechazar con justificación.
3. Corregir los problemas mediante prompts a la IA, sin reescribir todo a mano.

La lista detallada de antipatrones inyectados y dónde encontrarlos está en [ANTIPATRONES_FASE4.md](ANTIPATRONES_FASE4.md) (solo para evaluadores).
