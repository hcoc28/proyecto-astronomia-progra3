namespace AstronomiaApp.EstructurasDatos;

/// <summary>
/// Árbol AVL genérico con clave double y valor genérico.
/// Soporta múltiples elementos con igual clave (lista por nodo).
/// Implementación manual. Operaciones: O(log n).
/// </summary>
public class ArbolAVL<TValor>
{
    private class Nodo
    {
        public double Clave;
        public List<TValor> Valores = new();
        public Nodo? Izquierdo;
        public Nodo? Derecho;
        public int Altura;

        public Nodo(double clave, TValor valor)
        {
            Clave = clave;
            Valores.Add(valor);
            Altura = 1;
        }
    }

    private Nodo? _raiz;
    public int Tamanio { get; private set; }

    // ── Utilidades ──────────────────────────────────────────────────────

    private static int Altura(Nodo? n) => n?.Altura ?? 0;

    private static int FactorBalance(Nodo n) => Altura(n.Izquierdo) - Altura(n.Derecho);

    private static void ActualizarAltura(Nodo n) =>
        n.Altura = 1 + Math.Max(Altura(n.Izquierdo), Altura(n.Derecho));

    // ── Rotaciones ──────────────────────────────────────────────────────

    private static Nodo RotarDerecha(Nodo y)
    {
        var x = y.Izquierdo!;
        var t2 = x.Derecho;
        x.Derecho = y;
        y.Izquierdo = t2;
        ActualizarAltura(y);
        ActualizarAltura(x);
        return x;
    }

    private static Nodo RotarIzquierda(Nodo x)
    {
        var y = x.Derecho!;
        var t2 = y.Izquierdo;
        y.Izquierdo = x;
        x.Derecho = t2;
        ActualizarAltura(x);
        ActualizarAltura(y);
        return y;
    }

    private static Nodo Balancear(Nodo n)
    {
        ActualizarAltura(n);
        int fb = FactorBalance(n);

        if (fb > 1)
        {
            if (FactorBalance(n.Izquierdo!) < 0)
                n.Izquierdo = RotarIzquierda(n.Izquierdo!);
            return RotarDerecha(n);
        }
        if (fb < -1)
        {
            if (FactorBalance(n.Derecho!) > 0)
                n.Derecho = RotarDerecha(n.Derecho!);
            return RotarIzquierda(n);
        }
        return n;
    }

    // ── Insertar ────────────────────────────────────────────────────────

    /// <summary>Inserta elemento. O(log n).</summary>
    public void Insertar(double clave, TValor valor)
    {
        _raiz = Insertar(_raiz, clave, valor);
        Tamanio++;
    }

    private static Nodo Insertar(Nodo? nodo, double clave, TValor valor)
    {
        if (nodo == null) return new Nodo(clave, valor);

        if (clave < nodo.Clave)
            nodo.Izquierdo = Insertar(nodo.Izquierdo, clave, valor);
        else if (clave > nodo.Clave)
            nodo.Derecho = Insertar(nodo.Derecho, clave, valor);
        else
            nodo.Valores.Add(valor); // misma clave → agrega a lista

        return Balancear(nodo);
    }

    // ── Recorridos ──────────────────────────────────────────────────────

    /// <summary>Inorden ascendente — devuelve elementos de menor a mayor clave.</summary>
    public IEnumerable<TValor> Inorden()
    {
        var lista = new List<TValor>();
        Inorden(_raiz, lista);
        return lista;
    }

    private static void Inorden(Nodo? nodo, List<TValor> lista)
    {
        if (nodo == null) return;
        Inorden(nodo.Izquierdo, lista);
        lista.AddRange(nodo.Valores);
        Inorden(nodo.Derecho, lista);
    }

    /// <summary>Inorden descendente — mayor a menor.</summary>
    public IEnumerable<TValor> InordenDescendente()
    {
        var lista = new List<TValor>();
        InordenDesc(_raiz, lista);
        return lista;
    }

    private static void InordenDesc(Nodo? nodo, List<TValor> lista)
    {
        if (nodo == null) return;
        InordenDesc(nodo.Derecho, lista);
        lista.AddRange(nodo.Valores);
        InordenDesc(nodo.Izquierdo, lista);
    }

    /// <summary>Devuelve todos los elementos cuya clave esté en [min, max]. O(log n + k).</summary>
    public IEnumerable<TValor> BuscarRango(double min, double max)
    {
        var lista = new List<TValor>();
        BuscarRango(_raiz, min, max, lista);
        return lista;
    }

    private static void BuscarRango(Nodo? nodo, double min, double max, List<TValor> lista)
    {
        if (nodo == null) return;
        if (nodo.Clave > min) BuscarRango(nodo.Izquierdo, min, max, lista);
        if (nodo.Clave >= min && nodo.Clave <= max) lista.AddRange(nodo.Valores);
        if (nodo.Clave < max) BuscarRango(nodo.Derecho, min, max, lista);
    }

    public void Limpiar() { _raiz = null; Tamanio = 0; }
}
