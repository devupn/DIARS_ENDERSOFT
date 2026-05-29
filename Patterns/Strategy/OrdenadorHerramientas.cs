using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public class OrdenadorHerramientas
    {
        private IOrdenamientoStrategy _estrategia;

        public OrdenadorHerramientas(IOrdenamientoStrategy estrategia)
        {
            _estrategia = estrategia;
        }

        public void CambiarEstrategia(IOrdenamientoStrategy nuevaEstrategia)
        {
            _estrategia = nuevaEstrategia;
        }

        public IEnumerable<Herramienta> Ejecutar(IEnumerable<Herramienta> herramientas)
        {
            return _estrategia.Ordenar(herramientas);
        }

        public string EstrategiaActual => _estrategia.Nombre;

        public static Dictionary<string, IOrdenamientoStrategy> ObtenerEstrategias()
        {
            return new Dictionary<string, IOrdenamientoStrategy>
            {
                { "nombre_asc", new OrdenarPorNombreAsc() },
                { "nombre_desc", new OrdenarPorNombreDesc() },
                { "cantidad_asc", new OrdenarPorCantidadAsc() },
                { "cantidad_desc", new OrdenarPorCantidadDesc() }
            };
        }
    }
}
