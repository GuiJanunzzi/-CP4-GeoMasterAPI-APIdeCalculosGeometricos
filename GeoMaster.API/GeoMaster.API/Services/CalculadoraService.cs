using GeoMaster.API.Interfaces;

namespace GeoMaster.API.Services
{
    public class CalculadoraService : ICalculadoraService
    {
        public double CalcularArea(ICalculos2D forma)
        {
            return forma.CalcularArea();
        }

        public double CalcularPerimetro(ICalculos2D forma)
        {
            return forma.CalcularPerimetro();
        }

        public double CalcularVolume(ICalculos3D forma)
        {
            return forma.CalcularVolume();
        }

        public double CalcularAreaSuperficial(ICalculos3D forma)
        {
            return forma.CalcularAreaSuperficial();
        }
    }
}
