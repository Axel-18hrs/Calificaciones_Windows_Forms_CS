using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calificaciones_Windows_Forms_CS
{
    public partial class Form1 : Form
    {
        // hago uso de objetos o clases de tipos que me ayudaran a manejar archivos .bin y escoger rutas para guardar y leer
        // y los globalizo
        private BinaryReader reader;
        private BinaryWriter writer;
        private StreamReader readerStream;
        private FileStream fileStream;
        private OpenFileDialog ofd;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // al cargar el programa damos la opcion de leer un archivo creado anteriormente
            ofd = new OpenFileDialog();
            ofd.Title = "Calificaciones del alumno";
            ofd.Filter = "Archivos binarios (*.bin)|*.bin";
            // doy la opcion al usuario de cargar otro documento
            DialogResult opc = MessageBox.Show("Quieres usar un archivo existente?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (opc == DialogResult.Yes)
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // por lo que vi al investigar sobre el manejo de archivos binarios cuido
                    // que tanto la lectura como escritura del archivo sean en el mismo formato o  tipo de daaato para evitar errores
                    fileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    reader = new BinaryReader(fileStream);

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        // tanto la manera en como se escribe como la de cuando se lee tiene tanto que ver en los archivos bin
                        // que el mas minimo error podria arruinar una estructura de datos pequeña como la de este proyecto
                        double value1 = reader.ReadDouble();
                        double value2 = reader.ReadDouble();
                        double value3 = reader.ReadDouble();
                        double value4 = reader.ReadDouble();
                        double value5 = reader.ReadDouble();
                        double value6 = reader.ReadDouble();
                        dgvCalificaciones.Rows.Add(value1, value2, value3, value4, value5, value6);
                        btnEnviar.Enabled = false;
                        MessageBox.Show("No se podra calificar nuevamente si ya se establecio un archivo para este programa", "???", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    reader.Close();
                    fileStream.Close();

                }
                else
                {
                    MessageBox.Show("Error al guardar el archivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // haciendo uso de eventos indico qe al momento de que se este cerrando el programa el usuario
            // tenga la oportunidad de guardar archivos
            DialogResult x = MessageBox.Show("Deseas guardar las calificaciones?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (x == DialogResult.Yes)
            {
                SaveFileDialog ruta = new SaveFileDialog();
                ruta.Filter = "Archivos binarios (*.bin)|*.bin";
                ruta.Title = "Calificaciones del alumno";

                // detecto el error y lo evito antes de que suceda
                if (ruta.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("Error al escoger la ruta de archivo. ", "Error", MessageBoxButtons.OK);
                    return;
                }

                // con el filestream puedo escoger que clase de cosa quiero hacer en caunto de archivos se trata
                // en este caso lo unico que quiero hacer es crear un archivo en base a la ruta que me dieron anteriormente
                // y escrbir sobre el en este punto del programa
                FileStream rutad = new FileStream(ruta.FileName, FileMode.Create, FileAccess.Write);
                writer = new BinaryWriter(rutad);
                foreach (DataGridViewRow row in dgvCalificaciones.Rows)
                {
                    // Itera sobre cada celda en la fila
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Escribe el valor de la celda en el archivo binario
                        writer.Write(Convert.ToDouble(cell.Value));
                    }
                }

                writer.Close();
                rutad.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // esto lo añadi por error y no pude eliminarlo porque al borrarlo me surgia un error en la ventana de diseño
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // en este apartado simplemente registro los datos entregados por el usuario en el dgv
            dgvCalificaciones.Rows.Add(nud1.Value.ToString(), nud2.Value.ToString(), nud3.Value.ToString(), nud4.Value.ToString(), nud5.Value.ToString(), nud6.Value.ToString());
            btnEnviar.Enabled = false;
        }
    }
}
