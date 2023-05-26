using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace PIAA
{
    public partial class Form4 : Form
    {
        private List<Venta> ventas;
        public Form4()
        {
            InitializeComponent();
            ventas = new List<Venta>();
        }

       
        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cod;
            string nom;
            float precio;

            cod = cmbProductos.SelectedIndex;
            nom = cmbProductos.SelectedItem.ToString();
            precio = cmbProductos.SelectedIndex;

            switch (cod)
            {
                case 0: lbCodigo.Text = "001"; break;
                case 1: lbCodigo.Text = "002"; break;
                case 2: lbCodigo.Text = "003"; break;
                case 3: lbCodigo.Text = "004"; break;
                case 4: lbCodigo.Text = "005"; break;
                case 5: lbCodigo.Text = "006"; break;
                case 6: lbCodigo.Text = "007"; break;
                case 7: lbCodigo.Text = "008"; break;
                case 8: lbCodigo.Text = "009"; break;
                case 9: lbCodigo.Text = "010"; break;
                case 10: lbCodigo.Text = "011"; break;
                case 11: lbCodigo.Text = "012"; break;
            }

            switch (nom)
            {
                case "Orden de tacos de arrachera": lbNombre.Text = "Orden de tacos de arrachera"; break;
                case "Orden de tacos al pastor": lbNombre.Text = "Orden de tacos al pastor"; break;
                case "Orden de tacos arabes": lbNombre.Text = "Orden de tacos arabes"; break;
                case "Hamburguesa sencilla": lbNombre.Text = "Hamburguesa sencilla"; break;
                case "Hamburguesa doble": lbNombre.Text = "Hamburguesa doble"; break;
                case "Hamburguesa triple": lbNombre.Text = "Hamburguesa triple"; break;
                case "Burrito de arrachera": lbNombre.Text = "Burrito de arrachera"; break;
                case "Burrito de pastor": lbNombre.Text = "Burrito de pastor"; break;
                case "Burrito mixto": lbNombre.Text = "Burrito mixto"; break;
                case "Refresco": lbNombre.Text = "Refresco"; break;
                case "Limonada": lbNombre.Text = "Limonada"; break;
                case "Agua": lbNombre.Text = "Agua"; break;


            }

            switch (precio)
            {
                case 0: lbPrecio.Text = "80"; break;
                case 1: lbPrecio.Text = "80"; break;
                case 2: lbPrecio.Text = "80"; break;
                case 3: lbPrecio.Text = "75"; break;
                case 4: lbPrecio.Text = "90"; break;
                case 5: lbPrecio.Text = "100"; break;
                case 6: lbPrecio.Text = "90"; break;
                case 7: lbPrecio.Text = "90"; break;
                case 8: lbPrecio.Text = "90"; break;
                case 9: lbPrecio.Text = "15"; break;
                case 10: lbPrecio.Text = "20"; break;
                case 11: lbPrecio.Text = "15"; break;


            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Obtener los datos de la venta
            string codigo = lbCodigo.Text;
            string nombre = lbNombre.Text;
            float precio = float.Parse(lbPrecio.Text);
            int cantidad = 0;

            if (!int.TryParse(txtCantidad.Text, out cantidad))
            {
                MessageBox.Show("Por favor, ingresa una cantidad válida.");
                return;
            }

            // Calcular el total de la venta
            float total = precio * cantidad;

            // Crear una nueva venta
            Venta venta = new Venta
            {
                Codigo = codigo,
                Nombre = nombre,
                Precio = (decimal)precio,
                Cantidad = cantidad,
                ImporteTotal = (decimal)total
            };

            // Agregar la venta a la lista
            ventas.Add(venta);

            // Actualizar la vista de las ventas en el DataGridView
            dgvLista.Rows.Add(codigo, nombre, precio, cantidad, total);

            // Limpiar los campos
            lbCodigo.Text = lbNombre.Text = txtCantidad.Text = "";

            // Calcular el total a pagar
            obtenertotal();
        }


        public void obtenertotal()
        {
            float costot = 0;
            int contador = 0;

            contador = dgvLista.RowCount;

            for (int i = 0; i < contador; i++)
            {
                costot += float.Parse(dgvLista.Rows[i].Cells[4].Value.ToString());
            }

            lbTotalPagar.Text = costot.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult rppta = MessageBox.Show("Desea eliminar el producto?",
                    "Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rppta == DialogResult.Yes)
                {
                    dgvLista.Rows.Remove(dgvLista.CurrentRow);
                }
            }

            catch { }
            obtenertotal();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Verificar si el texto no es un número válido
            if (!float.TryParse(txtEfectivo.Text, out float efectivo))
            {
                txtEfectivo.Text = ""; // Borrar el texto ingresado
                return;
            }

            // Realizar el cálculo
            lbDevolucion.Text = (efectivo - float.Parse(lbTotalPagar.Text)).ToString();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out _))
            {
                // Clear the TextBox if the input is not a valid integer
                txtCantidad.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbDevolucion_Click(object sender, EventArgs e)
        {

        }

        private void registro_Click(object sender, EventArgs e)
        {
            // Verificar si hay datos para registrar
            if (ventas.Count == 0)
            {
                MessageBox.Show("No hay datos para registrar.", "Registro Vacío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save the ventas list to a JSON file
            string filePath = "ventas.json";
            string json = JsonConvert.SerializeObject(ventas, Formatting.Indented);

            // Check if the JSON file exists
            if (File.Exists(filePath))
            {
                // Append the JSON data to the existing file
                File.AppendAllText(filePath, json);
            }
            else
            {
                // Create a new JSON file and write the data
                File.WriteAllText(filePath, json);
            }

            MessageBox.Show("Los datos se han registrado en el archivo JSON.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public class Venta
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
            public decimal ImporteTotal { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Borrar los datos del panel2
            panel2.Controls.Clear();

            // Borrar los datos del panel3
            panel3.Controls.Clear();

            // Borrar el texto de txtEfectivo
            txtEfectivo.Text = string.Empty;
        }
    }
}
