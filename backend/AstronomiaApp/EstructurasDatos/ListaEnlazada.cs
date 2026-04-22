namespace AstronomiaApp.EstructurasDatos;

/// <summary>Lista doblemente enlazada genérica. Implementación manual sin librerías.</summary>
public class ListaEnlazada<T>
{
    private class Nodo
    {
        public T Valor;
        public Nodo? Siguiente;
        public Nodo? Anterior;
        public Nodo(T valor) { Valor = valor; }
    }

    private Nodo? _cabeza;
    private Nodo? _cola;
    private int _tamanio;

    public int Tamanio => _tamanio;
    public bool EstaVacia => _tamanio == 0;

    /// <summary>Agrega elemento al final. O(1).</summary>
    public void AgregarAlFinal(T valor)
    {
        var nuevo = new Nodo(valor);
        if (_cola == null)
        {
            _cabeza = _cola = nuevo;
        }
        else
        {
            nuevo.Anterior = _cola;
            _cola.Siguiente = nuevo;
            _cola = nuevo;
        }
        _tamanio++;
    }

    /// <summary>Agrega elemento al inicio. O(1).</summary>
    public void AgregarAlInicio(T valor)
    {
        var nuevo = new Nodo(valor);
        if (_cabeza == null)
        {
            _cabeza = _cola = nuevo;
        }
        else
        {
            nuevo.Siguiente = _cabeza;
            _cabeza.Anterior = nuevo;
            _cabeza = nuevo;
        }
        _tamanio++;
    }

    /// <summary>Elimina primera ocurrencia del valor. O(n).</summary>
    public bool Eliminar(T valor)
    {
        var actual = _cabeza;
        while (actual != null)
        {
            if (EqualityComparer<T>.Default.Equals(actual.Valor, valor))
            {
                if (actual.Anterior != null) actual.Anterior.Siguiente = actual.Siguiente;
                else _cabeza = actual.Siguiente;

                if (actual.Siguiente != null) actual.Siguiente.Anterior = actual.Anterior;
                else _cola = actual.Anterior;

                _tamanio--;
                return true;
            }
            actual = actual.Siguiente;
        }
        return false;
    }

    /// <summary>Busca elemento con predicado. O(n).</summary>
    public T? Buscar(Func<T, bool> predicado)
    {
        var actual = _cabeza;
        while (actual != null)
        {
            if (predicado(actual.Valor)) return actual.Valor;
            actual = actual.Siguiente;
        }
        return default;
    }

    /// <summary>Devuelve todos los elementos. O(n).</summary>
    public IEnumerable<T> ObtenerTodos()
    {
        var actual = _cabeza;
        while (actual != null)
        {
            yield return actual.Valor;
            actual = actual.Siguiente;
        }
    }

    /// <summary>Filtra elementos con predicado. O(n).</summary>
    public IEnumerable<T> Filtrar(Func<T, bool> predicado)
    {
        var actual = _cabeza;
        while (actual != null)
        {
            if (predicado(actual.Valor)) yield return actual.Valor;
            actual = actual.Siguiente;
        }
    }

    public void Limpiar()
    {
        _cabeza = _cola = null;
        _tamanio = 0;
    }
}
