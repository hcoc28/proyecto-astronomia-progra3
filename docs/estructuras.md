# Selección de Estructuras de Datos

El proyecto implementará **6 estructuras de datos** (el mínimo exigido por el PDF es 5, con al menos 2 manuales). A continuación se detalla cada una, su uso en el sistema y la justificación de la elección.

---

## Resumen

| # | Estructura | Uso en el sistema | Implementación | Complejidad clave |
|---|------------|-------------------|----------------|-------------------|
| 1 | **Lista enlazada** | Catálogo dinámico de objetos astronómicos | **Manual** | Inserción O(1), recorrido O(n) |
| 2 | **Tabla Hash** | Búsqueda rápida por nombre del objeto | **Manual** | Búsqueda promedio O(1) |
| 3 | **Árbol AVL** | Ordenamiento por distancia / masa / luminosidad | **Manual** | Búsqueda / inserción O(log n) |
| 4 | **Cola (Queue)** | Procesamiento FIFO de consultas de usuario | Librería (`Queue<T>`) | Encolar / desencolar O(1) |
| 5 | **Grafo** | Relaciones entre cuerpos celestes (sistemas, rutas) | **Manual** | BFS/DFS O(V + E), Dijkstra O((V + E) log V) |
| 6 | **Pila (Stack)** | Historial de navegación del usuario | Librería (`Stack<T>`) | Push / Pop O(1) |

> **Cumplimiento del requisito:** las implementaciones manuales son **Lista, Hash, AVL y Grafo** — cuatro manuales, superando el mínimo de dos.

---

## 1. Lista enlazada (manual)

### Uso
Catálogo principal en memoria de todos los objetos astronómicos cargados desde la API. Soporta recorridos ordenados de inserción y sirve como fuente para las demás estructuras.

### Justificación
- Crecimiento dinámico sin reservar memoria fija por adelantado (manejo de memoria dinámica, requisito del curso).
- Simplicidad para el equipo: primera estructura en implementar y probar.
- Operaciones básicas cubren los casos de uso de "listado completo" y "recorrido".

### Operaciones implementadas
- `Agregar(ObjetoAstronomico o)` — O(1) si se mantiene referencia a la cola.
- `Eliminar(string nombre)` — O(n).
- `Buscar(string nombre)` — O(n) *(esto se delega a la tabla hash para eficiencia).*
- `ObtenerTodos()` — O(n).
- `Count` — O(1) manteniendo contador.

### Archivo futuro
`backend/EstructurasDatos/ListaEnlazada.cs`

---

## 2. Tabla Hash (manual)

### Uso
Índice para búsqueda inmediata de un objeto astronómico por su **nombre**. El usuario escribe "Marte" y se devuelve el objeto en O(1) promedio.

### Justificación
- El caso de uso "buscar por nombre" es el más frecuente en la UI.
- Una lista daría O(n); el hash da O(1) promedio.
- Implementación manual con manejo de colisiones por **encadenamiento separado** (lista enlazada dentro de cada bucket) — reutiliza la Lista enlazada del punto 1.

### Función hash
Función hash polinómica sobre los caracteres del nombre:
```
hash(s) = (Σ s[i] * 31^i) mod tamaño_tabla
```

### Operaciones implementadas
- `Insertar(string clave, ObjetoAstronomico valor)` — O(1) promedio.
- `Obtener(string clave)` — O(1) promedio.
- `Eliminar(string clave)` — O(1) promedio.
- `Redimensionar()` — se duplica el tamaño cuando el factor de carga > 0.75.

### Archivo futuro
`backend/EstructurasDatos/TablaHash.cs`

---

## 3. Árbol AVL (manual)

### Uso
Mantener objetos astronómicos ordenados por atributos numéricos para:
- Listar planetas del más cercano al más lejano (ordenamiento por distancia al Sol).
- Mostrar las estrellas más masivas / luminosas.
- Buscar rangos: "estrellas con masa entre X e Y".

### Justificación
- El AVL mantiene las operaciones balanceadas a O(log n), crítico cuando tengamos miles de exoplanetas.
- Un árbol binario sin balancear podría degenerar en lista en casos ordenados.
- Es una estructura clásica del curso — oportunidad para demostrar dominio del contenido.

### Operaciones implementadas
- `Insertar(double clave, ObjetoAstronomico valor)` — O(log n).
- `Eliminar(double clave)` — O(log n).
- `BuscarRango(double min, double max)` — O(log n + k) donde k = resultados.
- `RecorridoInOrden()` — O(n), devuelve lista ordenada.
- Rotaciones simples (LL, RR) y dobles (LR, RL).

### Archivo futuro
`backend/EstructurasDatos/ArbolAVL.cs`

---

## 4. Cola (librería `Queue<T>`)

### Uso
Procesar consultas pesadas del usuario en orden FIFO. Ejemplo: si el usuario pide "calcular distancias a todas las estrellas en un radio de 100 años luz", esa tarea entra a la cola y se procesa en segundo plano.

### Justificación
- Para funcionalidad básica, la implementación de la librería estándar es suficiente y robusta.
- El PDF pide que al menos 2 estructuras sean manuales, no las 5 — la cola puede ir con librería sin penalización.
- Si el tiempo lo permite, se puede implementar manualmente en la Fase 2.

### Operaciones utilizadas
- `Enqueue(Consulta c)` — O(1).
- `Dequeue()` — O(1).
- `Peek()` — O(1).

### Archivo futuro
`backend/Servicios/ProcesadorConsultas.cs`

---

## 5. Grafo (manual)

### Uso
Representar relaciones entre cuerpos celestes:
- **Nodos:** planetas, estrellas, sistemas planetarios.
- **Aristas:** relaciones gravitacionales / distancias en años luz / pertenencia a sistemas.

Casos de uso:
- Calcular la ruta más corta entre dos sistemas estelares (Dijkstra).
- Explorar todos los planetas de un sistema (BFS/DFS).
- Identificar sistemas aislados (componentes conexos).

### Justificación
- Es la estructura más poderosa para modelar datos del espacio — el requisito específico de la Variante 8 menciona grafos explícitamente.
- Implementación con **lista de adyacencia** (eficiente en memoria para grafos dispersos, que es el caso de sistemas estelares).
- Base para algoritmos que enriquecen el análisis (diferenciadores frente a otros proyectos).

### Algoritmos implementados
- BFS — exploración por niveles.
- DFS — exploración profunda (iterativa con pila).
- Dijkstra — ruta mínima con pesos.
- Componentes conexos.

### Archivo futuro
`backend/EstructurasDatos/Grafo.cs`

---

## 6. Pila (librería `Stack<T>`)

### Uso
Historial de objetos visitados en el frontend. Cuando el usuario hace clic en "anterior", se hace `Pop` del último objeto visto.

### Justificación
- Uso auxiliar / funcional, no crítico para análisis.
- Librería estándar suficiente.
- Cumple con el requisito del PDF de usar pila.

### Operaciones utilizadas
- `Push(string idObjeto)` — O(1).
- `Pop()` — O(1).
- `Peek()` — O(1).

### Archivo futuro
`backend/Servicios/HistorialNavegacion.cs`

---

## Mapeo estructuras → funcionalidades del sistema

| Funcionalidad del sistema | Estructura(s) utilizada(s) |
|---------------------------|----------------------------|
| Listar catálogo completo | Lista enlazada |
| Buscar objeto por nombre | Tabla Hash |
| Ordenar por masa / distancia | Árbol AVL |
| Búsqueda por rango de valores | Árbol AVL |
| Procesar consultas pesadas | Cola |
| Calcular ruta entre sistemas | Grafo (Dijkstra) |
| Explorar un sistema planetario | Grafo (BFS/DFS) |
| Historial de navegación | Pila |

---

## Consideraciones de implementación

### Genéricos
Las estructuras manuales se implementarán usando **genéricos de C#** (`<T>`) para reutilización. Ejemplo:
```csharp
public class ListaEnlazada<T> { ... }
public class TablaHash<TClave, TValor> { ... }
public class ArbolAVL<TClave, TValor> where TClave : IComparable<TClave> { ... }
```

### Pruebas unitarias
Cada estructura manual tendrá una clase de pruebas en `backend/Tests/` con casos para:
- Operaciones básicas (insertar, eliminar, buscar).
- Casos borde (vacío, un elemento, duplicados).
- Escalabilidad (insertar 10,000 elementos).

### Manejo de memoria dinámica
- C# es gestionado (GC), pero se documentará cuándo se liberan referencias para permitir recolección.
- Las estructuras manuales evitarán fugas por referencias cíclicas (importante en el grafo).
