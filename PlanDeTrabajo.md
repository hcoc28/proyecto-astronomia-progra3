# Plan de Trabajo — Proyecto Final Programación III

**Universidad Mariano Gálvez de Guatemala — Campus Cobán**
**Ingeniería en Sistemas y Ciencias de la Computación**

---

## 1. Información General del Proyecto

| Campo | Detalle |
|-------|---------|
| **Variante seleccionada** | Variante 8 — Sistema de Exploración y Análisis de Objetos Astronómicos |
| **Modalidad** | Grupo de 3 integrantes |
| **Framework** | ASP.NET Core MVC (.NET 8) |
| **Base de Datos** | SQL Server |
| **Interfaz de usuario** | Razor Views (.cshtml) + CSS + JavaScript |
| **Control de versiones** | GitHub |
| **Evaluaciones** | 3 revisiones parciales + presentación final |

### 1.1 Descripción breve del sistema

Aplicación web construida con ASP.NET Core MVC que consume información astronómica desde APIs externas (Solar System OpenData API), la procesa usando estructuras de datos avanzadas y la almacena en SQL Server. Los usuarios interactúan directamente con vistas Razor generadas por el servidor, con soporte adicional de JavaScript para funcionalidades dinámicas como la visualización del grafo.

### 1.2 Estructuras de datos a implementar

Mínimo 5 estructuras (al menos 2 implementadas manualmente, sin librerías):

| Estructura | Uso en el sistema | Implementación |
|------------|-------------------|----------------|
| Lista enlazada | Catálogo dinámico de objetos astronómicos | **Manual** |
| Tabla Hash | Búsqueda rápida por nombre | **Manual** |
| Árbol AVL | Ordenamiento por distancia / tamaño / luminosidad | Manual |
| Cola | Procesamiento de consultas del usuario | Librería o manual |
| Grafo | Relaciones entre cuerpos celestes (sistemas planetarios, rutas) | Manual |
| Pila *(opcional)* | Historial de consultas / navegación | Librería |

---

## 2. Equipo y Roles

Para evitar problemas de integración al final, cada integrante tiene un **rol principal** (responsable) y un **rol secundario** (soporte). Todos conocen todas las capas, pero uno es dueño de cada una.

| Integrante | Rol Principal | Rol Secundario | Responsabilidades clave |
|------------|---------------|----------------|-------------------------|
| **Integrante 1** | Backend & Estructuras de Datos | Integración API externa | Implementar estructuras (Lista, AVL), lógica de negocio, Controllers API |
| **Integrante 2** | Base de Datos & API Externa | Backend (Hash, Grafo) | Diseño BD, scripts SQL Server, consumo API, DbContext EF Core |
| **Integrante 3** | Vistas & Documentación | QA / Pruebas | Razor Views, CSS/JS, documentación y README |

> Nota: los roles **secundarios** existen para que nadie quede bloqueado si un integrante no entrega a tiempo.

---

## 3. Plan General — 4 Fases

| Fase | Entregable | Duración sugerida | Revisión |
|------|-----------|-------------------|----------|
| **Fase 1** | Planeación y Diseño | 1–2 semanas | Primera revisión |
| **Fase 2** | Implementación del Backend | 2–3 semanas | Segunda revisión |
| **Fase 3** | Integración Completa (Vistas + Lógica + BD) | 2 semanas | Tercera revisión |
| **Fase 4** | Pruebas, Documentación y Presentación | 1 semana | Presentación final |

---

## 4. FASE 1 — Planeación y Diseño del Sistema (PRIMERA ETAPA)

> **Esta es la etapa que entrega la Primera Revisión.**

### 4.1 Objetivo de la fase

Dejar definido el **qué**, el **cómo** y el **quién** antes de escribir código de producción. Al cerrar esta fase, el grupo debe tener: documento de diseño, repositorio GitHub configurado, modelo de base de datos, arquitectura del sistema, elección de API externa funcionando en un prototipo mínimo y estructuras de datos seleccionadas y justificadas.

### 4.2 Entregables obligatorios (según rúbrica del PDF)

- [ ] Descripción del problema
- [ ] Arquitectura del sistema (diagrama)
- [ ] Selección de estructuras de datos (con justificación)
- [ ] Diseño de base de datos (modelo entidad-relación + script SQL inicial)
- [ ] Consumo inicial del API (prueba funcional mínima)
- [ ] Repositorio GitHub creado con estructura de carpetas

### 4.3 Tareas detalladas

#### Tarea 1.1 — Definir alcance y problema a resolver
**Responsable:** Todos (sesión grupal de 1–2 horas)
**Salida:** Sección "Descripción del problema" y "Objetivos" del documento.

Decisiones a tomar:
- ¿Qué objetos astronómicos cubre el sistema? (planetas, estrellas, exoplanetas, galaxias — se recomienda empezar con planetas + estrellas + exoplanetas).
- ¿Qué operaciones mínimas ofrece? (consulta, búsqueda por nombre/tipo, ordenamiento, cálculo de distancias, análisis de sistemas planetarios).
- ¿Cuáles son las funcionalidades opcionales? (mapa de constelaciones, simulación de rutas).

#### Tarea 1.2 — Crear repositorio GitHub y configurar estructura
**Responsable:** Integrante 3
**Apoyo:** Integrante 1
**Salida:** Repo público con carpetas y README inicial.

Pasos concretos:
1. Crear repo `proyecto-astronomia-progra3` (o nombre consensuado).
2. Estructura obligatoria del PDF:
   ```
   /backend
   /database
   /docs
   README.md
   .gitignore
   ```
3. Agregar a los 3 integrantes como colaboradores.
4. Crear ramas base:
   - `main` → solo código estable (merges por PR).
   - `develop` → rama de integración.
   - `feature/<nombre>` → una rama por funcionalidad/integrante.
5. Proteger `main`: requerir Pull Request antes de merge.
6. Primer commit: README con descripción del proyecto e integrantes.

#### Tarea 1.3 — Diseñar arquitectura del sistema
**Responsable:** Integrante 1
**Apoyo:** Integrante 2
**Salida:** Diagrama de arquitectura en `/docs/arquitectura.md`.

Contenido mínimo:
- Diagrama MVC: navegador → Controllers → Views/Services → EF Core → SQL Server.
- Módulo de integración con APIs externas.
- Módulo de estructuras de datos dentro del backend.
- Flujo de datos: API externa → Backend → Estructuras → BD → Razor View.

Herramientas sugeridas: draw.io, Excalidraw o Mermaid.

#### Tarea 1.4 — Seleccionar y justificar estructuras de datos
**Responsable:** Integrante 1
**Apoyo:** Todos (validación)
**Salida:** Documento `/docs/estructuras.md`.

Debe incluir, por cada estructura:
- Nombre y tipo.
- Dónde se usa en el sistema (caso concreto).
- Por qué se elige (complejidad, operaciones esperadas).
- Si es manual o con librería.

#### Tarea 1.5 — Diseñar base de datos
**Responsable:** Integrante 2
**Salida:** Modelo ER + script SQL en `/database/schema.sql`.

Tablas mínimas sugeridas (revisar según alcance final):
- `objetos_astronomicos` (id, nombre, tipo, masa, radio, distancia, temperatura, etc.)
- `sistemas_planetarios` (id, nombre, estrella_central_id)
- `relaciones` (origen_id, destino_id, tipo_relacion, distancia) — para el grafo
- `constelaciones` (id, nombre, descripcion) *(opcional)*
- `consultas_log` (id, usuario, consulta, fecha) *(opcional, para usar cola/pila)*

Entregables:
- Diagrama ER (`/docs/modelo-datos.md`).
- Script `schema.sql` con `CREATE TABLE` + constraints (sintaxis SQL Server).
- Script `seed.sql` con datos de prueba mínimos.

#### Tarea 1.6 — Seleccionar API externa y hacer prueba de consumo
**Responsable:** Integrante 2
**Apoyo:** Integrante 1
**Salida:** Script C# mínimo que consulta la API y muestra resultados en consola. Guardar en `/backend/prototipo-api/`.

API seleccionada:
- Solar System OpenData API — https://api.le-systeme-solaire.net (gratuita, sin API key).

Prueba mínima:
- Hacer GET a un endpoint (ej. `/bodies`).
- Deserializar JSON a una clase C#.
- Imprimir 5 objetos en consola.

#### Tarea 1.7 — Definir convenciones del equipo
**Responsable:** Todos
**Salida:** `/docs/convenciones.md`.

Debe incluir:
- **Convención de commits:** formato `tipo(scope): mensaje` (feat, fix, docs, refactor, test).
- **Convención de ramas:** `feature/<integrante>-<funcionalidad>`.
- **Revisión de PR:** mínimo 1 revisor antes de merge a `develop`.
- **Nomenclatura de código:** PascalCase para clases C#, camelCase para variables, kebab-case para archivos web.
- **Estilo de documentación:** comentarios XML en C# para clases públicas.
- **Frecuencia de sync:** reunión semanal + chat diario de bloqueos.

#### Tarea 1.8 — Redactar documento de la Primera Revisión
**Responsable:** Integrante 3
**Apoyo:** Todos
**Salida:** `/docs/revision-1.md` (o PDF final).

Secciones obligatorias:
1. Portada (universidad, curso, integrantes, fecha).
2. Descripción del problema.
3. Objetivos (general + específicos).
4. Arquitectura del sistema (con diagrama).
5. Selección de estructuras de datos.
6. Diseño de base de datos (modelo ER + SQL).
7. Prueba de consumo de API (captura de pantalla).
8. Enlace al repositorio GitHub.
9. Cronograma de fases siguientes.

---

## 5. Cronograma de la Fase 1 (sugerido, 2 semanas)

| Semana | Día | Tarea | Responsable |
|--------|-----|-------|-------------|
| 1 | Lun | Reunión inicial + definir alcance (1.1) | Todos |
| 1 | Mar | Crear repo + estructura (1.2) | Int. 3 |
| 1 | Mié–Jue | Diseño arquitectura (1.3) + elección estructuras (1.4) | Int. 1 |
| 1 | Vie | Modelo de BD (1.5) | Int. 2 |
| 1 | Sáb | Prototipo consumo API (1.6) | Int. 2 |
| 2 | Lun | Revisión conjunta de avances | Todos |
| 2 | Mar | Convenciones del equipo (1.7) | Todos |
| 2 | Mié–Jue | Redacción documento revisión (1.8) | Int. 3 |
| 2 | Vie | Revisión final + push de entregables | Todos |
| 2 | Sáb | Ensayo corto de presentación | Todos |

---

## 6. Estrategia de Integración (para evitar problemas al final)

Esta es la parte crítica. Los conflictos en proyectos grupales aparecen por: ramas desordenadas, contratos no definidos, nombres inconsistentes y falta de pruebas cruzadas.

### 6.1 Contratos de rutas internas (Controllers)

Antes de que el Integrante 3 empiece las vistas, Integrantes 1 y 2 deben publicar un archivo `/docs/api-contract.md` con las rutas MVC y los endpoints API que expondrá el backend. Ejemplo:

```
GET  /Objetos              → vista: catálogo de todos los objetos
GET  /Objetos/{id}         → vista: detalle de un objeto
GET  /Objetos/Buscar?nombre=...  → vista: resultados de búsqueda
GET  /Objetos/Ordenar?por=distancia  → vista: listado ordenado
GET  /Grafo                → vista: visualización del grafo
GET  /api/grafo/ruta?origen=...&destino=...  → JSON: ruta Dijkstra
POST /api/consultas        → JSON: encola consulta
```

Con rutas definidas desde el inicio, los integrantes pueden avanzar con vistas mock mientras el backend se implementa.

### 6.2 Reglas de Git

- **Nunca** hacer push directo a `main`.
- Cada tarea se hace en su rama `feature/...` y se fusiona a `develop` vía Pull Request.
- Antes de cada PR: `git pull origin develop` para evitar conflictos gigantes.
- Commits pequeños y frecuentes (mínimo 1 por día de trabajo).

### 6.3 Checkpoints semanales

- **Reunión de sincronización** cada semana (30–45 min):
  - Qué hice / qué voy a hacer / dónde estoy bloqueado.
  - Demo rápida del avance propio.
  - Revisión del tablero de tareas (GitHub Projects o Trello).

### 6.4 Pruebas de integración temprana

Al terminar la **Fase 2**, los 3 integrantes deben hacer un **día de integración**: levantar la app MVC + BD + datos reales en la misma máquina, navegar la UI completa y documentar lo que falla. Si algo falla, se arregla antes de avanzar.

### 6.5 Documentación continua

Cada PR debe actualizar `README.md` y `/docs` si afecta arquitectura, rutas o estructuras. No dejar documentación para el final.

---

## 7. Resumen de las Fases 2, 3 y 4 (vista previa)

### Fase 2 — Implementación del Backend (Segunda Revisión)
- Implementación de las 5+ estructuras de datos (2+ manuales).
- Conexión a SQL Server con Entity Framework Core (`Microsoft.EntityFrameworkCore.SqlServer`).
- Consumo funcional del API con almacenamiento en BD.
- Controllers MVC funcionando con datos reales.
- Controllers API devolviendo JSON para AJAX.
- Pruebas unitarias básicas de estructuras manuales.

### Fase 3 — Integración Completa (Tercera Revisión)
- Vistas Razor completas: catálogo, búsqueda, detalle, grafo.
- Funcionalidades end-to-end operativas (buscar, ordenar, ver rutas).
- JavaScript para interactividad (visualización del grafo en canvas/SVG).
- Uso visible de estructuras de datos para análisis en pantalla.

### Fase 4 — Pruebas, Documentación y Presentación Final
- Pruebas end-to-end.
- Documento PDF final (Introducción, Problema, Objetivos, Arquitectura, BD, Estructuras, Algoritmos, Resultados, Conclusiones).
- Presentación de 10–15 min (explicación + demo en vivo).
- Limpieza del repo y versionado final (tag `v1.0`).

---

## 8. Criterios de Evaluación (recordatorio del PDF)

| Criterio | Porcentaje |
|----------|------------|
| Implementación de estructuras de datos | 30% |
| Funcionamiento del sistema | 25% |
| Diseño del sistema | 20% |
| Documentación | 15% |
| Presentación final | 10% |

> **Implicación estratégica:** el 30% se lo llevan las estructuras de datos. Invertir tiempo de calidad en Lista, Hash, AVL y Grafo manuales es la apuesta más rentable. No subestimar pruebas y documentación del funcionamiento interno de cada estructura.

---

## 9. Checklist de Entrega — Primera Revisión

Antes de presentar la Fase 1, verificar que todo lo siguiente esté en el repositorio:

- [ ] `README.md` con descripción, integrantes, instrucciones de instalación.
- [ ] `/docs/arquitectura.md` con diagrama.
- [ ] `/docs/estructuras.md` con justificación de cada estructura.
- [ ] `/docs/modelo-datos.md` + `/database/schema.sql` + `/database/seed.sql`.
- [ ] `/backend/prototipo-api/` con prueba funcional de consumo API.
- [ ] `/docs/api-contract.md` con rutas y endpoints planeados.
- [ ] `/docs/convenciones.md` con reglas del equipo.
- [ ] `/docs/revision-1.md` o PDF con el documento completo de la revisión.
- [ ] Ramas `main`, `develop` y al menos 1 `feature/...` creadas.
- [ ] Al menos 5 commits por integrante (evidencia de colaboración).

---

**Última actualización:** 2026-04-21
**Siguiente reunión recomendada:** acordar fecha de kickoff con los 3 integrantes esta semana.
