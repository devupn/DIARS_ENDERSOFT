using EMDERSOFT.Models;

namespace EMDERSOFT.Patterns.Strategy
{
    public class OrdenarPorNombreAsc : IOrdenamientoStrategy
    {
        public string Nombre => "Nombre (A-Z)";
        public IEnumerable<Herramienta> Ordenar(IEnumerable<Herramienta> herramientas)
            => herramientas.OrderBy(h => h.Nombre);
    }
}
