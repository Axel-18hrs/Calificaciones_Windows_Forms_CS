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
            ofd = new OpenFileDialog();
            ofd.Title = "Calificaciones del alumno";
            ofd.Filter = "Archivos binarios (*.bin)|*.bin";

            DialogResult opc = MessageBox.Show("Quieres usar un archivo existente?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (opc == DialogResult.Yes)
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    fileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    reader = new BinaryReader(fileStream);

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
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

                }
                else
                {
                    MessageBox.Show("Error al guardar el archivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            DialogResult x = MessageBox.Show("Deseas guardar las calificaciones?", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (x == DialogResult.Yes)
            {
                SaveFileDialog ruta = new SaveFileDialog();
                ruta.Filter = "Archivos binarios (*.bin)|*.bin";
                ruta.Title = "Calificaciones del alumno";

                if (ruta.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("Error al escoger la ruta de archivo. ", "Error", MessageBoxButtons.OK);
                    return;
                }
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

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            dgvCalificaciones.Rows.Add(nud1.Value.ToString(), nud2.Value.ToString(), nud3.Value.ToString(), nud4.Value.ToString(), nud5.Value.ToString(), nud6.Value.ToString());
            btnEnviar.Enabled = false;
        }
    }
}
