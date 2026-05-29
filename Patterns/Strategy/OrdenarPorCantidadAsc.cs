using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public class OrdenarPorCantidadAsc : IOrdenamientoStrategy
    {
        public string Nombre => "Cantidad (menor a mayor)";
        public IEnumerable<Herramienta> Ordenar(IEnumerable<Herramienta> herramientas)
            => herramientas.OrderBy(h => h.Cantidad);
    }
}
