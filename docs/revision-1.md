# Primera Revisión — Planeación y Diseño del Sistema

**Universidad Mariano Gálvez de Guatemala — Campus Cobán**
**Ingeniería en Sistemas y Ciencias de la Computación**
**Curso:** Programación III
**Proyecto:** Sistema de Exploración y Análisis de Objetos Astronómicos (Variante 8)
**Fecha de entrega:** *(completar)*

---

## Integrantes del equipo

| Nombre completo | Carné | Rol principal | Usuario GitHub |
|-----------------|-------|---------------|----------------|
| *(Integrante 1)* | *(pendiente)* | Backend & Estructuras de Datos | *(pendiente)* |
| *(Integrante 2)* | *(pendiente)* | Base de Datos & API Externa | *(pendiente)* |
| *(Integrante 3)* | *(pendiente)* | Frontend & Documentación | `eurizar` |

---

## 1. Descripción del problema

En la actualidad existen vastas cantidades de datos astronómicos dispersos en catálogos científicos, APIs públicas y datasets abiertos. Si bien la información es accesible, **no existe una herramienta simple y unificada** que permita a estudiantes, aficionados y curiosos:

- Explorar cuerpos celestes de forma visual.
- Comparar propiedades físicas (masa, tamaño, distancia, temperatura).
- Entender las **relaciones** entre objetos (qué orbita qué, qué está cerca de qué).
- Calcular rutas entre sistemas estelares.
- Consultar todo lo anterior desde una interfaz amigable, sin necesidad de conocimientos técnicos.

### Solución propuesta

Un sistema web que:
1. **Obtiene** información astronómica desde APIs públicas (Solar System OpenData) y datasets.
2. **Procesa** los datos mediante estructuras de datos avanzadas (listas, tablas hash, árboles AVL, colas, grafos).
3. **Almacena** la información en una base de datos PostgreSQL.
4. **Presenta** los datos al usuario con opciones de búsqueda, filtrado, ordenamiento y visualización de relaciones.

---

## 2. Objetivos

### 2.1 Objetivo general
Desarrollar una aplicación web completa que integre los conocimientos de estructuras de datos, programación orientada a objetos, persistencia en base de datos y arquitectura por capas, para la exploración y análisis de objetos astronómicos.

### 2.2 Objetivos específicos

- Implementar **al menos 5 estructuras de datos** estudiadas en el curso, con **mínimo 2 implementadas manualmente** sin librerías predefinidas.
- Consumir datos desde al menos **una API pública** astronómica.
- **Persistir** los datos en PostgreSQL con un modelo relacional bien diseñado.
- Exponer un **backend REST en C#** (ASP.NET Core) que sirva como punto de acceso único para el frontend.
- Construir una **interfaz web** funcional con HTML, CSS y JavaScript.
- Aplicar **programación orientada a objetos** en toda la lógica de negocio.
- Usar **GitHub** con ramas, commits frecuentes y Pull Requests para el control de versiones.
- Implementar **algoritmos representativos** (ordenamiento, búsqueda por hash, búsqueda por rango con AVL, BFS/DFS/Dijkstra en el grafo).

---

## 3. Arquitectura del sistema

El sistema sigue una **arquitectura de 3 capas** con un módulo adicional de integración con fuentes externas.

- **Frontend (presentación):** HTML + CSS + JavaScript vanilla.
- **Backend (lógica):** C# con ASP.NET Core Web API.
- **Base de datos (persistencia):** PostgreSQL 15+.
- **Módulo de integración:** consume APIs externas y carga datos en la BD y las estructuras.

Diagrama completo y descripción detallada en [arquitectura.md](arquitectura.md).

### Decisiones arquitectónicas clave

| Decisión | Justificación |
|----------|---------------|
| Arquitectura de 3 capas | Exigida por el PDF del curso; separa responsabilidades. |
| REST + JSON | Estándar simple, consumible desde JS vanilla. |
| C# / .NET 8 | Requisito del curso. LTS con excelente soporte PostgreSQL. |
| PostgreSQL | Requisito del curso. Soporta tipos numéricos precisos. |
| JS vanilla (sin framework) | Requisito del curso. Mantiene foco en las estructuras de datos. |
| Estructuras en memoria + BD | BD para persistencia; estructuras para análisis rápido. |

---

## 4. Selección de estructuras de datos

Se implementarán **6 estructuras** (una más que el mínimo exigido), con **4 manuales** (superando el mínimo de 2).

| # | Estructura | Uso | Implementación |
|---|------------|-----|----------------|
| 1 | Lista enlazada | Catálogo dinámico de objetos | **Manual** |
| 2 | Tabla Hash | Búsqueda rápida por nombre | **Manual** |
| 3 | Árbol AVL | Ordenamiento por atributos numéricos | **Manual** |
| 4 | Cola | Procesamiento de consultas | Librería |
| 5 | Grafo | Relaciones entre cuerpos celestes | **Manual** |
| 6 | Pila | Historial de navegación | Librería |

Justificación detallada, operaciones por estructura y mapeo a funcionalidades del sistema en [estructuras.md](estructuras.md).

---

## 5. Diseño de base de datos

### 5.1 Tablas principales

- **`tipos_objeto`** — catálogo de tipos (planeta, estrella, galaxia, etc.).
- **`objetos_astronomicos`** — tabla principal del sistema.
- **`sistemas_planetarios`** — agrupación de objetos alrededor de una estrella.
- **`constelaciones`** — catálogo de constelaciones (IAU).
- **`relaciones`** — aristas del grafo (vínculos entre objetos).
- **`consultas_log`** — auditoría de consultas del usuario.

### 5.2 Scripts SQL

- **Creación de esquema:** [`database/schema.sql`](../database/schema.sql).
- **Datos iniciales:** [`database/seed.sql`](../database/seed.sql) (7 tipos, 5 constelaciones, 3 sistemas, 13 objetos, 8 relaciones).

Modelo ER completo, descripción de tablas, índices y consultas representativas en [modelo-datos.md](modelo-datos.md).

---

## 6. Consumo inicial del API

### 6.1 API seleccionada

**Solar System OpenData API** — https://api.le-systeme-solaire.net

**Razones:**
- Gratuita, sin API key ni cuota.
- Datos oficiales y actualizados del Sistema Solar.
- JSON simple y bien estructurado.
- Documentación clara.

### 6.2 Prototipo funcional

Se desarrolló un prototipo en C# que consulta el endpoint `/bodies`, deserializa la respuesta, filtra los planetas y los muestra ordenados por distancia al Sol.

- **Código:** [`backend/prototipo-api/Program.cs`](../backend/prototipo-api/Program.cs)
- **Ejecución:** `cd backend/prototipo-api && dotnet run`
- **Documentación:** [`backend/prototipo-api/README.md`](../backend/prototipo-api/README.md)

### 6.3 Evidencia de ejecución

> Al correr el prototipo se obtiene una tabla con los 8 planetas del Sistema Solar con su radio medio, distancia al Sol y número de lunas. *(Agregar captura de pantalla como `docs/evidencia-api.png` antes de la entrega.)*

---

## 7. Repositorio GitHub

- **URL:** https://github.com/eurizar/proyecto-astronomia-progra3
- **Estructura de carpetas:**
  ```
  /backend     → código C#
  /frontend    → interfaz web
  /database    → scripts SQL
  /docs        → documentación
  ```
- **Ramas configuradas:** `main` (producción), `develop` (integración).
- **Convenciones documentadas** en [convenciones.md](convenciones.md).
- **Contrato de API** para la integración backend ↔ frontend en [api-contract.md](api-contract.md).

---

## 8. Cronograma del proyecto

### Fase actual: **Fase 1 — Planeación y Diseño** (entrega: Primera Revisión)

| Fase | Duración estimada | Entregable | Evaluación |
|------|-------------------|------------|------------|
| **Fase 1** (actual) | 1–2 semanas | Planeación y diseño documentado | Primera Revisión |
| Fase 2 | 2–3 semanas | Backend con estructuras + BD + API funcional | Segunda Revisión |
| Fase 3 | 2 semanas | Frontend integrado + operaciones completas | Tercera Revisión |
| Fase 4 | 1 semana | Pruebas, documentación PDF, presentación | Presentación final |

Cronograma detallado de la Fase 1 en [PlanDeTrabajo.md](../PlanDeTrabajo.md).

---

## 9. Distribución de responsabilidades

| Integrante | Rol principal | Responsabilidades en Fase 1 |
|------------|---------------|------------------------------|
| Int. 1 | Backend & Estructuras | Arquitectura, selección de estructuras, apoyo en prototipo API |
| Int. 2 | BD & API | Modelo de datos, scripts SQL, prototipo de consumo API |
| Int. 3 | Frontend & Docs | Repositorio GitHub, README, redacción del documento |

---

## 10. Checklist de entrega — Primera Revisión

- [x] Descripción del problema
- [x] Arquitectura del sistema (con diagrama)
- [x] Selección de estructuras de datos (con justificación)
- [x] Diseño de base de datos (ER + SQL)
- [x] Consumo inicial de API (prototipo funcional)
- [x] Repositorio GitHub creado con estructura de carpetas
- [ ] Captura de ejecución del prototipo como evidencia
- [ ] Presentación / sustentación ante el catedrático

---

## 11. Conclusiones parciales de la Fase 1

1. El equipo definió un **alcance claro** alineado con la Variante 8 del documento de requerimientos.
2. Se seleccionaron estructuras de datos cuyo uso tiene **justificación funcional** en el sistema, no solo académica.
3. Se estableció una **arquitectura escalable** que permite desarrollar las tres capas en paralelo una vez listo el contrato de API.
4. El **prototipo de consumo de API** demuestra que la fuente de datos elegida es viable y deserializable.
5. El equipo cuenta con **convenciones y flujo de Git** que previenen conflictos de integración al final del proyecto.

---

## Anexos

- [PlanDeTrabajo.md](../PlanDeTrabajo.md) — plan general del proyecto.
- [arquitectura.md](arquitectura.md) — arquitectura detallada.
- [estructuras.md](estructuras.md) — estructuras de datos.
- [modelo-datos.md](modelo-datos.md) — modelo de BD.
- [api-contract.md](api-contract.md) — contrato interno de API.
- [convenciones.md](convenciones.md) — convenciones del equipo.
