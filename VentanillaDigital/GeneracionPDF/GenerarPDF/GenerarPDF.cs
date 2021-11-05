using System;
using System.IO;
using HiQPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Generacion_PDF_Notaria.GenerarPDF
{
    public class GenerarPDF
    {
        public static byte[] pdfBuffer { get; set; }
        public static byte[] ConverFromHTMLToPdf(string codeHTML, string setHeader, string password)
        {
            HiQPdf.HtmlToPdf htmlToPdfConverter = new HiQPdf.HtmlToPdf();
            try
            {

                // create the HTML to PDF converter

                htmlToPdfConverter.SerialNumber = "66OCuruP-jaeCiZmK-mZLa28Xb-y9rL2cvT-2dvL2NrF-2tnF0tLS-0g==";

                // set browser width
                htmlToPdfConverter.BrowserWidth = 1200;

                // set browser height if specified, otherwise use the default

                // set HTML Load timeout
                htmlToPdfConverter.HtmlLoadedTimeout = 120;


                // set the PDF standard used by the document
                htmlToPdfConverter.Document.PdfStandard = PdfStandard.Pdf;


                // set PDF page margins
                htmlToPdfConverter.Document.Margins = new PdfMargins();

                // set whether to embed the true type font in PDF
                htmlToPdfConverter.Document.FontEmbedding = true;
                htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;

                #region Colocar Encabezado Fijo
                SetHeader(htmlToPdfConverter.Document, setHeader);
                #endregion

                #region Colocar Pie de Pagina Fijo

                SetFooter(htmlToPdfConverter.Document);
                #endregion

                htmlToPdfConverter.PageCreatingEvent += new PdfPageCreatingDelegate(htmlToPdfConverter_PageCreatingEvent);
                #region Colocar contraseña en los pdf
                // set the document security
                /*htmlToPdfConverter.Document.Security.OpenPassword = password;
                htmlToPdfConverter.Document.Security.AllowPrinting = false;

                // set the permissions password too if an open password was set
                if (htmlToPdfConverter.Document.Security.OpenPassword != null && htmlToPdfConverter.Document.Security.OpenPassword != String.Empty)
                    htmlToPdfConverter.Document.Security.PermissionsPassword = ConfigurationManager.AppSettings["pdfPwd"];*/
                #endregion

                pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(codeHTML, null);

                #region remover paginas en blanco

                PdfReader originalPDFReader = new PdfReader(pdfBuffer);

                if (originalPDFReader.NumberOfPages > 1)
                {
                    // create the PDF text extractor
                    PdfTextExtract pdfTextExtract = new PdfTextExtract();
                    pdfTextExtract.TextExtractMode = HiQPdf.PdfTextExtractMode.KeepReadingOrder;

                    // create the PDF images extractor
                    PdfImagesExtract pdfImagesExtract = new PdfImagesExtract();

                    string lastText = pdfTextExtract.ExtractText(pdfBuffer, originalPDFReader.NumberOfPages, originalPDFReader.NumberOfPages).Trim();
                    ExtractedPdfImage[] lastExtractedImages = pdfImagesExtract.ExtractToImageObjects(pdfBuffer, originalPDFReader.NumberOfPages, originalPDFReader.NumberOfPages);

                    //Si la pagina no tiene texto ni imagenes se asume que esta vacia
                    if (lastText.Equals("===== HiQPdf Evaluation ======") && lastExtractedImages.Length == 0)
                    {
                        using (MemoryStream msCopy = new MemoryStream())
                        {
                            using (Document docCopy = new Document())
                            {
                                using (PdfCopy copy = new PdfCopy(docCopy, msCopy))
                                {
                                    docCopy.Open();
                                    for (int pageNum = 1; pageNum <= originalPDFReader.NumberOfPages; pageNum++)
                                    {
                                        // extract the text from a range of pages of the PDF document
                                        string text = pdfTextExtract.ExtractText(pdfBuffer, pageNum, pageNum).Trim();
                                        ExtractedPdfImage[] extractedImages = pdfImagesExtract.ExtractToImageObjects(pdfBuffer, pageNum, pageNum);

                                        //Si la pagina no tiene texto ni imagenes se asume que esta vacia
                                        if (text.Equals("===== HiQPdf Evaluation ======") && extractedImages.Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            copy.AddPage(copy.GetImportedPage(originalPDFReader, pageNum));
                                        }
                                    }
                                    docCopy.Close();
                                }
                            }

                            pdfBuffer = msCopy.ToArray();
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
            }
            finally
            {
                // dettach from PageCreatingEvent event
                htmlToPdfConverter.PageCreatingEvent -= new PdfPageCreatingDelegate(htmlToPdfConverter_PageCreatingEvent);

            }
            return pdfBuffer;

        }
        private static void SetHeader(PdfDocumentControl htmlToPdfDocument, string header)
        {

            // enable header display
            htmlToPdfDocument.Header.Enabled = true;

            if (!htmlToPdfDocument.Header.Enabled)
                return;

            // set header height
            htmlToPdfDocument.Header.Height = 100;


            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ? htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

            float headerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left - htmlToPdfDocument.Margins.Right;
            float headerHeight = htmlToPdfDocument.Header.Height;


            // layout HTML in header
            PdfHtml headerHtml = new PdfHtml(0, 10, headerWidth - 2, headerHeight - 2, header, null);

            headerHtml.FitDestHeight = true;
            //headerHtml.FontEmbedding = true;
            htmlToPdfDocument.Header.Layout(headerHtml);
        }

        private static void SetFooter(PdfDocumentControl htmlToPdfDocument)
        {
            // enable footer display
            htmlToPdfDocument.Footer.Enabled = true;

            if (!htmlToPdfDocument.Footer.Enabled)
                return;

            // set footer height
            htmlToPdfDocument.Footer.Height = 50;

            // set footer background color
            // htmlToPdfDocument.Footer.BackgroundColor = System.Drawing.Color.WhiteSmoke; #c7c7c7

            float pdfPageWidth = htmlToPdfDocument.PageOrientation == PdfPageOrientation.Portrait ?
                                        htmlToPdfDocument.PageSize.Width : htmlToPdfDocument.PageSize.Height;

            float footerWidth = pdfPageWidth - htmlToPdfDocument.Margins.Left - htmlToPdfDocument.Margins.Right;
            float footerHeight = htmlToPdfDocument.Footer.Height;

            // layout HTML in footer
            PdfHtml footerHtml = new PdfHtml(16, -4, footerWidth - 31, footerHeight - 0, @"<!DOCTYPE html>
<html>
<head>
<title>Page Title</title>
            <style type=""text/css"">
                                    .item {border-bottom-left-radius: 40px; border-bottom-right-radius: 40px; border: #c7c7c7 1px solid;}
            </style>
</head>
<body>

</body>
</html>", null);
            footerHtml.FitDestHeight = true;
            //footerHtml.FontEmbedding = true;
            htmlToPdfDocument.Footer.Layout(footerHtml);

            // add page numbering
            System.Drawing.Font pageNumberingFont = new System.Drawing.Font(new System.Drawing.FontFamily("Arial"),
                                        8, System.Drawing.GraphicsUnit.Point);
            PdfText pageNumberingText = new PdfText(10, footerHeight - 20, "Página {CrtPage} de {PageCount}", pageNumberingFont);
            pageNumberingText.HorizontalAlign = PdfTextHAlign.Center;
            pageNumberingText.EmbedSystemFont = true;
            htmlToPdfDocument.Footer.Layout(pageNumberingText);
        }

        static void htmlToPdfConverter_PageCreatingEvent(PdfPageCreatingParams eventParams)
        {
            HiQPdf.PdfPage pdfPage = eventParams.PdfPage;
            int pdfPageNumber = eventParams.PdfPageNumber;

            if (pdfPageNumber == 1)
            {
                // set the header and footer visibility in first page
                pdfPage.DisplayHeader = true;
                pdfPage.DisplayFooter = true;
            }
            else if (pdfPageNumber >= 2)
            {
                // set the header and footer visibility in second page
                pdfPage.DisplayHeader = true;
                pdfPage.DisplayFooter = true;
                pdfPage.CreateHeaderCanvas(50);


                string header = @"<!DOCTYPE html>
                                        <html lang=""es"">
                                        <head>
                                            <meta charset = ""UTF-8"">
 
                                             <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"" >
    
                                                <meta http-equiv = ""X-UA-Compatible"" content = ""ie=edge"" >
         

                                                     <title> Reporte de consumo </title>
         
                                                     <style type=""text/css"">
                                              @page {
                                                        size: A4 portrait;
                                                    }

                                                    * {
                                                        margin: 0px;
                                                        padding: 0px;
                                                        color: rgb(49, 49, 49);
                                                        font-family: sans-serif;
                                                    }

                                                    .wrapper-print {
                                                        margin: 1px 15px 0;
                                                        border-radius: 8px;
                                                        padding: 10px 15px;
                                                        height: 100%;
                                                    }

                                                    .flex-container {
                                                        padding: 1px 0 10px;
                                                        display: flex;
                                                        justify-content: space-between;
                                                        align-items: center;
                                                    }

                                                    .logo {
                                                        width: 200px;
                                                        text-align: right;
                                                    }

                                                    .logo p {
                                                        font-size: 12px;
                                                        margin-bottom: 5px;
                                                        font-weight: 200;
                                                        line-height: 1;
                                                    }

                                                    .text-fecha{
                                                        font-size: 18px;
                                                        font-weight: bold;
                                                        text-align: right;
                                                        line-height: 15px;
                                                    }
		                                            .text-doc{
                                                        font-size: 18px;
                                                        font-weight: bold;
                                                        text-align: right;
                                                        line-height: 15px;
                                                    }

                                                    .item {
                                                        width: 100%;
                                                        padding: 5px 8px;
                                                        line-height: 21px;
                                                        flex: 1px;
                                                        color: #a4a3a3;
                                                        min-height: 43px;
                                                    }

                                                    .tl-c {
                                                        border-radius: 4px 0 0 0;
                                                    }

                                                    .tr-c {
                                                        border-radius: 0 4px 0 0;
                                                    }

                                                    .table .numeros {
                                                        text-align: right;
                                                        font-variant-numeric: tabular-nums;
                                                    }

                                                    td {
                                                        font-size: 12px;
                                                        font-weight: 200;
                                                        padding: 4px 6px;
                                                        border-right: #dedede;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: hsl(38, 10%, 96%);
                                                        -webkit-print-color-adjust: exact;
                                                        color-adjust: exact;
                                                    }

                                                    strong {
                                                        font-weight: bold;
                                                    }

                                                    .table thead th {
                                                        font-size: 12px;
                                                        text-align: left;
                                                        padding: 5px;
                                                        -webkit-print-color-adjust: exact;
                                                        color: #272727;
                                                        vertical-align: top;
                                                        border: 1px solid #dedede;
                                                        white-space: nowrap;
                                                    }

                                                    .red {
                                                        color: red;
                                                    }

                                                    .foot {
                                                        font-size: 10px;
                                                        padding: 10px 0 10px;
                                                        color: #a4a3a3;
                                                    }

                                                    .table {
                                                        border: 1px solid #dedede;
                                                        border-collapse: collapse;
                                                        font-variant-numeric: tabular-nums;
                                                    }

                                                    .table tbody td{
                                                        border: 1px solid #dedede;
                                                    }

                                                    .table td:not(:nth-child(2)) {
                                                        width: 1px;
                                                        white-space: nowrap;
                                                    }

                                                    .titulo-tabla {
                                                        color: #c20600;
                                                    }

                                                    @media print {

                                                        .wrapper-print {
                                                            height: 2cm;
                                                        }

                                                        .logo-ucnc {
                                                            width: 2.5cm;
                                                        }

                                                        .logo-ucnc img {
                                                            width: 1.8cm;
                                                        }

                                                        .logo {
                                                            width: 3.5cm;
                                                        }

                                                        .flex-container h1 {
                                                            font-size: 18pt;
                                                        }

                                                    }
                                            </style>
                                        <body>
                                        
                                       </body>
                                </html>";
                float pdfPageWidth = pdfPage.Orientation == PdfPageOrientation.Portrait ? pdfPage.Size.Width : pdfPage.Size.Height;

                float headerWidth = pdfPageWidth - pdfPage.Margins.Left - pdfPage.Margins.Right;
                float headerHeight = pdfPage.Header.Height;


                // layout HTML in header
                PdfHtml htmlInHeader = new PdfHtml(0, 28, headerWidth - 0, headerHeight - 3, header, null);

                htmlInHeader.FitDestHeight = true;
                pdfPage.Header.Layout(htmlInHeader);

            }
        }

    }
}
