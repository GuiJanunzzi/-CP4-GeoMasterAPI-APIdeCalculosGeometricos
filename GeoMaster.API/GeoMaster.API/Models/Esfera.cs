using GeoMaster.API.Interfaces;

namespace GeoMaster.API.Models
{
    public class Esfera : ICalculos3D
    {
        public double Raio { get; set; }

        public double CalcularAreaSuperficial()
        {
            // Fórmula da área superficial: 4 * π * r²
            return 4 * Math.PI * Math.Pow(Raio, 2);
        }

        public double CalcularVolume()
        {
            // Fórmula do volume: (4/3) * π * r³
            return (4.0 / 3.0) * Math.PI * Math.Pow(Raio, 3);
        }
    }
}
