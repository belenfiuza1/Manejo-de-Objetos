using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Club
    {
        List<Socio> ListaSocios;
        List<Descuento> ListaDescuento;
        public Club()
        {
            ListaSocios = new List<Socio>();
            ListaDescuento = new List<Descuento>();
        }
        public void AgregarSocio(Socio pSocio)
        {
            try
            {
                ListaSocios.Add(pSocio); 
            }
            catch (Exception ex) { throw ex; }
                
        }
        public void AgregarDescuento(Descuento pDto)
        {
            try
            {
                ListaDescuento.Add(pDto);
            }
            catch (Exception ex) { throw ex; }

        }
        public void BorrarSocio(Socio pSocio)
        {

            try
            {
                ListaSocios.Remove(pSocio);
            }
            catch (Exception ex) { throw ex; }

        }
        public void BorrarDescuento(Descuento pDto)
        {

            try
            {
                ListaDescuento.Remove(pDto);
            }
            catch (Exception ex) { throw ex; }

        }
        public void ModificarSocio(Socio pSocio)
        {

            try
            {
                Socio s= ListaSocios.Find(x => x.Legajo == pSocio.Legajo);
                if(s!=null)
                {
                    s.Nombre = pSocio.Nombre;
                    s.Apellido = pSocio.Apellido;
                    s.FechaIngreso = pSocio.FechaIngreso;
                }
            }
            catch (Exception ex) { throw ex; }

        }
        public void ModificarDescuento(Descuento pDto)
        {

            try
            {
                Descuento d = ListaDescuento.Find(x => x.Codigo == pDto.Codigo);
                if (d != null)
                {
                    
                    d.Descripcion = pDto.Descripcion;
                    if (pDto is DescuentoImporteAlto)
                    { (d as DescuentoImporteAlto).Importe = (pDto as DescuentoImporteAlto).Importe; }
                    else { (d as DescuentoSocioEspecial).Porcentaje = (pDto as DescuentoSocioEspecial).Porcentaje; }
                 
                }
            }
            catch (Exception ex) { throw ex; }

        }
        public List<Socio> RetornaSocios() { return ListaSocios; } 
        public List<Descuento> RetornaDescuentos(){ return ListaDescuento; }
        
    }
}
