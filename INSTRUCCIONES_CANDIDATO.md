# Prueba Técnica: Desarrollador .NET en la Era del Vibe Coding

Esta prueba evalúa tu capacidad para orquestar herramientas de IA generativa, imponer rigor arquitectónico (Clean Architecture) y auditar código en un entorno realista. **No se evalúa solo la sintaxis**: se valora la gobernanza del contexto, la delegación a agentes y la auditoría crítica del código generado.

**Duración estimada:** 150–180 minutos.

---

## Herramientas permitidas

Puedes utilizar **cualquiera** de los siguientes asistentes o IDEs impulsados por IA:

- **Cursor**
- **Antigravity**
- **Claude**
- **Codex**

La elección es libre. En la entrega deberás documentar qué herramienta(s) utilizaste y qué modelos en cada momento (ver sección de entregables).

---

## Aplicación a construir (dominio fijo)

Debes construir un sistema de **Gestión de Inventario y Órdenes** (Inventory & Order Management System) que incluya, como mínimo:

- **Inventario:** productos con identificador, nombre, cantidad en stock y precio.
- **Órdenes:** creación de órdenes que referencian productos, validación de stock disponible y actualización de inventario al confirmar una orden.
- API REST para consultar productos, consultar órdenes y crear órdenes (respetando Clean Architecture y las reglas que definas en Fase 1).

Todas las fases (reglas, MCP, implementación y auditoría) se desarrollan en torno a este dominio.

---

## Requisitos de entrega (obligatorios)

Debes entregar la prueba mediante un **repositorio público en GitHub** que contenga **obligatoriamente**:

1. **Código fuente completo**  
   Toda la solución .NET 9, incluyendo las **pruebas unitarias** que hayas asegurado durante la prueba.

2. **Transcripts / historial de conversaciones**  
   **Todas** las conversaciones y prompts realizados con el agente del IDE durante el desarrollo. Inclúyelas en una carpeta del repositorio (por ejemplo `conversations/`, `transcripts/` o `chats/`) en formato exportado por tu herramienta (JSON, Markdown, etc.). Sin excepciones: el evaluador debe poder revisar el diálogo completo con la IA.

3. **Documentación de modelos utilizados**  
   Un archivo **`MODEL_USAGE.md`** en la raíz del repositorio donde indiques de forma explícita:
   - Qué **modelos** de IA utilizaste (nombre exacto del modelo).
   - En **qué fase o momento** los usaste y para **qué tipo de tarea**.

   **Ejemplo de redacción:**

   ```markdown
   - **Opus 4.6**: diseño de capas y lógica de negocio (Fase 1 y 3).
   - **Gemini Flash**: generación de documentación y comentarios (Fase 3).
   - **Claude 3.5 Sonnet**: refactorización y auditoría del PR (Fase 4).
   - **Claude 3.5 Haiku**: generación masiva de pruebas unitarias vía SubAgent (Fase 3).
   ```

   Sin este archivo la prueba se considerará incompleta.

---

## Estructura de la prueba (cuatro fases)

### Fase 1: Arquitectura de contexto y establecimiento de fronteras (30 min)

**Objetivo:** Demostrar que sabes definir un contexto determinista para la IA mediante Rules y, si aplica, Skills.

**Dinámica:**  
Tienes acceso a un repositorio esqueleto en .NET 9 con proyectos por capas (Domain, Application, Infrastructure, API) pero sin implementación. **No se pide que escribas código C# en esta fase**: tu tarea es redactar las **instrucciones fundacionales** para el agente.

Debes:

- Redactar el archivo de reglas del proyecto (por ejemplo `.cursorrules`, `.windsurfrules`, `CLAUDE.md` o el equivalente de tu herramienta).
- Estructurar al menos un archivo o sección de **Skills** (habilidades dinámicas) para el proyecto.

El contenido debe:

- Definir **explícitamente** las responsabilidades de cada capa de Clean Architecture.
- Establecer la regla **innegociable** de que `DbContext` y EF Core pertenecen **exclusivamente** a la capa de Infraestructura.
- Fijar preferencias para APIs RESTful: rutas por atributos, inyección de dependencias estándar de ASP.NET Core, uso de registros inmutables donde corresponda.

**Evaluación:** Se valorará que las reglas sean concretas, imperativas y que restrinjan comportamientos (por ejemplo: prohibir inyección de `DbContext` en controladores o en la capa Application).

---

### Fase 2: Instrumentación del ecosistema mediante MCP (45 min)

**Objetivo:** Evaluar que sabes crear un servidor MCP dentro de la misma solución .NET, conectarlo al IDE y dotar al agente de herramientas alineadas con el dominio (Gestión de Inventario y Órdenes).

**Tareas obligatorias:**

1. **Crear un servidor MCP como proyecto de la misma solución .NET**  
   Añade un proyecto ejecutable (por ejemplo `PruebaTecnica.McpServer`) a la solución existente. El servidor debe implementar el protocolo MCP (Model Context Protocol) y exponer **herramientas (tools)** relacionadas con el dominio, por ejemplo:
   - Consulta de catálogo de productos (`get_product_catalog` o similar).
   - Consulta de stock/inventario (`check_inventory`, `get_stock` o similar).
   - Otras herramientas que consideres útiles para que el agente del IDE trabaje con el contexto de inventario y órdenes sin pegar datos crudos en el chat.

2. **Conectar el servidor MCP en la herramienta del IDE que estés utilizando**  
   Configura tu IDE para usar el servidor MCP que creaste (por ejemplo, ejecutando el proyecto `PruebaTecnica.McpServer` como proceso MCP).

3. **Incluir en el repositorio la configuración de conexión del MCP**  
   Si utilizas **Cursor**, debes añadir y commitear el archivo de configuración donde se declara el servidor MCP (por ejemplo **`.cursor/mcp.json`** o la ruta equivalente que use tu versión de Cursor), de forma que el evaluador pueda ver cómo quedó conectado el servidor. Si usas otro IDE (Antigravity, Claude, Codex), incluye el archivo o carpeta equivalente donde se configure el MCP en ese entorno.

**Evaluación:** Se verificará que el servidor MCP forme parte de la solución .NET, que exponga al menos un par de herramientas útiles para el dominio, que la configuración de conexión (p. ej. `mcp.json`) esté en el repo y que se apliquen buenas prácticas de seguridad (por ejemplo, credenciales o rutas restringidas si el servidor accede a datos sensibles).

---

### Fase 3: Orquestación activa y Vibe Coding (45 min)

**Objetivo:** Observar si actúas como orquestador que planifica y delega, no como mecanógrafo que pide “todo de una vez”.

**Dinámica:**  
Usando el contexto estático (Rules) y dinámico (MCP) de las fases anteriores, debes instruir a la IA para implementar el **núcleo funcional** del sistema de **Gestión de Inventario y Órdenes**, en concreto:

- Entidades y reglas de dominio (Producto, Orden, ítems de orden, validación de stock).
- Endpoints de API: consulta de productos, consulta de órdenes, creación de órdenes (con validación de stock y actualización de inventario).
- Persistencia en base de datos (repositorios en Infraestructura, sin fugas de `DbContext` a Application).
- Manejo de excepciones global (middleware) y, si aplica, eventos de dominio.

**Requisito obligatorio:** El trabajo debe estar **paralelizado** y debes **asegurar pruebas unitarias exhaustivas**. Concretamente:

1. Actuar como **orquestador**: usar el agente principal para definir interfaces y la lógica del caso de uso principal.
2. **Desplegar de forma explícita** un SubAgent (o equivalente en tu herramienta) en segundo plano, encargado **exclusivamente** de generar la suite de **pruebas unitarias** con xUnit y Moq, sin saturar la conversación principal.

Al finalizar la Fase 3, el repositorio debe contener código funcional **y** pruebas unitarias que demuestren tu supervisión (no solo “camino feliz”).

**Evaluación:** Se observará la descomposición en pasos incrementales (“crea la entidad”, “crea el repositorio”, “configura el endpoint”), el uso real de SubAgents/tareas en segundo plano y la calidad/cobertura de las pruebas generadas.

---

### Fase 4: Auditoría inversa y profundidad de verificación (30–60 min)

**Objetivo:** Demostrar escepticismo técnico frente a código que compila y “parece correcto” pero oculta deuda técnica y antipatrones.

**Dinámica:**  
Se te entrega un **Pull Request** (o rama) con código **autogenerado**. El código compila y las pruebas unitarias pueden reportar alta cobertura, pero contiene **antipatrones introducidos a propósito**, por ejemplo:

- Consultas **N+1** en EF Core (bucles que disparan lazy loading).
- Lógica de dominio o acceso a datos **en controladores**.
- Dependencias acopladas sin inyección (uso directo de `DbContext` en capa Application).
- Manejo de excepciones excesivo o en lugares incorrectos.

Debes:

1. Realizar una **auditoría de refactorización**: para cada problema relevante, emitir un juicio **Aceptar / Modificar / Rechazar** con justificación en términos del ecosistema .NET/C#.
2. **Corregir los problemas interactuando con la IA**: no reescribir todo a mano, sino mostrar cómo **elaboras el prompt** para que el agente aplique los patrones correctos (repositorios, `Include`, middleware de excepciones, etc.).

**Evaluación:** Se penaliza la aprobación de vulnerabilidades arquitectónicas o de rendimiento. Se valora que exijas al agente mejoras concretas (p. ej. análisis de consultas generadas, uso de carga explícita, revisión de complejidad).

---

## Resumen de entregables en GitHub

| Entregable | Ubicación / formato |
|------------|---------------------|
| Código fuente completo (.NET 9 + pruebas unitarias) | Raíz del repo (solución y proyectos, **incluyendo el proyecto del servidor MCP**) |
| Servidor MCP integrado en la solución | Proyecto ejecutable en la misma solución (ej. `PruebaTecnica.McpServer`) |
| Configuración de conexión MCP en el IDE | Archivo **`.cursor/mcp.json`** (o equivalente) en el repo |
| Todas las conversaciones con el agente del IDE | Carpeta `conversations/`, `transcripts/` o similar (exportación completa) |
| Modelos usados y momento de uso | Archivo **`MODEL_USAGE.md`** en la raíz |

Sin estos puntos la prueba se considera **incompleta**. Asegúrate de que el repositorio sea **público** y que el enlace se entregue en el canal indicado por el reclutador.
