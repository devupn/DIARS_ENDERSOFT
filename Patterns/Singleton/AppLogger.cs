namespace EMDERSOFT.Patterns.Singleton
{
    public class AppLogger
    {
        private static volatile AppLogger? _instancia;
        private static readonly object _lock = new object();

        private AppLogger()
        {
            var ruta = ObtenerRutaLog();
            if (!File.Exists(ruta))
                File.WriteAllText(ruta, $"=== Log iniciado: {DateTime.Now} ==={Environment.NewLine}");
        }

        public static AppLogger ObtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                        _instancia = new AppLogger();
                }
            }
            return _instancia;
        }

        public void Info(string mensaje) => Escribir("INFO", mensaje);
        public void Error(string mensaje) => Escribir("ERROR", mensaje);
        public void Advertencia(string mensaje) => Escribir("ADVERTENCIA", mensaje);

        private void Escribir(string nivel, string mensaje)
        {
            var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nivel}] {mensaje}";
            lock (_lock)
            {
                File.AppendAllText(ObtenerRutaLog(), linea + Environment.NewLine);
            }
            Console.WriteLine(linea);
        }

        public List<string> ObtenerUltimosRegistros(int cantidad = 20)
        {
            var ruta = ObtenerRutaLog();
            if (!File.Exists(ruta)) return new List<string>();
            return File.ReadAllLines(ruta)
                .TakeLast(cantidad)
                .Reverse()
                .ToList();
        }

        private string ObtenerRutaLog() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
    }
}
