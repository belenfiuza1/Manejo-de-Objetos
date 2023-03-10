using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
       
        Club club;
        
        
        public Form1()
        {
            InitializeComponent();
            club = new Club();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.MultiSelect = true;
            dataGridView2.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Mostrar(DataGridView pDGV, object pO) 
        { 
            pDGV.DataSource = null;pDGV.DataSource = pO; 
            if(pDGV.Name=="dataGridView2")
            {
                pDGV.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                pDGV.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string legajo = Interaction.InputBox("Legajo: ");
                var re = new Regex(@"[A-Z]\d{3}[a-z]");
                if (!re.IsMatch(legajo)) throw new Exception("El legajo no posee el formato correcto");
                if (club.RetornaSocios().Exists(x => x.Legajo==legajo)) throw new Exception("El legajo ya existe");
                string nombre = Interaction.InputBox("Nombre: ");
                string apellido = Interaction.InputBox("Apellido: ");
                string fechaIngreso = Interaction.InputBox("Fecha de Ingreso: ");
                if (!Information.IsDate(fechaIngreso)) throw new Exception("Fecha de ingreso invalida");
                Socio s = null;
                if (radioButton1.Checked) { s = new SocioPrincipal(); } else { s = new SocioFamiliar(); }
                s.Legajo = legajo;s.Nombre = nombre;s.Apellido = apellido;s. FechaIngreso = DateTime.Parse(fechaIngreso);
                club.AgregarSocio(s);
                Mostrar(dataGridView1, club.RetornaSocios());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                if(dataGridView1.Rows.Count==0) throw new Exception("No hay socios para modificar");
                Socio s = dataGridView1.SelectedRows[0].DataBoundItem as Socio;
                s.Nombre=Interaction.InputBox("Nombre: ","Modificando el nombre...",s.Nombre);
                s.Apellido = Interaction.InputBox("Apellido: ", "Modificando el apellido...", s.Apellido);
                string fechaIngreso = Interaction.InputBox("Fecha de Ingreso: ", "Modificando la fecha de ingreso...", s.FechaIngreso.ToShortDateString());
                if (!Information.IsDate(fechaIngreso)) throw new Exception("Fecha de ingreso invalida");
                s.FechaIngreso = DateTime.Parse(fechaIngreso);
                club.ModificarSocio(s);
                Mostrar(dataGridView1, club.RetornaSocios());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.Rows.Count == 0) throw new Exception("No hay socios para modificar");
                club.BorrarSocio(dataGridView1.SelectedRows[0].DataBoundItem as Socio);
                Mostrar(dataGridView1, club.RetornaSocios());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = Interaction.InputBox("Codigo: ");
                if (!Information.IsNumeric(codigo)) throw new Exception("Codigo Incorrecto");
                if (club.RetornaDescuentos().Exists(x => x.Codigo == int.Parse(codigo))) throw new Exception("El codigo ya existe");
                string descripcion = Interaction.InputBox("Descripcion: ");
                Descuento d = null;
                if (radioButton4.Checked) 
                { 
                    d = new DescuentoImporteAlto(); 
                } 
                else 
                { 
                    d = new DescuentoSocioEspecial(); 
                }
                d.Codigo = int.Parse(codigo);d.Descripcion = descripcion;
                if (radioButton4.Checked) 
                {
                    string importe = Interaction.InputBox("Importe: ");
                    if (!Information.IsNumeric(importe)) throw new Exception("El importe es incorrecto");
                    (d as DescuentoImporteAlto).Importe = decimal.Parse(importe);
                }
                else 
                {
                    string porcentaje = Interaction.InputBox("Porcentaje entre 0 y 100: ");
                    if (!Information.IsNumeric(porcentaje)) throw new Exception("El porcentaje no es numerico");
                    if(decimal.Parse(porcentaje)<0 || decimal.Parse(porcentaje)>100) throw new Exception("El porcentaje debe ser mayor a 0 y menor o igual a 100");
                    (d as DescuentoSocioEspecial).Porcentaje = decimal.Parse(porcentaje);
                }
                
                club.AgregarDescuento(d);
                var ie = from dd in club.RetornaDescuentos() select new { Codigo = dd.Codigo, 
                                                                          Descripción = dd.Descripcion, 
                                                                          Importe = dd is DescuentoImporteAlto ? (dd as DescuentoImporteAlto).Importe.ToString() : "--", 
                                                                          Porcentaje = dd is DescuentoSocioEspecial ? (dd as DescuentoSocioEspecial).Porcentaje.ToString() : "--" };
                Mostrar(dataGridView2, ie.ToList());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView2.Rows.Count == 0) throw new Exception("No hay descuentos para modificar");
                Descuento d = club.RetornaDescuentos().Find(x=>x.Codigo==int.Parse(dataGridView2.SelectedRows[0].Cells[0].Value.ToString()));
                if(d is null) throw new Exception("Error al modificar");
                d.Descripcion = Interaction.InputBox("Descripcion: ", "Modificando la descripcion...", d.Descripcion);
                
                if (d is DescuentoImporteAlto)
                {
                    string importe = Interaction.InputBox("Importe: ","Modificando el importe...",(d as DescuentoImporteAlto).Importe.ToString());
                    if (!Information.IsNumeric(importe)) throw new Exception("El importe es incorrecto");
                    (d as DescuentoImporteAlto).Importe = decimal.Parse(importe);
                }
                else 
                {
                    string porcentaje = Interaction.InputBox("Porcentaje entre 0 y 100: ","Modificando el porcentaje...",(d as DescuentoSocioEspecial).Porcentaje.ToString());
                    if (!Information.IsNumeric(porcentaje)) throw new Exception("El porcentaje no es numerico");
                    if (decimal.Parse(porcentaje) < 0 || decimal.Parse(porcentaje) > 100) throw new Exception("El porcentaje debe ser mayor a 0 y menor o igual a 100");
                    (d as DescuentoSocioEspecial).Porcentaje = decimal.Parse(porcentaje);
                }
                
                var ie = from dd in club.RetornaDescuentos()
                         select new
                         {
                             Codigo = dd.Codigo,
                             Descripción = dd.Descripcion,
                             Importe = dd is DescuentoImporteAlto ? (dd as DescuentoImporteAlto).Importe.ToString() : "--",
                             Porcentaje = dd is DescuentoSocioEspecial ? (dd as DescuentoSocioEspecial).Porcentaje.ToString() : "--"
                         };
                Mostrar(dataGridView2, RetornaVistaDescuentos());

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        public object RetornaVistaDescuentos()
        {
            return (from dd in club.RetornaDescuentos()
                    select new
                    {
                        Codigo = dd.Codigo,
                        Descripción = dd.Descripcion,
                        Importe = dd is DescuentoImporteAlto ? (dd as DescuentoImporteAlto).Importe.ToString() : "--",
                        Porcentaje = dd is DescuentoSocioEspecial ? (dd as DescuentoSocioEspecial).Porcentaje.ToString() : "--"
                    }).ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView2.Rows.Count == 0) throw new Exception("No hay descuentos para borrar");
                Descuento d = club.RetornaDescuentos().Find(x => x.Codigo == int.Parse(dataGridView2.SelectedRows[0].Cells[0].Value.ToString()));
                if (d is null) throw new Exception("Error al modificar");
                club.BorrarDescuento(d);
                var ie = from dd in club.RetornaDescuentos()
                         select new
                         {
                             Codigo = dd.Codigo,
                             Descripción = dd.Descripcion,
                             Importe = dd is DescuentoImporteAlto ? (dd as DescuentoImporteAlto).Importe.ToString() : "--",
                             Porcentaje = dd is DescuentoSocioEspecial ? (dd as DescuentoSocioEspecial).Porcentaje.ToString() : "--"
                         };
                Mostrar(dataGridView2, ie.ToList());

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
