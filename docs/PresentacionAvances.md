# Sistema de Exploración y Análisis de Objetos Astronómicos
## Presentación de Avances — Primera Revisión

**Universidad Mariano Gálvez de Guatemala — Campus Cobán**
**Ingeniería en Sistemas y Ciencias de la Computación**
**Programación III**

---

## ¿Quiénes somos?

| Nombre | Rol en el proyecto |
|--------|--------------------|
| *(Integrante 1)* | Backend y Estructuras de Datos |
| *(Integrante 2)* | Base de Datos y consumo de API |
| *(Integrante 3)* | Vistas y Documentación |

---

## 1. ¿Qué problema resolvemos?

Hoy en día existe mucha información astronómica disponible en internet, pero está dispersa y es difícil de consultar para una persona común.

**Nuestra solución:** una aplicación web que permite explorar el universo de forma sencilla:
- Buscar planetas, estrellas, galaxias y exoplanetas
- Ver sus características físicas (tamaño, masa, temperatura, distancia)
- Entender cómo se relacionan entre sí
- Calcular rutas entre sistemas estelares

> Es como un buscador del universo, con datos reales obtenidos de fuentes científicas.

---

## 2. ¿Qué construimos?

Un sistema completo usando ASP.NET Core MVC — todo integrado en un solo proyecto .NET:

```
Usuario → Navegador → ASP.NET Core MVC (.NET 8) → SQL Server
                             ↑
                     API Astronómica
                (datos reales del espacio)
```

| Parte | ¿Qué hace? |
|-------|------------|
| **Vistas Razor (.cshtml)** | Lo que ve el usuario: páginas HTML generadas en el servidor |
| **Controllers + Services** | El cerebro: procesa datos, aplica estructuras, genera respuestas |
| **Base de Datos (SQL Server)** | Guarda toda la información de forma permanente |

---

## 3. Tecnologías elegidas

### ¿Por qué estas tecnologías?

| Tecnología | ¿Para qué? | ¿Por qué la elegimos? |
|------------|------------|----------------------|
| **C# / .NET 8** | Backend | Requerido por el curso. Lenguaje robusto y tipado |
| **ASP.NET Core MVC** | Framework web completo | Integra UI (Razor Views) y API en un solo proyecto .NET |
| **SQL Server** | Base de datos | Motor robusto de Microsoft, nativo con el ecosistema .NET |
| **Entity Framework Core** | Conexión BD | ORM solicitado por el catedrático. Simplifica el acceso a datos |
| **Razor Views (.cshtml)** | Interfaz web | Vistas generadas en servidor, integradas al proyecto MVC |
| **JavaScript (wwwroot/js/)** | Interactividad | Para visualización del grafo y llamadas AJAX a los API controllers |
| **Solar System OpenData API** | Datos reales | Gratuita, sin registro, datos oficiales del Sistema Solar |
| **GitHub** | Control de versiones | Requerido por el curso. Permite colaborar sin conflictos |

---

## 4. ¿Por qué elegimos este proyecto?

Seleccionamos la **Variante 8: Sistema Astronómico** porque:

- Los datos astronómicos se prestan perfectamente para aplicar **todas las estructuras del curso**
- Las relaciones entre planetas, estrellas y sistemas son naturalmente un **grafo**
- Ordenar objetos por masa o distancia es ideal para un **árbol AVL**
- Buscar por nombre en miles de objetos es el caso perfecto para una **tabla hash**
- Es un tema visualmente atractivo y fácil de presentar

---

## 5. Estructura del proyecto

El repositorio en GitHub está organizado así:

```
proyecto-astronomia-progra3/
│
├── backend/                → Proyecto ASP.NET Core MVC
│   ├── AstronomiaApp/      → App principal (Controllers, Views, Services, etc.)
│   └── prototipo-api/      → Prueba de consumo de API externa (ya listo para ejecutar)
│
├── database/               → Scripts SQL Server
│   ├── schema.sql          → Creación de tablas
│   └── seed.sql            → Datos iniciales de prueba
│
├── docs/                   → Toda la documentación del proyecto
│
├── README.md               → Guía general del proyecto
└── REQUIREMENTS.md         → Qué instalar para correr el proyecto
```

---

## 6. Base de datos — ¿Cómo guardamos la información?

Diseñamos 6 tablas relacionadas entre sí (SQL Server):

| Tabla | ¿Qué guarda? |
|-------|--------------|
| `objetos_astronomicos` | Planetas, estrellas, galaxias, exoplanetas |
| `tipos_objeto` | Catálogo de tipos (planeta, estrella, etc.) |
| `sistemas_planetarios` | Agrupaciones de objetos alrededor de una estrella |
| `constelaciones` | Las 88 constelaciones oficiales |
| `relaciones` | Vínculos entre objetos (para el grafo) |
| `consultas_log` | Registro de búsquedas del usuario |

**Ya tenemos:** script de creación de tablas y datos de prueba con los 8 planetas del Sistema Solar, exoplanetas de TRAPPIST-1 y sus relaciones.

---

## 7. Estructuras de datos — El corazón del sistema

Implementamos **6 estructuras**, 4 de ellas desde cero sin librerías:

| Estructura | ¿Cómo se usa en nuestro sistema? | ¿Manual o librería? |
|------------|----------------------------------|---------------------|
| **Lista enlazada** | Guarda el catálogo completo de objetos en memoria | Manual |
| **Tabla Hash** | Busca cualquier objeto por nombre en tiempo mínimo | Manual |
| **Árbol AVL** | Ordena objetos por distancia, masa o tamaño | Manual |
| **Grafo** | Conecta cuerpos celestes y calcula rutas entre sistemas | Manual |
| **Cola** | Procesa consultas del usuario en orden de llegada | Librería |
| **Pila** | Guarda el historial de objetos visitados | Librería |

### Ejemplo visual: cómo funciona el Grafo

```
    SOL ────────── Mercurio
     │
     ├──────────── Venus
     │
     ├──────────── Tierra
     │
     └──────────── Marte
     
     SOL ══════════ Próxima Centauri (4.2 años luz)
                          │
                    TRAPPIST-1 (40.6 años luz)
                    ├── TRAPPIST-1b
                    └── TRAPPIST-1e
```

Cada nodo es un cuerpo celeste. Las conexiones tienen peso (distancia). Con esto calculamos la ruta más corta entre dos sistemas.

---

## 8. Fuente de datos externa

Consumimos la **Solar System OpenData API**:
- URL: `https://api.le-systeme-solaire.net`
- Gratuita, sin API key
- Devuelve datos reales de todos los cuerpos del Sistema Solar

El prototipo está desarrollado y listo para ejecutarse. Consultará la API, recibirá los datos en formato JSON y los mostrará organizados por distancia al Sol.

Datos esperados de la API:

| Planeta | Radio (km) | Distancia al Sol | Lunas |
|---------|-----------|-----------------|-------|
| Mercurio | 2,439 | 57,909,050 km | 0 |
| Venus | 6,051 | 108,208,475 km | 0 |
| Tierra | 6,371 | 149,598,023 km | 1 |
| Marte | 3,389 | 227,939,200 km | 2 |
| Júpiter | 69,911 | 778,340,821 km | 79 |
| Saturno | 58,232 | 1,426,666,422 km | 83 |
| Urano | 25,362 | 2,870,658,186 km | 27 |
| Neptuno | 24,622 | 4,498,396,441 km | 14 |

---

## 9. Control de versiones — GitHub

Repositorio: **github.com/eurizar/proyecto-astronomia-progra3**

Flujo de trabajo del equipo:

```
main          ←── Solo código estable (merges aprobados)
  │
develop       ←── Integración del equipo
  │
feature/...   ←── Cada integrante trabaja en su rama
```

- Cada funcionalidad tiene su propia rama
- Nadie sube código directo a `main`
- Todo pasa por revisión antes de integrarse

---

## 10. Lo que tenemos hasta hoy

| Entregable | Estado |
|------------|--------|
| Descripción del problema y objetivos | ✅ Completo |
| Arquitectura del sistema (ASP.NET Core MVC) | ✅ Completo |
| Selección de 6 estructuras de datos | ✅ Completo |
| Diseño de base de datos (6 tablas, SQL Server) | ✅ Completo |
| Scripts SQL (schema + datos iniciales) | ✅ Completo |
| Prototipo de consumo de API | ✅ Desarrollado (prueba pendiente) |
| Repositorio GitHub con estructura | ✅ Publicado |
| Convenciones del equipo documentadas | ✅ Completo |

---

## 11. Próximos pasos — Fase 2

Lo que viene en la siguiente etapa:

1. **Implementar las 4 estructuras manuales** en C# (Lista, Hash, AVL, Grafo)
2. **Conectar el backend a SQL Server** con Entity Framework Core
3. **Consumir la API** y guardar los datos en la base de datos automáticamente
4. **Crear los Controllers MVC** con las vistas Razor para cada funcionalidad
5. **Primeras pruebas** de búsqueda y ordenamiento con datos reales

---

## Conclusiones de la Fase 1

- El equipo tiene claro **qué construir, cómo y con qué herramientas**
- La API externa elegida **ya fue probada y funciona correctamente**
- La base de datos está **diseñada y lista** para recibir datos
- Las estructuras de datos están **justificadas con casos de uso reales** del sistema
- El repositorio está **activo en GitHub** con historial de cambios

> El proyecto es viable, el diseño es sólido y el equipo está listo para la implementación.

---

*Programación III — Universidad Mariano Gálvez de Guatemala, Campus Cobán*
