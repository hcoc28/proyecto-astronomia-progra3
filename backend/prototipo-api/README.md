# Prototipo de Consumo de API Externa

Prueba funcional para la **Primera Revisión** del proyecto. Consume la API pública **Solar System OpenData** y muestra los planetas del Sistema Solar en consola, ordenados por distancia al Sol.

Este código **no es el backend final** — es solo una prueba aislada para validar que la API funciona, que podemos deserializar el JSON y que el equipo tiene el entorno de .NET funcionando.

---

## API utilizada

- **Nombre:** Solar System OpenData API
- **URL base:** https://api.le-systeme-solaire.net/rest
- **Endpoint probado:** `GET /bodies` — devuelve todos los cuerpos celestes conocidos del Sistema Solar.
- **Autenticación:** ninguna (API gratuita y abierta).
- **Formato:** JSON.

---

## Requisitos

- [.NET SDK 8.0 o superior](https://dotnet.microsoft.com/download)
- Conexión a internet

---

## Cómo ejecutar

```bash
cd backend/prototipo-api
dotnet run
```

### Salida esperada (ejemplo)

```
========================================================
  Prototipo de consumo - Solar System OpenData API
  Proyecto Astronomia - Programacion III
========================================================

GET https://api.le-systeme-solaire.net/rest/bodies

Total de cuerpos celestes recibidos: 294

PLANETAS DEL SISTEMA SOLAR (ordenados por distancia al Sol):
----------------------------------------------------------------
Nombre          Radio (km)      Distancia (km)       Lunas
----------------------------------------------------------------
Mercury         2,439           57,909,050           0
Venus           6,051           108,208,475          0
Earth           6,371           149,598,023          1
Mars            3,389           227,939,200          2
Jupiter         69,911          778,340,821          79
Saturn          58,232          1,426,666,422        83
Uranus          25,362          2,870,658,186        27
Neptune         24,622          4,498,396,441        14

Prueba completada con exito.
```

---

## ¿Qué demuestra esta prueba?

1. **Conexión a API externa funciona.** El endpoint responde y devuelve JSON válido.
2. **Deserialización correcta.** Los modelos C# (`Cuerpo`, `RespuestaBodies`) capturan los campos que necesitamos.
3. **Procesamiento básico.** Filtrado (solo planetas) y ordenamiento (por distancia) sin librerías externas.
4. **El equipo puede compilar y ejecutar .NET** en sus equipos.

---

## Siguientes pasos (Fase 2)

- Migrar el cliente HTTP a un servicio inyectable (`SolarSystemApiClient`).
- Persistir los datos obtenidos en PostgreSQL usando Entity Framework Core con el provider de Npgsql.
- Cargar los objetos en las estructuras de datos (Lista, Hash, AVL, Grafo).
- Exponer los datos mediante los endpoints REST descritos en [docs/api-contract.md](../../docs/api-contract.md).

---

## Captura de pantalla

> Al ejecutar `dotnet run`, tomar captura y guardarla en `docs/evidencia-api.png` para la entrega de la Primera Revisión.
