using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public class OrdenarPorCantidadDesc : IOrdenamientoStrategy
    {
        public string Nombre => "Cantidad (mayor a menor)";
        public IEnumerable<Herramienta> Ordenar(IEnumerable<Herramienta> herramientas)
            => herramientas.OrderByDescending(h => h.Cantidad);
    }
}
