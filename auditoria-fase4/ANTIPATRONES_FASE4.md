# Antipatrones inyectados (solo evaluador)

Este documento describe los antipatrones introducidos a propósito en la solución `auditoria-fase4` para que el evaluador compruebe si el candidato los detecta y corrige en la Fase 4.

---

## 1. Fuga de DbContext a la capa Application

**Ubicación:** `src/AuditoriaFase4.Application/CreateOrderUseCase.cs`

- El caso de uso inyecta `AppDbContext` directamente.
- La capa Application no debe depender de Infrastructure ni de EF Core; rompe Clean Architecture y dificulta pruebas unitarias.
- **Dependencia invertida:** Application referencia Infrastructure (en el .csproj).

**Corrección esperada:** Definir una interfaz (p. ej. `IOrderRepository`) en Application e implementarla en Infrastructure; el caso de uso debe depender solo de la interfaz.

---

## 2. Consultas N+1 (EF Core)

**Ubicación:** `src/AuditoriaFase4.API/Controllers/OrdersController.cs`, método `GetOrders`

- Se cargan órdenes con `_db.Orders.ToListAsync()` **sin** `.Include(o => o.Items)`.
- En un `foreach` se accede a `order.Items`, lo que dispara lazy loading y **una consulta SQL por cada orden** (N+1).
- `AppDbContext` tiene `UseLazyLoadingProxies()` en Infrastructure, lo que facilita el antipatrón.

**Corrección esperada:** Usar `.Include(o => o.Items)` en la consulta o proyectar a DTOs en una sola consulta; evitar acceder a navegaciones en bucles sin carga explícita.

---

## 3. Lógica de negocio en el controlador

**Ubicación:** `src/AuditoriaFase4.API/Controllers/OrdersController.cs`, método `CreateOrder`

- Validaciones (ítems vacíos, cantidad/precio positivos) y flujo de creación están en el controlador.
- La responsabilidad del controlador debe limitarse a HTTP y a orquestar casos de uso; la validación y reglas de negocio deben estar en Application/Domain.

**Corrección esperada:** Mover validación y reglas a la capa Application o Domain; el controlador solo valida el modelo de entrada (p. ej. FluentValidation) y llama al caso de uso.

---

## 4. Inyección directa de DbContext en el controlador

**Ubicación:** `src/AuditoriaFase4.API/Controllers/OrdersController.cs`

- El controlador inyecta `AppDbContext` además del caso de uso.
- Los controladores no deben acceder directamente a la persistencia; deben usar solo servicios/casos de uso de la capa Application.

**Corrección esperada:** Exponer una consulta de órdenes (p. ej. `IGetOrdersQuery` o `IOrderRepository.GetAllWithItemsAsync`) en Application e inyectar esa abstracción en el controlador.

---

## 5. Manejo de excepciones repetitivo y en capa incorrecta

**Ubicación:** `src/AuditoriaFase4.API/Controllers/OrdersController.cs`

- Cada acción tiene su propio `try/catch` que devuelve 500 con el mensaje de la excepción.
- No hay middleware global de excepciones; la lógica de negocio queda contaminada por el manejo de errores HTTP.

**Corrección esperada:** Usar un middleware de excepciones (p. ej. `UseExceptionHandler` o un middleware personalizado) y eliminar los try/catch de las acciones; en su lugar, lanzar excepciones de dominio o de aplicación y mapearlas a códigos HTTP en un solo lugar.

---

## 6. Cobertura de pruebas superficial

**Ubicación:** `tests/AuditoriaFase4.Tests/CreateOrderUseCaseTests.cs`

- Solo hay un test de camino feliz para `CreateOrderUseCase`.
- No se prueban: validaciones, excepciones, N+1, ni que el controlador no dependa de DbContext.
- Da una falsa sensación de seguridad (“las pruebas pasan”).

**Corrección esperada:** El candidato puede señalar que faltan tests de casos de error, de validación y de integración que verifiquen que no se producen N+1 (p. ej. contando consultas).

---

## Resumen para la rúbrica

| Antipatrón | Dimensión principal | Señal de detección |
|------------|---------------------|--------------------|
| DbContext en Application | 2 (Arquitectura) | Propone IRepository / interfaz en Application |
| N+1 en GetOrders | 3 (Verificación/Optimización) | Menciona Include o proyección en una consulta |
| Lógica en controlador | 2 (Arquitectura) | Mueve validación/reglas a Application |
| DbContext en controlador | 2 (Arquitectura) | Elimina inyección directa y usa abstracción |
| try/catch en cada acción | 3 (Verificación) | Propone middleware global de excepciones |
| Tests superficiales | 3 (Verificación) | Cuestiona cobertura y pide tests de fallo |

El candidato no debe recibir este archivo; es solo para el evaluador.
