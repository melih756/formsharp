using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FormSharp
{
    public partial class Form1 : Form
    {
        public string originalPdf = "C:\\Users\\ACER\\Desktop\\örnek.pdf";
        public string newPdf = "C:\\Users\\ACER\\Desktop\\yeni_belge.pdf";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatePdf(originalPdf, newPdf);

            Process.Start(newPdf);
        }

        public static void CreatePdf(string sourcePdf, string newPdf)
        {
            using (var doc = PdfReader.Open(sourcePdf, PdfDocumentOpenMode.Modify)) //pdf belgesinde değişikliğe izin verir
            {
                var page = doc.Pages[0];
                var contents = ContentReader.ReadContent(page);

                ReplaceText(contents, "Hello", "Hola");

                var gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Verdana", 15, XFontStyle.Regular); 

                
                XTextFormatter tf = new XTextFormatter(gfx); //metin için konum belirlenir ve o konuma istenilen metin yazdırılır
                XRect rect = new XRect(40, 40, 300, 200); 
                tf.DrawString("merhaba", font, XBrushes.Black, rect, XStringFormats.TopLeft);

                doc.Save(newPdf);
            }
        }

        public static void ReplaceText(CSequence contents, string searchText, string replaceText)
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (contents[i] is COperator)
                {
                    var cOp = contents[i] as COperator;
                    for (int j = 0; j < cOp.Operands.Count; j++)
                    {
                        if (cOp.OpCode.Name == OpCodeName.Tj.ToString() ||
                            cOp.OpCode.Name == OpCodeName.TJ.ToString())
                        {
                            if (cOp.Operands[j] is CString)
                            {
                                var cString = cOp.Operands[j] as CString;
                                if (cString.Value.Contains(searchText))
                                {
                                    cString.Value = cString.Value.Replace(searchText, replaceText);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
