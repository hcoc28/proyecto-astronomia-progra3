namespace AstronomiaApp.EstructurasDatos;

/// <summary>
/// Tabla Hash con encadenamiento (chaining) para manejo de colisiones.
/// Implementación manual. Clave siempre string, valor genérico.
/// </summary>
public class TablaHash<TValor>
{
    private const int CAPACIDAD_INICIAL = 64;
    private const double FACTOR_CARGA_MAX = 0.75;

    private class Entrada
    {
        public string Clave;
        public TValor Valor;
        public Entrada? Siguiente;
        public Entrada(string clave, TValor valor) { Clave = clave; Valor = valor; }
    }

    private Entrada?[] _buckets;
    private int _capacidad;
    private int _tamanio;

    public int Tamanio => _tamanio;

    public TablaHash(int capacidadInicial = CAPACIDAD_INICIAL)
    {
        _capacidad = capacidadInicial;
        _buckets = new Entrada?[_capacidad];
    }

    /// <summary>Función hash — suma ponderada de caracteres mod capacidad.</summary>
    private int Hash(string clave)
    {
        unchecked
        {
            int hash = 0;
            int primo = 31;
            foreach (char c in clave.ToLowerInvariant())
            {
                hash = hash * primo + c;
            }
            return Math.Abs(hash) % _capacidad;
        }
    }

    /// <summary>Inserta o actualiza clave-valor. O(1) amortizado.</summary>
    public void Insertar(string clave, TValor valor)
    {
        if ((double)_tamanio / _capacidad >= FACTOR_CARGA_MAX)
            Redimensionar();

        int idx = Hash(clave);
        var actual = _buckets[idx];
        while (actual != null)
        {
            if (actual.Clave.Equals(clave, StringComparison.OrdinalIgnoreCase))
            {
                actual.Valor = valor; // actualizar
                return;
            }
            actual = actual.Siguiente;
        }
        // nueva entrada al frente de la cadena
        var nueva = new Entrada(clave, valor) { Siguiente = _buckets[idx] };
        _buckets[idx] = nueva;
        _tamanio++;
    }

    /// <summary>Busca por clave exacta. O(1) promedio.</summary>
    public bool Buscar(string clave, out TValor? valor)
    {
        int idx = Hash(clave);
        var actual = _buckets[idx];
        while (actual != null)
        {
            if (actual.Clave.Equals(clave, StringComparison.OrdinalIgnoreCase))
            {
                valor = actual.Valor;
                return true;
            }
            actual = actual.Siguiente;
        }
        valor = default;
        return false;
    }

    /// <summary>Elimina una clave. O(1) promedio.</summary>
    public bool Eliminar(string clave)
    {
        int idx = Hash(clave);
        var actual = _buckets[idx];
        Entrada? anterior = null;
        while (actual != null)
        {
            if (actual.Clave.Equals(clave, StringComparison.OrdinalIgnoreCase))
            {
                if (anterior == null) _buckets[idx] = actual.Siguiente;
                else anterior.Siguiente = actual.Siguiente;
                _tamanio--;
                return true;
            }
            anterior = actual;
            actual = actual.Siguiente;
        }
        return false;
    }

    /// <summary>Devuelve todos los pares clave-valor.</summary>
    public IEnumerable<(string Clave, TValor Valor)> ObtenerTodos()
    {
        foreach (var bucket in _buckets)
        {
            var actual = bucket;
            while (actual != null)
            {
                yield return (actual.Clave, actual.Valor);
                actual = actual.Siguiente;
            }
        }
    }

    private void Redimensionar()
    {
        var viejos = _buckets;
        _capacidad *= 2;
        _buckets = new Entrada?[_capacidad];
        _tamanio = 0;
        foreach (var bucket in viejos)
        {
            var actual = bucket;
            while (actual != null)
            {
                Insertar(actual.Clave, actual.Valor);
                actual = actual.Siguiente;
            }
        }
    }
}
