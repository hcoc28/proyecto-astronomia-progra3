# Convenciones del Equipo

Reglas acordadas por los 3 integrantes para trabajar sin conflictos.

---

## 1. Flujo de Git

### 1.1 Ramas

| Rama | Propósito | Reglas |
|------|-----------|--------|
| `main` | Código estable y revisado. | Protegida. Solo merges desde `develop` vía Pull Request. |
| `develop` | Integración de funcionalidades. | Merges desde ramas `feature/*` vía PR. |
| `feature/<descripcion>` | Una rama por funcionalidad nueva. | Se crea desde `develop`. Se borra después del merge. |
| `fix/<descripcion>` | Corrección de bugs puntuales. | Mismo flujo que `feature/*`. |
| `docs/<descripcion>` | Cambios solo de documentación. | Puede mergearse directo a `develop`. |

**Formato del nombre de rama:**
```
feature/lista-enlazada
feature/controlador-objetos
feature/vistas-catalogo
fix/ordenamiento-avl
docs/actualizar-readme
```

### 1.2 Commits

Formato: **Conventional Commits simplificado**.
```
<tipo>(<alcance>): <mensaje corto en presente>
```

**Tipos permitidos:**
- `feat` — nueva funcionalidad
- `fix` — corrección de bug
- `docs` — documentación
- `refactor` — cambio de código sin cambiar comportamiento
- `test` — agregar o corregir pruebas
- `chore` — tareas de mantenimiento (dependencias, configuración)

**Ejemplos buenos:**
```
feat(controllers): agregar ObjetosController con accion Index
feat(views): agregar vista Razor de catalogo de planetas
fix(hash): corregir colision en tabla hash
docs(readme): actualizar instrucciones de instalacion
test(avl): agregar pruebas de rotaciones dobles
refactor(grafo): extraer logica de Dijkstra a metodo aparte
```

**Ejemplos malos (evitar):**
```
✗ "cambios"
✗ "update"
✗ "arreglando cosas"
✗ "WIP"
```

**Reglas de commit:**
- Mensaje en español, primera letra minúscula.
- Máximo 72 caracteres en la primera línea.
- Commits atómicos: un cambio = un commit.
- Mínimo un commit por día de trabajo activo.

### 1.3 Pull Requests (PR)

**Antes de abrir un PR:**
```bash
git checkout develop
git pull origin develop
git checkout mi-rama
git merge develop      # resolver conflictos localmente
```

**Requisitos del PR:**
- Título con formato de commit: `feat(views): agregar pagina de busqueda`.
- Descripción con: qué cambia, por qué, cómo probarlo.
- Al menos **1 revisor** distinto del autor debe aprobar antes del merge.
- El autor hace el merge después de la aprobación.

**Plantilla sugerida de PR:**
```markdown
## ¿Qué cambia?
Breve descripción.

## ¿Por qué?
Contexto o razón del cambio.

## ¿Cómo probarlo?
Pasos para verificar que funciona.

## Checklist
- [ ] El código compila sin errores
- [ ] Se probó manualmente
- [ ] Se actualizó la documentación si aplica
```

---

## 2. Convenciones de código

### 2.1 C# (Backend / MVC)

- **Clases:** `PascalCase` — `ObjetoAstronomico`, `TablaHash`.
- **Métodos públicos:** `PascalCase` — `BuscarPorNombre()`.
- **Métodos privados:** `PascalCase` con prefijo opcional `_` para campos privados.
- **Variables locales y parámetros:** `camelCase` — `objetoActual`.
- **Constantes:** `SCREAMING_SNAKE_CASE` — `MAX_CONEXIONES`.
- **Interfaces:** prefijo `I` — `IRepositorio`.
- **Archivos:** nombre igual a la clase principal — `TablaHash.cs`.
- **Espaciado:** 4 espacios (no tabs).
- **Comentarios XML** en métodos públicos:
  ```csharp
  /// <summary>Busca un objeto por su nombre exacto.</summary>
  /// <param name="nombre">Nombre del objeto.</param>
  /// <returns>El objeto encontrado o null.</returns>
  public ObjetoAstronomico BuscarPorNombre(string nombre) { ... }
  ```

### 2.2 Razor Views (.cshtml)

- **Archivos de vista:** `PascalCase` — `Index.cshtml`, `Detalle.cshtml`.
- **ViewModels:** sufijo `ViewModel` — `ObjetoViewModel`, `BusquedaViewModel`.
- **IDs y clases CSS:** `kebab-case` — `buscador-input`, `card-planeta`.
- **Indentación HTML:** 2 espacios.
- **Evitar lógica compleja en la vista** — toda lógica va en el Controller o Service.

### 2.3 JavaScript (wwwroot/js/)

- **Variables y funciones:** `camelCase` — `obtenerPlanetas()`.
- **Constantes globales:** `SCREAMING_SNAKE_CASE` — `API_BASE_URL`.
- **Archivos:** `kebab-case` — `grafo-viewer.js`.
- **Indentación:** 2 espacios.
- **Uso obligatorio de `const` y `let`** (nunca `var`).
- **Comillas:** simples por defecto, backticks para template strings.

### 2.4 CSS (wwwroot/css/)

- **IDs y clases:** `kebab-case` — `buscador-input`, `card-planeta`.
- **Un selector por línea** en bloques multilínea.
- **Indentación:** 2 espacios.

### 2.5 SQL

- **Palabras clave en mayúsculas:** `SELECT`, `FROM`, `WHERE`, `JOIN`.
- **Nombres de tablas y columnas:** `snake_case` — `objetos_astronomicos`.
- **Tablas en plural** — `objetos_astronomicos`, no `objeto_astronomico`.
- **Prefijo `N` en literales** de texto para Unicode — `N'Marte'`.

---

## 3. Estructura del proyecto

### 3.1 Repositorio (carpetas raíz)

```
proyecto-astronomia-progra3/
├── backend/          → Proyecto ASP.NET Core MVC (.NET 8)
├── database/         → Scripts SQL (schema.sql, seed.sql)
├── docs/             → Documentación del proyecto
├── README.md
├── REQUIREMENTS.md
└── .gitignore
```

> **Nota:** no existe carpeta `/frontend` separada. Las vistas, CSS y JS están integrados dentro del proyecto MVC en `backend/AstronomiaApp/Views/` y `backend/AstronomiaApp/wwwroot/`.

### 3.2 Proyecto ASP.NET Core MVC

```
backend/
├── AstronomiaApp/
│   ├── Controllers/
│   │   ├── ObjetosController.cs      # MVC: devuelve Views (páginas)
│   │   ├── GrafoController.cs        # MVC: devuelve Views
│   │   └── Api/
│   │       ├── ObjetosApiController.cs  # API: devuelve JSON (para AJAX)
│   │       └── GrafoApiController.cs
│   ├── Views/
│   │   ├── Objetos/
│   │   │   ├── Index.cshtml          # Catálogo de objetos
│   │   │   ├── Detalle.cshtml        # Vista de un objeto
│   │   │   └── Busqueda.cshtml       # Resultados de búsqueda
│   │   ├── Grafo/
│   │   │   └── Index.cshtml          # Visualización del grafo
│   │   └── Shared/
│   │       ├── _Layout.cshtml        # Plantilla base (nav, footer)
│   │       └── _ValidationScripts.partial.cshtml
│   ├── Models/
│   │   ├── ObjetoAstronomico.cs      # Entidad EF Core
│   │   ├── Relacion.cs
│   │   └── ViewModels/
│   │       └── ObjetoViewModel.cs
│   ├── Services/
│   │   ├── ObjetoService.cs
│   │   └── GrafoService.cs
│   ├── Repositories/
│   │   └── ObjetoRepository.cs
│   ├── EstructurasDatos/
│   │   ├── ListaEnlazada.cs          # Manual
│   │   ├── TablaHash.cs              # Manual
│   │   ├── ArbolAVL.cs               # Manual
│   │   └── Grafo.cs                  # Manual
│   ├── Integracion/
│   │   └── SolarSystemApiClient.cs
│   ├── Data/
│   │   └── AstronomiaDbContext.cs    # EF Core DbContext
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── estilos.css
│   │   ├── js/
│   │   │   ├── grafo-viewer.js
│   │   │   └── api.js
│   │   └── assets/
│   ├── Program.cs
│   └── appsettings.json
├── AstronomiaApp.Tests/
└── AstronomiaApp.sln
```

---

## 4. Comunicación del equipo

### 4.1 Reuniones

- **Reunión semanal de sincronización:** 30–45 min. Cada integrante responde:
  1. ¿Qué hice la semana pasada?
  2. ¿Qué voy a hacer esta semana?
  3. ¿Dónde estoy bloqueado?

- **Demo rápida** al final de la reunión: cada uno muestra 2–3 minutos de su avance.

### 4.2 Canal de chat diario

- Usar WhatsApp / Discord / Telegram (acordar cuál).
- Avisar bloqueos en el mismo día, no acumular.

### 4.3 Tablero de tareas

- Usar **GitHub Projects** o **Trello**.
- Columnas: `Por hacer` → `En progreso` → `En revisión` → `Hecho`.
- Cada tarea asignada a un responsable.

---

## 5. Reglas de oro (para evitar problemas al final)

1. **Nunca hacer push directo a `main`.**
2. **Siempre hacer `pull` antes de empezar a trabajar.**
3. **Si cambias el modelo de datos o un endpoint, avisa al equipo antes de mergear.**
4. **Si vas a estar ausente más de 2 días, comunícalo antes.**
5. **Documentación se actualiza junto al código, no al final.**
6. **Si no entiendes algo del código de otro integrante, pregunta antes de modificarlo.**
7. **Los conflictos de merge se resuelven hablando, no borrando código ajeno.**

---

## 6. Herramientas recomendadas

| Propósito | Herramienta |
|-----------|-------------|
| IDE principal | Visual Studio 2022 (recomendado para MVC) / VS Code + C# Dev Kit |
| Base de datos | SQL Server Management Studio (SSMS) / Azure Data Studio |
| Cliente REST / pruebas API | Postman / Thunder Client (VS Code) |
| Diagramas | draw.io / Excalidraw |
| Git UI | GitHub Desktop / línea de comandos |

---

## 7. Puntos de sincronización obligatorios

- **Inicio de cada semana:** reunión de sync.
- **Fin de cada fase:** día de integración (levantar la app y probar end-to-end).
- **Antes de cada revisión:** ensayo de presentación.
- **Día de la revisión:** todos presentes, con el repositorio en estado estable.
