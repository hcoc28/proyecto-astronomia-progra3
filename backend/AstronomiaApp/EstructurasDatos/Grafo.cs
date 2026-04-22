namespace AstronomiaApp.EstructurasDatos;

/// <summary>
/// Grafo no dirigido ponderado con lista de adyacencia.
/// Implementación manual. Soporta BFS, DFS y Dijkstra.
/// </summary>
public class Grafo
{
    private class Arista
    {
        public int Destino;
        public double Peso;
        public string TipoRelacion;
        public Arista(int destino, double peso, string tipo)
        {
            Destino = destino;
            Peso = peso;
            TipoRelacion = tipo;
        }
    }

    private readonly Dictionary<int, List<Arista>> _adyacencia = new();
    private readonly Dictionary<int, string> _nombres = new();

    public int CantidadNodos => _adyacencia.Count;
    public int CantidadAristas { get; private set; }

    // ── Construcción ────────────────────────────────────────────────────

    /// <summary>Agrega un nodo al grafo si no existe.</summary>
    public void AgregarNodo(int id, string nombre)
    {
        if (!_adyacencia.ContainsKey(id))
        {
            _adyacencia[id] = new List<Arista>();
            _nombres[id] = nombre;
        }
    }

    /// <summary>Agrega arista no dirigida entre dos nodos.</summary>
    public void AgregarArista(int origen, int destino, double peso, string tipoRelacion = "")
    {
        if (!_adyacencia.ContainsKey(origen) || !_adyacencia.ContainsKey(destino))
            return;

        _adyacencia[origen].Add(new Arista(destino, peso, tipoRelacion));
        _adyacencia[destino].Add(new Arista(origen, peso, tipoRelacion));
        CantidadAristas++;
    }

    /// <summary>Devuelve vecinos directos de un nodo.</summary>
    public IEnumerable<(int Id, string Nombre, string TipoRelacion, double Peso)> ObtenerVecinos(int id)
    {
        if (!_adyacencia.ContainsKey(id)) yield break;
        foreach (var a in _adyacencia[id])
            yield return (a.Destino, _nombres.GetValueOrDefault(a.Destino, ""), a.TipoRelacion, a.Peso);
    }

    public string ObtenerNombre(int id) => _nombres.GetValueOrDefault(id, "");
    public IEnumerable<int> ObtenerNodos() => _adyacencia.Keys;

    // ── BFS ─────────────────────────────────────────────────────────────

    /// <summary>BFS desde un nodo — devuelve IDs en orden de visita.</summary>
    public IEnumerable<int> BFS(int inicio)
    {
        if (!_adyacencia.ContainsKey(inicio)) yield break;

        var visitados = new HashSet<int>();
        var cola = new Queue<int>();
        cola.Enqueue(inicio);
        visitados.Add(inicio);

        while (cola.Count > 0)
        {
            int actual = cola.Dequeue();
            yield return actual;
            foreach (var arista in _adyacencia[actual])
            {
                if (visitados.Add(arista.Destino))
                    cola.Enqueue(arista.Destino);
            }
        }
    }

    // ── DFS ─────────────────────────────────────────────────────────────

    /// <summary>DFS desde un nodo — devuelve IDs en orden de visita.</summary>
    public IEnumerable<int> DFS(int inicio)
    {
        if (!_adyacencia.ContainsKey(inicio)) yield break;

        var visitados = new HashSet<int>();
        var pila = new Stack<int>();
        pila.Push(inicio);

        while (pila.Count > 0)
        {
            int actual = pila.Pop();
            if (!visitados.Add(actual)) continue;
            yield return actual;
            foreach (var arista in _adyacencia[actual])
            {
                if (!visitados.Contains(arista.Destino))
                    pila.Push(arista.Destino);
            }
        }
    }

    // ── Dijkstra ────────────────────────────────────────────────────────

    public class ResultadoRuta
    {
        public List<int> Ruta { get; set; } = new();
        public double DistanciaTotal { get; set; }
        public bool Encontrada { get; set; }
    }

    /// <summary>
    /// Dijkstra con min-heap (PriorityQueue .NET 6+).
    /// Devuelve la ruta de menor peso entre origen y destino.
    /// O((V + E) log V).
    /// </summary>
    public ResultadoRuta Dijkstra(int origen, int destino)
    {
        var resultado = new ResultadoRuta();
        if (!_adyacencia.ContainsKey(origen) || !_adyacencia.ContainsKey(destino))
            return resultado;

        var distancias = new Dictionary<int, double>();
        var anteriores = new Dictionary<int, int>();
        var pq = new PriorityQueue<int, double>();

        foreach (var nodo in _adyacencia.Keys)
            distancias[nodo] = double.MaxValue;

        distancias[origen] = 0;
        pq.Enqueue(origen, 0);

        while (pq.Count > 0)
        {
            int actual = pq.Dequeue();
            if (actual == destino) break;

            foreach (var arista in _adyacencia[actual])
            {
                double nuevaDist = distancias[actual] + arista.Peso;
                if (nuevaDist < distancias[arista.Destino])
                {
                    distancias[arista.Destino] = nuevaDist;
                    anteriores[arista.Destino] = actual;
                    pq.Enqueue(arista.Destino, nuevaDist);
                }
            }
        }

        if (distancias[destino] == double.MaxValue)
            return resultado; // sin ruta

        // Reconstruir camino
        var camino = new Stack<int>();
        int cur = destino;
        while (anteriores.ContainsKey(cur))
        {
            camino.Push(cur);
            cur = anteriores[cur];
        }
        camino.Push(origen);

        resultado.Ruta = camino.ToList();
        resultado.DistanciaTotal = distancias[destino];
        resultado.Encontrada = true;
        return resultado;
    }

    public void Limpiar()
    {
        _adyacencia.Clear();
        _nombres.Clear();
        CantidadAristas = 0;
    }
}
