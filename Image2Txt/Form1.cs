using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MODI;

namespace Image2Txt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            texto.Text="";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Abrir Archivo";
            ofd.Filter = "Jpg files (*.jpg)|*.jpg|Gif files (*.gif)|*.gif|Bitmap files (*.Bmp)|*.bmp|PNG files (*.png)|*.png*";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                string nombreArchivo = ofd.FileName;
                Bitmap Picture = new Bitmap(nombreArchivo);

                imagen.Image = (System.Drawing.Image)Picture;

                imagen.SizeMode = PictureBoxSizeMode.StretchImage;

                String NombreImg = ofd.SafeFileName;

                Document Mivariable = new Document();
                Mivariable.Create(@nombreArchivo);
                Mivariable.OCR(MODI.MiLANGUAGES.miLANG_SPANISH, true, true);
                MODI.Image image2 = (MODI.Image)Mivariable.Images[0];
                Layout layout = image2.Layout;

                foreach (Word word in layout.Words)
                {
                    texto.Text = texto.Text + word.Text+" ";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string rutaCarpeta = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                rutaCarpeta = fbd.SelectedPath;
                IEnumerator archivos = Directory.GetFiles(rutaCarpeta).GetEnumerator();
                while (archivos.MoveNext())
                {
                    string extension = Path.GetExtension(Convert.ToString(archivos.Current));
                    string nombreArchivo = Convert.ToString(archivos.Current).Replace(extension, string.Empty);

                    if (extension == ".jpg" || extension == ".JPG" || extension == ".bmp" ||
                    extension == ".BMP" || extension == ".tif" || extension == ".TIF" ||
                    extension == ".gif" || extension == ".GIF" || extension == ".png" ||
                    extension == ".PNG" || extension == ".tiff" || extension == ".TIFF")
                    {
                        try
                        {
                            Document documento = new Document();
                            documento.Create(Convert.ToString(archivos.Current));
                            documento.OCR(MODI.MiLANGUAGES.miLANG_SPANISH, true, true);
                            MODI.Image imagen2 = (MODI.Image)documento.Images[0];
                            texto.Text = imagen2.Layout.Text;

                            string ruta = nombreArchivo + extension;
                            imagen.Image= new System.Drawing.Bitmap(ruta);
                            imagen.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        catch (Exception exc)
                        {

                            MessageBox.Show(exc.Message);
                        }
                    }
                }
            }
        }
    }
}
