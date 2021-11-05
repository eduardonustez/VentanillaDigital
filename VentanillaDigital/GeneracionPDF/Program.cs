using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CorreoFactory;
using Generacion_PDF_Notaria.Data;
using Generacion_PDF_Notaria.EnviarCorreo;
using Generacion_PDF_Notaria.Models;

namespace Generacion_PDF_Notaria
{
    class Program
    {
        public static Conexion db = new Conexion();
        static void Main(string[] args)
        {
            Stopwatch timeMeasure = new Stopwatch();
            ManejadorCorreosFactory.SetCurrent(new ManejadorCorreosSendFactory());
            bool respuestagenerarPDFGraficaDiaria = false;
            bool respuestagenerarPDFListaConsumo = false;
            bool respuestaCorreo = false;
            bool respuestaImagenes = false;
            
            Console.WriteLine("Inicio a ejecutar las graficas");
            timeMeasure.Start();
            while (!respuestaImagenes)
            {
                eliminarimagen();
                ProcessStartInfo startInfo = new ProcessStartInfo(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + ConfigurationManager.AppSettings["nombrePython"].ToString(), "Script.py");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process proc = Process.Start(startInfo);
                proc.WaitForExit(); // Espera a que termine el proceso

                List<string> files = getAllFilesImagen(ConfigurationManager.AppSettings["RutaDocumentos"].ToString());
                if (files.Count() == 5)
                    respuestaImagenes = true;
                else
                    respuestaImagenes = false;

                Console.WriteLine("Respuesta Script: " + respuestaImagenes);
            }


            timeMeasure.Stop();
            Console.WriteLine("Termino de generar las graficas");
            Console.WriteLine($"Tiempo respuestaImagenes: {timeMeasure.Elapsed.TotalMilliseconds} ms");

            timeMeasure.Reset();
            timeMeasure.Start();

            while (!respuestagenerarPDFListaConsumo)
            {
                eliminar("Listado de Consumos diario por notarías ");
                respuestagenerarPDFListaConsumo = generarPDFListaConsumo();
                Console.WriteLine("Respuesta respuestagenerarPDFListaConsumo: " + respuestagenerarPDFListaConsumo);
            }

            timeMeasure.Stop();
            Console.WriteLine($"Tiempo generarPDFListaConsumo: {timeMeasure.Elapsed.TotalMilliseconds} ms");

            timeMeasure.Reset();
            timeMeasure.Start();
            
            while (!respuestagenerarPDFGraficaDiaria)
            {
                eliminar("Gráficas reporte diario de consumo ");
                respuestagenerarPDFGraficaDiaria = generarPDFGraficaDiaria();
                Console.WriteLine("Respuesta respuestagenerarPDFGraficaDiaria: " + respuestagenerarPDFGraficaDiaria);
            }
            timeMeasure.Stop();
            Console.WriteLine($"Tiempo generarPDFGraficaDiaria: {timeMeasure.Elapsed.TotalMilliseconds} ms");

            while (!respuestaCorreo)
            {
                respuestaCorreo = enviarCorreo();
                Console.WriteLine("Respuesta enviarCorreo: " + respuestaCorreo);
            }

            //Console.ReadKey();

        }
        public static List<string> getAllFilesImagen(string sDirt)
        {
            List<string> files = new List<string>();
            DateTime fecha = DateTime.Now;
            try
            {
                foreach (string file in Directory.GetFiles(sDirt, "*" + string.Format("{0:ddMMyyyy}", fecha) + ".jpg"))
                {
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return files;
        }
        public static List<string> getAllFiles(string sDirt)
        {
            List<string> files = new List<string>();
            DateTime fecha = DateTime.Now;
            try
            {
                foreach (string file in Directory.GetFiles(sDirt, "*" + string.Format("{0:ddMMyyyy}", fecha) + ".pdf"))
                {
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return files;
        }
        public static bool enviarCorreo()
        {
            ManejadorCorreos manejadorCorreo = new ManejadorCorreos();
            List<string> destinatarios = new List<string>();
            bool respuesta = false;

            try
            {
                ServidorCorreo servidorCorreo = new ServidorCorreo();
                servidorCorreo.host = ConfigurationManager.AppSettings["host"].ToString();
                servidorCorreo.usuarioFrom = ConfigurationManager.AppSettings["usuarioFrom"].ToString();
                servidorCorreo.password = ConfigurationManager.AppSettings["password"].ToString();
                servidorCorreo.port = ConfigurationManager.AppSettings["port"].ToString();
                servidorCorreo.nombreFrom = ConfigurationManager.AppSettings["nombreFrom"].ToString();

                string correos = ConfigurationManager.AppSettings["correo"].ToString();

                string[] correo = correos.Split(';');

                List<string> listCorreo = new List<string>();
                foreach (var correoEnvio in correo)
                {
                    listCorreo.Add(correoEnvio);
                }

                destinatarios = listCorreo;

                String mensajeHTML = "<p><b>Buen día,</b></p>";

                mensajeHTML += "Se adjuntan los reportes gráfico y de consumo para la UCNC correspondientes al día de hoy. <br> <br> Cordialmente,";

                List<Attachment> adjuntos = new List<Attachment>();

                List<string> files = getAllFiles(ConfigurationManager.AppSettings["RutaDocumentos"].ToString());

                foreach (string archivo in files)
                {
                    //comprobamos si existe el archivo y lo agregamos a los adjuntos
                    if (System.IO.File.Exists(@archivo))
                        adjuntos.Add(new Attachment(@archivo));

                }


                if (servidorCorreo != null)
                    respuesta = ManejadorCorreosFactory.Create().EnviarCorreo(servidorCorreo, destinatarios, null, "Reportes UCNC " + String.Format("{0:D}", DateTime.Now), mensajeHTML, false, adjuntos);
            }
            catch (Exception ex)
            {

                respuesta = false;
            }

            return respuesta;
        }
        public static bool generarPDFGraficaDiaria()
        {
            List<NotariaGrafica.Total> Listar_cotejos_Total = new List<NotariaGrafica.Total>();
            List<NotariaGrafica.MesTotal> Listar_cotejos_Total_mes = new List<NotariaGrafica.MesTotal>();
            List<NotariaGrafica.Total> Listar_cotejos_fijos = new List<NotariaGrafica.Total>();
            List<NotariaGrafica.MesTotal> Listar_cotejos_fijos_mes = new List<NotariaGrafica.MesTotal>();
            List<NotariaGrafica.Total> Listar_cotejos_movil = new List<NotariaGrafica.Total>();
            List<NotariaGrafica.MesTotal> Listar_cotejos_movil_mes = new List<NotariaGrafica.MesTotal>();
            List<NotariaGrafica.Cotejos> Listar_cotejos_30_fijo_segundo = new List<NotariaGrafica.Cotejos>();
            List<NotariaGrafica.Cotejos> Listar_cotejos_30_movil_segundo = new List<NotariaGrafica.Cotejos>();
            bool respuesta = false;
            try
            {
                if (validaciones())
                {
                    db.Consulta("Notaria.Listar_cotejos_Total");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.Total notaria = new NotariaGrafica.Total
                        {
                            Fecha = Convert.ToDateTime(db.Dr["Fecha"].ToString()),
                            Dias = db.Dr["Dias"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        Listar_cotejos_Total.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejos_Total_mes");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.MesTotal notaria = new NotariaGrafica.MesTotal
                        {
                            Mes = db.Dr["2.021"].ToString(),
                            Total = int.Parse(db.Dr["Cotejos"].ToString())
                        };
                        Listar_cotejos_Total_mes.Add(notaria);
                    }

                    //segunda pagina
                    db.Consulta("Notaria.Listar_cotejos_fijos");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.Total notaria = new NotariaGrafica.Total
                        {
                            Fecha = Convert.ToDateTime(db.Dr["Fecha"].ToString()),
                            Dias = db.Dr["Dias"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        Listar_cotejos_fijos.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejos_fijos_mes");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.MesTotal notaria = new NotariaGrafica.MesTotal
                        {
                            Mes = db.Dr["2.021"].ToString(),
                            Total = int.Parse(db.Dr["Cotejos"].ToString())
                        };
                        Listar_cotejos_fijos_mes.Add(notaria);
                    }

                    //tercera pagina
                    db.Consulta("Notaria.Listar_cotejos_movil");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.Total notaria = new NotariaGrafica.Total
                        {
                            Fecha = Convert.ToDateTime(db.Dr["Fecha"].ToString()),
                            Dias = db.Dr["Dias"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        Listar_cotejos_movil.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejos_movil_mes");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.MesTotal notaria = new NotariaGrafica.MesTotal
                        {
                            Mes = db.Dr["2.021"].ToString(),
                            Total = int.Parse(db.Dr["Cotejos"].ToString())
                        };
                        Listar_cotejos_movil_mes.Add(notaria);
                    }

                    //cuarta pagina
                    db.Consulta("Notaria.Listar_cotejos_30_fijo_segundo");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.Cotejos notaria = new NotariaGrafica.Cotejos
                        {
                            Fecha = Convert.ToDateTime(db.Dr["Fecha"].ToString()),
                            Dias = db.Dr["Dias"].ToString(),
                            MayoresA30 = int.Parse(db.Dr["MayoresA30"].ToString()),
                            MenoresA30 = int.Parse(db.Dr["MenoresA30"].ToString()),
                            Total = int.Parse(db.Dr["Total"].ToString())
                        };
                        Listar_cotejos_30_fijo_segundo.Add(notaria);
                    }
                    //quinta pagina
                    db.Consulta("Notaria.Listar_cotejos_30_movil_segundo");
                    while (db.Dr.Read())
                    {
                        NotariaGrafica.Cotejos notaria = new NotariaGrafica.Cotejos
                        {
                            Fecha = Convert.ToDateTime(db.Dr["Fecha"].ToString()),
                            Dias = db.Dr["Dias"].ToString(),
                            MayoresA30 = int.Parse(db.Dr["MayoresA30"].ToString()),
                            MenoresA30 = int.Parse(db.Dr["MenoresA30"].ToString()),
                            Total = int.Parse(db.Dr["Total"].ToString())
                        };
                        Listar_cotejos_30_movil_segundo.Add(notaria);
                    }
                }

                NotariaGrafica.Totales listar_cotejos_Total = Totales(Listar_cotejos_Total);
                NotariaGrafica.Totales listar_cotejos_Total_fijos = Totales(Listar_cotejos_fijos);
                NotariaGrafica.Totales listar_cotejos_Total_movil = Totales(Listar_cotejos_movil);


                byte[] pdfBuffer = Generacion_PDF_Notaria.GenerarPDF.GenerarPDF.ConverFromHTMLToPdf(getHtmlCodeSegundoReporte(Listar_cotejos_Total, listar_cotejos_Total, Listar_cotejos_Total_mes, Listar_cotejos_fijos, Listar_cotejos_fijos_mes, listar_cotejos_Total_fijos, Listar_cotejos_movil, Listar_cotejos_movil_mes, listar_cotejos_Total_movil, Listar_cotejos_30_fijo_segundo, Listar_cotejos_30_movil_segundo, totales()), getHeaderSegundoReporte(), "123");
                DateTime fecha = DateTime.Now;
                string nombreReporte = "Gráficas reporte diario de consumo " + string.Format("{0:ddMMyyyy}", fecha) + ".pdf";
                if (pdfBuffer != null)
                {
                    using (FileStream stream = System.IO.File.Create(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + nombreReporte))
                    {
                        string ToBase64String = Convert.ToBase64String(pdfBuffer);
                        byte[] byteArray = Convert.FromBase64String(ToBase64String);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    respuesta = true;
                }


                return respuesta;
            }
            catch (Exception Ex)
            {
                return respuesta;
            }
        }
        public static bool generarPDFListaConsumo()
        {
            List<Notaria> list_Notarias_Mayores_A_30 = new List<Notaria>();
            List<Notaria> list_Notarias_Menores_A_30 = new List<Notaria>();
            List<Notaria> list_Notarias_Mayores_A_30_Movil = new List<Notaria>();
            List<Notaria> list_Notarias_Menores_A_30_Movil = new List<Notaria>();
            List<Notaria> list_Notarias_Iguales_A_0 = new List<Notaria>();
            bool respuesta = false;
            try
            {
                if (validaciones())
                {
                    db.Consulta("Notaria.Listar_cotejo_mayores_a_30");
                    while (db.Dr.Read())
                    {
                        Notaria notaria = new Notaria
                        {
                            Notarias = db.Dr["Notaria"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        list_Notarias_Mayores_A_30.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejo_menores_a_30");
                    while (db.Dr.Read())
                    {
                        Notaria notaria = new Notaria
                        {
                            Notarias = db.Dr["Notaria"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        list_Notarias_Menores_A_30.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejo_mayores_a_30_movil");
                    while (db.Dr.Read())
                    {
                        Notaria notaria = new Notaria
                        {
                            Notarias = db.Dr["Notaria"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        list_Notarias_Mayores_A_30_Movil.Add(notaria);
                    }

                    db.Consulta("Notaria.Listar_cotejo_menores_a_30_movil");
                    while (db.Dr.Read())
                    {
                        Notaria notaria = new Notaria
                        {
                            Notarias = db.Dr["Notaria"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        list_Notarias_Menores_A_30_Movil.Add(notaria);
                    }
                    db.Consulta("Notaria.Listar_cotejo_con_0");
                    while (db.Dr.Read())
                    {
                        Notaria notaria = new Notaria
                        {
                            Notarias = db.Dr["Notaria"].ToString(),
                            HIT = int.Parse(db.Dr["HIT"].ToString()),
                            NOHIT = int.Parse(db.Dr["NO HIT"].ToString()),
                            TOTAL = int.Parse(db.Dr["TOTAL"].ToString())
                        };
                        list_Notarias_Iguales_A_0.Add(notaria);
                    }

                    list_Notarias_Mayores_A_30 = list_Notarias_Mayores_A_30.OrderByDescending(o => o.TOTAL).ToList();
                    list_Notarias_Menores_A_30 = list_Notarias_Menores_A_30.OrderByDescending(o => o.TOTAL).ToList();
                    list_Notarias_Mayores_A_30_Movil = list_Notarias_Mayores_A_30_Movil.OrderByDescending(o => o.TOTAL).ToList();
                    list_Notarias_Menores_A_30_Movil = list_Notarias_Menores_A_30_Movil.OrderByDescending(o => o.TOTAL).ToList();
                    list_Notarias_Iguales_A_0 = list_Notarias_Iguales_A_0.OrderByDescending(o => o.TOTAL).ToList();

                    byte[] pdfBuffer = Generacion_PDF_Notaria.GenerarPDF.GenerarPDF.ConverFromHTMLToPdf(getHtmlCode(list_Notarias_Mayores_A_30, list_Notarias_Menores_A_30, list_Notarias_Mayores_A_30_Movil, list_Notarias_Menores_A_30_Movil, list_Notarias_Iguales_A_0), getHeader(), "123");
                    DateTime fecha = DateTime.Now;
                    string nombreReporte = "Listado de Consumos diario por notarías " + string.Format("{0:ddMMyyyy}", fecha) + ".pdf";
                    if (pdfBuffer != null)
                    {

                        using (FileStream stream = System.IO.File.Create(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + nombreReporte))
                        {
                            string ToBase64String = Convert.ToBase64String(pdfBuffer);
                            byte[] byteArray = Convert.FromBase64String(ToBase64String);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                    }
                    respuesta = true;
                }
                else
                    respuesta = false;

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = false;
                return respuesta;
            }
        }
        public static string getHeader()
        {
            string header = string.Empty;
            header = @"<!DOCTYPE html>
                                <html lang=""es"">
                                <head>
                                    <meta charset = ""UTF-8"">
 
                                     <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"">
    
                                        <meta http-equiv = ""X-UA-Compatible"" content = ""ie=edge"">
         

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
                                </head>
                                <body>
                                     <div class=""wrapper-print"">
                                        <div class=""flex-container"">
                                            <div class=""logo-ucnc"">
                                                <img src = ""data:image/jpeg;base64,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RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAA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SU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg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""
                                                    alt="""">
                                            </div>
                                            <div>
                                                <h1>REPORTE DE CONSUMO</h1>
                                            </div>
                                            <div class=""logo"">
                                                <p>Elaborado por:</p>
                                                <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                                                    alt="""">";
            header += String.Format(@"<div class=""text-fecha"">
                        <p>Fecha de elaboración:</p>
                        <p>{0}</p>
                    </div>", String.Format("{0:D}", DateTime.Now));

            header += "</div> " +
                " </div>" +
                "</div>" +
                "</body>" +
                "</html>";

            return header;
        }
        public static string getHtmlCode(List<Notaria> list_Notarias_Mayores_A_30, List<Notaria> list_Notarias_Menores_A_30, List<Notaria> list_Notarias_Mayores_A_30_Movil, List<Notaria> list_Notarias_Menores_A_30_Movil, List<Notaria> list_Notarias_Iguales_A_0)
        {
            string total = "Total";
            string htmlCode = string.Empty;
            htmlCode = @"<!DOCTYPE html>
                                        <html lang=""es"">
                                        <head>
                                            <meta charset = ""UTF-8"">
 
                                             <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"">
    
                                                <meta http-equiv = ""X-UA-Compatible"" content = ""ie=edge"">
         

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
                                        </head>
                                        <body>";
            if (list_Notarias_Mayores_A_30.Count > 0)
            {
                htmlCode += @"<div class=""wrapper-print"">
                          <div class=""flex-container"">
                            <div class=""text-doc""></div>
                            <div class=""text-doc"">
                                <span class=""titulo-tabla"" style=""text-align: center""> COTEJOS MAYORES A 30 (BIOMETRÍA FIJA)</span>
                            </div>
                            <div class=""text-doc""></div>
                        </div>
                        <div class=""p__table"">
                            <table width = ""100%"" cellspacing=""0"" cellpadding=""0"" class=""table"">
                                <thead>
                                    <tr>
                                        <th class=""tl-c numeros"">NÚMERO</th>
                                        <th>RAZÓN SOCIAL</th>
                                        <th class=""numeros"">HIT</th>
                                        <th class=""numeros"">NO HIT</th>
                                        <th class=""tr-c numeros"">TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>";

                int i = 1;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (Notaria item in list_Notarias_Mayores_A_30)
                {

                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"
                     <tr>
                     <td class=""tl-c numeros"">{0}</td>
                     <td>{1}</td>
                     <td class=""numeros"">{2}</td>
                     <td class=""numeros"">{3}</td>
                     <td class=""numeros"">{4}</td>", i, item.Notarias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));

                    htmlCode += @"</tr>";
                    i++;
                }
                htmlCode += String.Format(@"<tr>
                             <td colspan=""2"">{0}</td>
                             <td class=""numeros"">{1}</td>
                             <td class=""numeros"">{2}</td>
                             <td class=""numeros"">{3}</td>", total, decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL));

                htmlCode += @"</tr>" +
                           "</tbody>" +
                           "</table> " +
                           "</div>" +
                          "</div> ";
            }

            if (list_Notarias_Menores_A_30.Count > 0)
            {
                htmlCode += @"<div class=""wrapper-print"">
                          <div class=""flex-container"">
                            <div class=""text-doc""></div>
                            <div class=""text-doc"">
                                <span class=""titulo-tabla"">COTEJOS MENORES A 30 (BIOMETRÍA FIJA)</span>
                            </div>
                           <div class=""text-doc""></div>
                        </div>
                        <div class=""p__table"">
                            <table width = ""100%"" cellspacing=""0"" cellpadding=""0"" class=""table"">
                                <thead>
                                    <tr>
                                        <th class=""tl-c numeros"">NÚMERO</th>
                                        <th>RAZÓN SOCIAL</th>
                                        <th class=""numeros"">HIT</th>
                                        <th class=""numeros"">NO HIT</th>
                                        <th class=""tr-c numeros"">TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>";

                int i = 1;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (Notaria item in list_Notarias_Menores_A_30)
                {
                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"
                     <tr>
                     <td class=""tl-c numeros"">{0}</td>
                     <td>{1}</td>
                     <td class=""numeros"">{2}</td>
                     <td class=""numeros"">{3}</td>
                     <td class=""numeros"">{4}</td>", i, item.Notarias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));

                    htmlCode += @"</tr>";
                    i++;
                }
                htmlCode += String.Format(@"<tr>
                             <td colspan=""2"">{0}</td>
                             <td class=""numeros"">{1}</td>
                             <td class=""numeros"">{2}</td>
                             <td class=""numeros"">{3}</td>", total, decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL));

                htmlCode += @"</tr>" +
                           "</tbody>" +
                           "</table> " +
                           "</div>" +
                          "</div> ";
            }
            //movil
            if (list_Notarias_Mayores_A_30_Movil.Count > 0)
            {
                htmlCode += @"<div class=""wrapper-print"">
                          <div class=""flex-container"">
                            <div class=""text-doc""></div>
                            <div class=""text-doc"">
                                <span class=""titulo-tabla"" style=""text-align: center""> COTEJOS MAYORES A 30 (BIOMETRÍA MÓVIL))</span>
                            </div>
                            <div class=""text-doc""></div>
                        </div>
                        <div class=""p__table"">
                            <table width = ""100%"" cellspacing=""0"" cellpadding=""0"" class=""table"">
                                <thead>
                                    <tr>
                                        <th class=""tl-c numeros"">NÚMERO</th>
                                        <th>RAZÓN SOCIAL</th>
                                        <th class=""numeros"">HIT</th>
                                        <th class=""numeros"">NO HIT</th>
                                        <th class=""tr-c numeros"">TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>";

                int i = 1;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (Notaria item in list_Notarias_Mayores_A_30_Movil)
                {

                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"
                     <tr>
                     <td class=""tl-c numeros"">{0}</td>
                     <td>{1}</td>
                     <td class=""numeros"">{2}</td>
                     <td class=""numeros"">{3}</td>
                     <td class=""numeros"">{4}</td>", i, item.Notarias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));

                    htmlCode += @"</tr>";
                    i++;
                }
                htmlCode += String.Format(@"<tr>
                             <td colspan=""2"">{0}</td>
                             <td class=""numeros"">{1}</td>
                             <td class=""numeros"">{2}</td>
                             <td class=""numeros"">{3}</td>", total, decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL));

                htmlCode += @"</tr>" +
                           "</tbody>" +
                           "</table> " +
                           "</div>" +
                          "</div> ";
            }

            if (list_Notarias_Menores_A_30_Movil.Count > 0)
            {
                htmlCode += @"<div class=""wrapper-print"">
                          <div class=""flex-container"">
                            <div class=""text-doc""></div>
                            <div class=""text-doc"">
                                <span class=""titulo-tabla"">COTEJOS MENORES A 30 (BIOMETRÍA MÓVIL))</span>
                            </div>
                           <div class=""text-doc""></div>
                        </div>
                        <div class=""p__table"">
                            <table width = ""100%"" cellspacing=""0"" cellpadding=""0"" class=""table"">
                                <thead>
                                    <tr>
                                        <th class=""tl-c numeros"">NÚMERO</th>
                                        <th>RAZÓN SOCIAL</th>
                                        <th class=""numeros"">HIT</th>
                                        <th class=""numeros"">NO HIT</th>
                                        <th class=""tr-c numeros"">TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>";

                int i = 1;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (Notaria item in list_Notarias_Menores_A_30_Movil)
                {
                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"
                     <tr>
                     <td class=""tl-c numeros"">{0}</td>
                     <td>{1}</td>
                     <td class=""numeros"">{2}</td>
                     <td class=""numeros"">{3}</td>
                     <td class=""numeros"">{4}</td>", i, item.Notarias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));

                    htmlCode += @"</tr>";
                    i++;
                }
                htmlCode += String.Format(@"<tr>
                             <td colspan=""2"">{0}</td>
                             <td class=""numeros"">{1}</td>
                             <td class=""numeros"">{2}</td>
                             <td class=""numeros"">{3}</td>", total, decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL));

                htmlCode += @"</tr>" +
                           "</tbody>" +
                           "</table> " +
                           "</div>" +
                          "</div> ";
            }

            //
            if (list_Notarias_Iguales_A_0.Count > 0)
            {
                htmlCode += @"<div class=""wrapper-print"">
                          <div class=""flex-container"">
                            <div class=""text-doc""></div>
                            <div class=""text-doc"">
                                <span class=""titulo-tabla"">NOTARÍAS CON 0 COTEJOS</span>
                            </div>
                           <div class=""text-doc""></div>
                        </div>
                        <div class=""p__table"">
                            <table width = ""100%"" cellspacing=""0"" cellpadding=""0"" class=""table"">
                                <thead>
                                    <tr>
                                        <th class=""tl-c numeros"">NÚMERO</th>
                                        <th>RAZÓN SOCIAL</th>
                                        <th class=""numeros"">HIT</th>
                                        <th class=""numeros"">NO HIT</th>
                                        <th class=""tr-c numeros"">TOTAL</th>
                                    </tr>
                                </thead>
                                <tbody>";

                int i = 1;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (Notaria item in list_Notarias_Iguales_A_0)
                {
                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"
                     <tr>
                     <td class=""tl-c numeros"">{0}</td>
                     <td>{1}</td>
                     <td class=""numeros"">{2}</td>
                     <td class=""numeros"">{3}</td>
                     <td class=""numeros"">{4}</td>", i, item.Notarias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));

                    htmlCode += @"</tr>";
                    i++;
                }
                htmlCode += String.Format(@"<tr>
                             <td colspan=""2"">{0}</td>
                             <td class=""numeros"">{1}</td>
                             <td class=""numeros"">{2}</td>
                             <td class=""numeros"">{3}</td>", total, decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL));

                htmlCode += @"</tr>" +
                           "</tbody>" +
                           "</table> " +
                           "</div>" +
                          "</div> ";
            }
            htmlCode += @" </body>
                   </html>";
            return htmlCode;
        }
        public static string getHeaderSegundoReporte()
        {
            string header = string.Empty;
            header = @"<!DOCTYPE html>
                            <html lang=""es"">

                            <head>
                                <meta charset = ""UTF-8"">
 
                                 <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"">
   
                                    <meta http - equiv = ""X-UA-Compatible"" content = ""ie=edge"">
        
                                         <title> Reporte de consumo de servicio de cotejos</title>
                                        <style type=""text/css"">
                                           @page {
                                                     size: A4 landscape;
                                                     margin: 0.5cm;
                                                     }
 
                                                     * {
                                                     margin: 0px;
                                                     padding: 0px;
                                                     color: rgb(49, 49, 49);
                                                     font-family: 'Arial', sans-serif;
                                                     }
 
                                                     .pagina {
                                                     page-break-after: always;
                                                        width: 1200px;
                                                     }
                                                    .paginas {
                                                    
                                                        width: 1200px;
                                                     }
 
                                                     .wrapper-print {
                                                     margin: 10px;
                                                     border-radius: 8px;
                                                     padding: 0;
                                                     height: 100%;
                                                     }
 
                                                     .flex-container {
                                                     padding: 1px 0 10px;
                                                     display: flex;
                                                     justify-content: space-between;
                                                     align-items: center;
                                                     }
 
                                                     .header {
                                                     padding: 0 10px;
                                                     }
 
                                                     h1 {
                                                     text-align: center;
                                                     font-size: 14pt;
                                                     }
 
                                                     .logo-ucnc,
                                                     .logo-ucnc img {
                                                     width: 1.7cm;
                                                     }
 
                                                     .logo {
                                                     width: 4.5cm;
                                                     text-align: right;
                                                     }
 
                                                     .logo img {
                                                     width: 2.5cm;
                                                     }
 
                                                     .logo p {
                                                     font-size: 12px;
                                                     margin-bottom: 5px;
                                                     font-weight: 200;
                                                     line-height: 1;
                                                     }
 
                                                     .text-doc {
                                                     font-size: 18px;
                                                     font-weight: bold;
                                                     text-align: right;
                                                     line-height: 15px;
                                                     white-space: nowrap;
                                                     }
 
                                                     .body-flex {
                                                     display: flex;
                                                     justify-content: space-between;
                                                     }
 
                                                     .tabla-uno-contenedor {
                                                     width: 7cm;
                                                     margin-right: 15px;
                                                     }
 
                                                     .grafica-contenedor {
                                                     width: 19cm;
                                                     margin-right: 10px;
                                                     }
 
                                                     .grafica-contenedor img {
                                                     width: 100%;
                                                     }
 
                                                     .tabla-dos-contenedor {
                                                     width: 4cm;
                                                     }
 
                                                     .grafica-contenedor.notarias {
                                                     width: 21cm;
                                                     }
 
                                                     .tabla-uno-contenedor.notarias {
                                                     width: 8cm;
                                                     }
 
                                                     .tabla {
                                                     border: 1px solid #dedede;
                                                     border-collapse: collapse;
                                                     font-variant-numeric: tabular-nums;
                                                     }
 
                                                     .tabla tfoot td,
                                                     .tabla tbody td {
                                                     border: 1px solid #dedede;
                                                     }
 
                                                     .titulo-tabla,
                                                     .titulo-tabla span {
                                                     color: #c20600;
                                                     white-space: normal;
                                                     text-align: center;
                                                     margin-bottom: 10px;
                                                     }
 
                                                     .tabla .numeros {
                                                     text-align: right;
                                                     font-variant-numeric: tabular-nums;
                                                     }
 
                                                     td {
                                                     font-size: 10px;
                                                     font-weight: 200;
                                                     padding: 3px 5px;
                                                     border-right: #dedede;
                                                     white-space: nowrap;
                                                     }
 
                                                     tbody tr:nth-child(even) {
                                                     background-color: hsl(38, 10%, 96%);
                                                     -webkit-print-color-adjust: exact;
                                                     color-adjust: exact;
                                                     }
 
                                                     .tabla thead th {
                                                     font-size: 10px;
                                                     text-align: left;
                                                     padding: 5px 10px;
                                                     -webkit-print-color-adjust: exact;
                                                     color: #272727;
                                                     vertical-align: middle;
                                                     text-align: center;
                                                     white-space: nowrap;
                                                     border: 1px solid #dedede;
                                                     }
 
                                                     .subtitulo-tabla {
                                                     text-align: right;
                                                     }
 
                                                     .red {
                                                     color: red;
                                                     }
 
                                                     .total,
                                                     .subtitulo-tabla strong,
                                                     .tabla-anios .numeros.total,
                                                     .tabla tfoot td {
                                                     background-color: #e6fff1;
                                                     color: #05612b;
                                                     -webkit-print-color-adjust: exact;
                                                     font-weight: bold;
                                                     }
 
                                                     .tabla-dos-contenedor table {
                                                     margin-top: 10px;
                                                     width: 100%;
                                                     }
 
                                                     .tabla-mes thead th,
                                                     .tabla-mes td {
                                                     padding: 1px 5px;
                                                     }
                                </style>
                            </head>

                            <body>
                                <header>
                                    <div class=""flex-container header"">
                                        <div class=""logo-ucnc"">
                                            <img src = ""data:image/jpeg;base64,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RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAABmJvdW5kc09iamMAAAABAAAAAAAAUmN0MQAAAAQAAAAAVG9wIGxvbmcAAAAAAAAAAExlZnRsb25nAAAAAAAAAABCdG9tbG9uZwAAAGYAAAAAUmdodGxvbmcAAABbAAAABnNsaWNlc1ZsTHMAAAABT2JqYwAAAAEAAAAAAAVzbGljZQAAABIAAAAHc2xpY2VJRGxvbmcAAAAAAAAAB2dyb3VwSURsb25nAAAAAAAAAAZvcmlnaW5lbnVtAAAADEVTbGljZU9yaWdpbgAAAA1hdXRvR2VuZXJhdGVkAAAAAFR5cGVlbnVtAAAACkVTbGljZVR5cGUAAAAASW1nIAAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAABmAAAAAFJnaHRsb25nAAAAWwAAAAN1cmxURVhUAAAAAQAAAAAAAG51bGxURVhUAAAAAQAAAAAAAE1zZ2VURVhUAAAAAQAAAAAABmFsdFRhZ1RFWFQAAAABAAAAAAAOY2VsbFRleHRJc0hUTUxib29sAQAAAAhjZWxsVGV4dFRFWFQAAAABAAAAAAAJaG9yekFsaWduZW51bQAAAA9FU2xpY2VIb3J6QWxpZ24AAAAHZGVmYXVsdAAAAAl2ZXJ0QWxpZ25lbnVtAAAAD0VTbGljZVZlcnRBbGlnbgAAAAdkZWZhdWx0AAAAC2JnQ29sb3JUeXBlZW51bQAAABFFU2xpY2VCR0NvbG9yVHlwZQAAAABOb25lAAAACXRvcE91dHNldGxvbmcAAAAAAAAACmxlZnRPdXRzZXRsb25nAAAAAAAAAAxib3R0b21PdXRzZXRsb25nAAAAAAAAAAtyaWdodE91dHNldGxvbmcAAAAAADhCSU0EKAAAAAAADAAAAAI/8AAAAAAAADhCSU0EFAAAAAAABAAAAAU4QklNBAwAAAAAA7IAAAABAAAAFQAAABgAAABAAAAGAAAAA5YAGAAB/9j/7QAMQWRvYmVfQ00AAf/uAA5BZG9iZQBkgAAAAAH/2wCEAAwICAgJCAwJCQwRCwoLERUPDAwPFRgTExUTExgRDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBDQsLDQ4NEA4OEBQODg4UFA4ODg4UEQwMDAwMEREMDAwMDAwRDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIABgAFQMBIgACEQEDEQH/3QAEAAL/xAE/AAABBQEBAQEBAQAAAAAAAAADAAECBAUGBwgJCgsBAAEFAQEBAQEBAAAAAAAAAAEAAgMEBQYHCAkKCxAAAQQBAwIEAgUHBggFAwwzAQACEQMEIRIxBUFRYRMicYEyBhSRobFCIyQVUsFiMzRygtFDByWSU/Dh8WNzNRaisoMmRJNUZEXCo3Q2F9JV4mXys4TD03Xj80YnlKSFtJXE1OT0pbXF1eX1VmZ2hpamtsbW5vY3R1dnd4eXp7fH1+f3EQACAgECBAQDBAUGBwcGBTUBAAIRAyExEgRBUWFxIhMFMoGRFKGxQiPBUtHwMyRi4XKCkkNTFWNzNPElBhaisoMHJjXC0kSTVKMXZEVVNnRl4vKzhMPTdePzRpSkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2JzdHV2d3h5ent8f/2gAMAwEAAhEDEQA/AO16j1/H6fkn7bkmi1tmz0msc9jan73VWOaxn6ay1tW/+c/Qf9ufaK1f1rwHkMxsm6y17N1jRVZa4QR6z66xTtr9Kv3Nf/Mf6WpZ31morzOo7LSWHIa19G2ADY37XXQyxzvosupo/wC3FRFPTj0Gyixt/T35A3WZQLZsNcluH6N9mPbkV37n101U+x9n6SxQSnISoVTrYOWwSwiUhLjPCPTw8I4uvy8cnrB1bG+xusOU8dPD21NyNrvVJLXmykmN/t/R/re3f/b/AFtJZIaP+bbiWwRc25rZ5eb34LmTP5rmssSUnEfwtqe0L6/znB0/d/6T/9DtOt9Buym1+lDjUXem8aODS4XtqdH0XU217sTKb/M/zduNbV6+/Dr+qWZZki+42W2NO4PsPqEFv82/Y9lLbXfuV3WUV7/5yp9S8SSUGT2+IW6vKfevYl7fDw0f739Z+kx0WOmuwtlZrdUKPQkwKwHcX7f6S61/rfaPQ/6z/hkl82JKX0/g0f1v/P8AH5/5f4b/AP/ZOEJJTQQhAAAAAABXAAAAAQEAAAAPAEEAZABvAGIAZQAgAFAAaABvAHQAbwBzAGgAbwBwAAAAFABBAGQAbwBiAGUAIABQAGgAbwB0AG8AcwBoAG8AcAAgADIAMAAyADEAAAABADhCSU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg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""
                                                alt="""">
                                        </div>
                                        <div>
                                            <h1>Informe consumo de servicio de cotejos biométricos en línea Fijos y Móviles</h1>
                                        </div>
                                        <div class=""logo"">
                                            <p>Elaborado por:</p>
                                            <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                                                alt="""">";
            header += String.Format(@"<div class=""text-doc"">
                        <p>Fecha de elaboración:</p>
                        <p>{0}</p>
                    </div>", String.Format("{0:D}", DateTime.Now));

            header += "</div> " +
                " </div>" +
                "</header>" +
                "</body>" +
                "</html>";

            return header;
        }
        public static string getHtmlCodeSegundoReporte(List<NotariaGrafica.Total> Listar_cotejos_Total, NotariaGrafica.Totales listar_cotejos_Total, List<NotariaGrafica.MesTotal> Listar_cotejos_Total_mes, List<NotariaGrafica.Total> Listar_cotejos_fijos, List<NotariaGrafica.MesTotal> Listar_cotejos_fijos_mes, NotariaGrafica.Totales listar_cotejos_Total_fijos, List<NotariaGrafica.Total> Listar_cotejos_movil, List<NotariaGrafica.MesTotal> Listar_cotejos_movil_mes, NotariaGrafica.Totales listar_cotejos_Total_movil, List<NotariaGrafica.Cotejos> fijos, List<NotariaGrafica.Cotejos> movil, List<Totalesanios> listtotal)
        {

            string htmlCode = string.Empty;
            htmlCode = @"<!DOCTYPE html>
                            <html lang=""es"">

                            <head>
                                <meta charset = ""UTF-8"">
 

                                 <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"">
    

                                    <meta http - equiv = ""X-UA-Compatible"" content = ""ie=edge"">
         


                                         <title> Reporte de consumo de servicio de cotejos</title>
           

                                      <style type=""text/css"">
                                                                                       @page {
                                                     size: A4 landscape;
                                                     margin: 0.5cm;
                                                     }
 
                                                     * {
                                                     margin: 0px;
                                                     padding: 0px;
                                                     color: rgb(49, 49, 49);
                                                     font-family: 'Arial', sans-serif;
                                                     }
 
                                                     .pagina {
                                                    page-break-after: always;
                                                    width: 1200px;
                                                     }
 
                                                    .paginas {
                                                     
                                                        width: 1200px;
                                                     }
                                                     .wrapper-print {
                                                     margin: 10px;
                                                     border-radius: 8px;
                                                     padding: 0;
                                                     height: 100%;
                                                     }
 
                                                     .flex-container {
                                                     padding: 1px 0 10px;
                                                     display: flex;
                                                     justify-content: space-between;
                                                     align-items: center;
                                                     }
 
                                                     .header {
                                                     padding: 0 10px;
                                                     }
 
                                                     h1 {
                                                     text-align: center;
                                                     font-size: 14pt;
                                                     }
 
                                                     .logo-ucnc,
                                                     .logo-ucnc img {
                                                     width: 1.7cm;
                                                     }
 
                                                     .logo {
                                                     width: 4.5cm;
                                                     text-align: right;
                                                     }
 
                                                     .logo img {
                                                     width: 2.5cm;
                                                     }
 
                                                     .logo p {
                                                     font-size: 12px;
                                                     margin-bottom: 5px;
                                                     font-weight: 200;
                                                     line-height: 1;
                                                     }
 
                                                     .text-doc {
                                                     font-size: 18px;
                                                     font-weight: bold;
                                                     text-align: right;
                                                     line-height: 15px;
                                                     white-space: nowrap;
                                                     }
 
                                                     .body-flex {
                                                     display: flex;
                                                     justify-content: space-between;
                                                     }
 
                                                     .tabla-uno-contenedor {
                                                     width: 7cm;
                                                     margin-right: 15px;
                                                     }
 
                                                     .grafica-contenedor {
                                                     width: 19cm;
                                                     margin-right: 10px;
                                                     }
 
                                                     .grafica-contenedor img {
                                                     width: 100%;
                                                     }
 
                                                     .tabla-dos-contenedor {
                                                     width: 4cm;
                                                     }
 
                                                     .grafica-contenedor.notarias {
                                                     width: 21cm;
                                                     }
 
                                                     .tabla-uno-contenedor.notarias {
                                                     width: 8cm;
                                                     }
 
                                                     .tabla {
                                                     border: 1px solid #dedede;
                                                     border-collapse: collapse;
                                                     font-variant-numeric: tabular-nums;
                                                     }
 
                                                     .tabla tfoot td,
                                                     .tabla tbody td {
                                                     border: 1px solid #dedede;
                                                     }
 
                                                     .titulo-tabla,
                                                     .titulo-tabla span {
                                                     color: #c20600;
                                                     white-space: normal;
                                                     text-align: center;
                                                     margin-bottom: 10px;
                                                     }
 
                                                     .tabla .numeros {
                                                     text-align: right;
                                                     font-variant-numeric: tabular-nums;
                                                     }
 
                                                     td {
                                                     font-size: 10px;
                                                     font-weight: 200;
                                                     padding: 3px 5px;
                                                     border-right: #dedede;
                                                     white-space: nowrap;
                                                     }
 
                                                     tbody tr:nth-child(even) {
                                                     background-color: hsl(38, 10%, 96%);
                                                     -webkit-print-color-adjust: exact;
                                                     color-adjust: exact;
                                                     }
 
                                                     .tabla thead th {
                                                     font-size: 10px;
                                                     text-align: left;
                                                     padding: 5px 10px;
                                                     -webkit-print-color-adjust: exact;
                                                     color: #272727;
                                                     vertical-align: middle;
                                                     text-align: center;
                                                     white-space: nowrap;
                                                     border: 1px solid #dedede;
                                                     }
 
                                                     .subtitulo-tabla {
                                                     text-align: right;
                                                     }
 
                                                     .red {
                                                     color: red;
                                                     }
 
                                                     .total,
                                                     .subtitulo-tabla strong,
                                                     .tabla-anios .numeros.total,
                                                     .tabla tfoot td {
                                                     background-color: #e6fff1;
                                                     color: #05612b;
                                                     -webkit-print-color-adjust: exact;
                                                     font-weight: bold;
                                                     }
 
                                                     .tabla-dos-contenedor table {
                                                     margin-top: 10px;
                                                     width: 100%;
                                                     }
 
                                                     .tabla-mes thead th,
                                                     .tabla-mes td {
                                                     padding: 1px 5px;
                                                     }
                                </style>
                            </head>

                            <body>";
            htmlCode += String.Format(@"<div class=""pagina"">
                            <div class=""wrapper-print body-flex"">
                                <div class=""tabla-uno-contenedor"">
                                    <table cellspacing = ""0"" cellpadding=""0"" class=""tabla tabla-mes"">
                                        <h4 class=""titulo-tabla"">Reporte de Cotejos Total <span> {0}", String.Format("{0:y}", Listar_cotejos_Total[0].Fecha));
            htmlCode += @"</span> </h4>
                                        <thead>";
            if (Listar_cotejos_Total.Count > 0)
            {
                htmlCode += @"<tr>
                                                <th rowspan = ""2"">#</th>
                                                <th rowspan= ""2""> FECHA </th>
                                                <th colspan= ""3""> COTEJOS </th>
                                            </tr>
                                            <tr>
                                                <th> HIT </th>
                                                <th> NO HIT</th>
                                                <th>TOTAL</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
                int i = 1;
                int j = 0;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (NotariaGrafica.Total item in Listar_cotejos_Total)
                {

                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td>{1}</td>
                                                <td class=""numeros"">{2}</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>", i, item.Dias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));
                    htmlCode += @"</tr>";
                    i++;
                    j++;
                }
                htmlCode += String.Format(@"</tbody>
                                        <tfoot>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio total</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>
                                                <td class=""numeros"">{5}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio día hábil</td>", decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL), decimales(totalHIT / j), decimales(totalNOHIT / j), decimales(totaTOTAL / j));
                htmlCode += String.Format(@"<td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>", decimales(listar_cotejos_Total.HIT), decimales(listar_cotejos_Total.NOHIT), decimales(listar_cotejos_Total.TOTAL));
                htmlCode += @"</tr>
                                        </tfoot>
                                    </table>
                                </div>
                                <div class=""grafica-contenedor"">";

            }
            htmlCode += String.Format(@"<img src = ""{0}"" alt="""">", imageToBase64(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "cotejos_totales_p1 " + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".jpg"));
            if (listtotal.Count > 0)
            {
                htmlCode += @"</div>
                                <div class=""tabla-dos-contenedor"">
                                    <table class=""tabla tabla-anios"">
                                        <thead>
                                            <tr>
                                                <th>AÑO</th>
                                                <th>TOTAL COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";

                Totalesanios totalesanios = new Totalesanios
                {
                    Producto = "Total",
                    Total = Listar_cotejos_Total_mes.Sum(item => item.Total),
                    Anio = 2021
                };
                listtotal.Add(totalesanios);
                foreach (Totalesanios item in listtotal.Where(x => x.Producto == "Total"))
                {


                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros total"">{1}</td>
                                            </tr>", decimales(item.Anio), decimales(item.Total));
                }
            }

            htmlCode += @" </tbody>
                             </table>
                                <table cellspacing = ""0"" cellpadding=""0"" class=""tabla"">
                                        <thead>
                                            <tr>
                                                <th>2.021</th>
                                                <th>COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
            if (Listar_cotejos_Total_mes.Count > 0)
            {
                int i = 1;
                int j = 0;
                int Total = 0;
                int Promedio = 0;
                foreach (NotariaGrafica.MesTotal item in Listar_cotejos_Total_mes)
                {
                    htmlCode += String.Format(@"<tr>
                                                <td>{0}</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", item.Mes, decimales(item.Total));

                    Total += item.Total;
                    Promedio += item.Total;
                    i++;
                    j++;
                }

                htmlCode += String.Format(@" </tbody>
                                        <tfoot>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                            </tr>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Promedio</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", decimales(Total), decimales(Promedio / j));
            }
            htmlCode += @"</tfoot>
                        </table>
                    </div>
                </div>
            </div>";


            //segunda pagina
            htmlCode += @"<div class=""pagina"">
                                    <div>
                                        <div class=""flex-container header"">
                                            <div class=""logo-ucnc"">
                                                <img src = ""data:image/jpeg;base64,/9j/4QTQRXhpZgAATU0AKgAAAAgABwESAAMAAAABAAEAAAEaAAUAAAABAAAAYgEbAAUAAAABAAAAagEoAAMAAAABAAIAAAExAAIAAAAhAAAAcgEyAAIAAAAUAAAAk4dpAAQAAAABAAAAqAAAANQALcbAAAAnEAAtxsAAACcQQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkAMjAyMTowMToyMSAxMToyNzoxMAAAAAOgAQADAAAAAQABAACgAgAEAAAAAQAAAFugAwAEAAAAAQAAAGYAAAAAAAAABgEDAAMAAAABAAYAAAEaAAUAAAABAAABIgEbAAUAAAABAAABKgEoAAMAAAABAAIAAAIBAAQAAAABAAABMgICAAQAAAABAAADlgAAAAAAAABIAAAAAQAAAEgAAAAB/9j/7QAMQWRvYmVfQ00AAf/uAA5BZG9iZQBkgAAAAAH/2wCEAAwICAgJCAwJCQwRCwoLERUPDAwPFRgTExUTExgRDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBDQsLDQ4NEA4OEBQODg4UFA4ODg4UEQwMDAwMEREMDAwMDAwRDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIABgAFQMBIgACEQEDEQH/3QAEAAL/xAE/AAABBQEBAQEBAQAAAAAAAAADAAECBAUGBwgJCgsBAAEFAQEBAQEBAAAAAAAAAAEAAgMEBQYHCAkKCxAAAQQBAwIEAgUHBggFAwwzAQACEQMEIRIxBUFRYRMicYEyBhSRobFCIyQVUsFiMzRygtFDByWSU/Dh8WNzNRaisoMmRJNUZEXCo3Q2F9JV4mXys4TD03Xj80YnlKSFtJXE1OT0pbXF1eX1VmZ2hpamtsbW5vY3R1dnd4eXp7fH1+f3EQACAgECBAQDBAUGBwcGBTUBAAIRAyExEgRBUWFxIhMFMoGRFKGxQiPBUtHwMyRi4XKCkkNTFWNzNPElBhaisoMHJjXC0kSTVKMXZEVVNnRl4vKzhMPTdePzRpSkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2JzdHV2d3h5ent8f/2gAMAwEAAhEDEQA/AO16j1/H6fkn7bkmi1tmz0msc9jan73VWOaxn6ay1tW/+c/Qf9ufaK1f1rwHkMxsm6y17N1jRVZa4QR6z66xTtr9Kv3Nf/Mf6WpZ31morzOo7LSWHIa19G2ADY37XXQyxzvosupo/wC3FRFPTj0Gyixt/T35A3WZQLZsNcluH6N9mPbkV37n101U+x9n6SxQSnISoVTrYOWwSwiUhLjPCPTw8I4uvy8cnrB1bG+xusOU8dPD21NyNrvVJLXmykmN/t/R/re3f/b/AFtJZIaP+bbiWwRc25rZ5eb34LmTP5rmssSUnEfwtqe0L6/znB0/d/6T/9DtOt9Buym1+lDjUXem8aODS4XtqdH0XU217sTKb/M/zduNbV6+/Dr+qWZZki+42W2NO4PsPqEFv82/Y9lLbXfuV3WUV7/5yp9S8SSUGT2+IW6vKfevYl7fDw0f739Z+kx0WOmuwtlZrdUKPQkwKwHcX7f6S61/rfaPQ/6z/hkl82JKX0/g0f1v/P8AH5/5f4b/AP/Z/+0M0FBob3Rvc2hvcCAzLjAAOEJJTQQlAAAAAAAQAAAAAAAAAAAAAAAAAAAAADhCSU0EOgAAAAAA7wAAABAAAAABAAAAAAALcHJpbnRPdXRwdXQAAAAFAAAAAFBzdFNib29sAQAAAABJbnRlZW51bQAAAABJbnRlAAAAAENscm0AAAAPcHJpbnRTaXh0ZWVuQml0Ym9vbAAAAAALcHJpbnRlck5hbWVURVhUAAAAAQAAAAAAD3ByaW50UHJvb2ZTZXR1cE9iamMAAAARAEEAagB1AHMAdABlACAAZABlACAAcAByAHUAZQBiAGEAAAAAAApwcm9vZlNldHVwAAAAAQAAAABCbHRuZW51bQAAAAxidWlsdGluUHJvb2YAAAAJcHJvb2ZDTVlLADhCSU0EOwAAAAACLQAAABAAAAABAAAAAAAScHJpbnRPdXRwdXRPcHRpb25zAAAAFwAAAABDcHRuYm9vbAAAAAAAQ2xicmJvb2wAAAAAAFJnc01ib29sAAAAAABDcm5DYm9vbAAAAAAAQ250Q2Jvb2wAAAAAAExibHNib29sAAAAAABOZ3R2Ym9vbAAAAAAARW1sRGJvb2wAAAAAAEludHJib29sAAAAAABCY2tnT2JqYwAAAAEAAAAAAABSR0JDAAAAAwAAAABSZCAgZG91YkBv4AAAAAAAAAAAAEdybiBkb3ViQG/gAAAAAAAAAAAAQmwgIGRvdWJAb+AAAAAAAAAAAABCcmRUVW50RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAABmJvdW5kc09iamMAAAABAAAAAAAAUmN0MQAAAAQAAAAAVG9wIGxvbmcAAAAAAAAAAExlZnRsb25nAAAAAAAAAABCdG9tbG9uZwAAAGYAAAAAUmdodGxvbmcAAABbAAAABnNsaWNlc1ZsTHMAAAABT2JqYwAAAAEAAAAAAAVzbGljZQAAABIAAAAHc2xpY2VJRGxvbmcAAAAAAAAAB2dyb3VwSURsb25nAAAAAAAAAAZvcmlnaW5lbnVtAAAADEVTbGljZU9yaWdpbgAAAA1hdXRvR2VuZXJhdGVkAAAAAFR5cGVlbnVtAAAACkVTbGljZVR5cGUAAAAASW1nIAAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAABmAAAAAFJnaHRsb25nAAAAWwAAAAN1cmxURVhUAAAAAQAAAAAAAG51bGxURVhUAAAAAQAAAAAAAE1zZ2VURVhUAAAAAQAAAAAABmFsdFRhZ1RFWFQAAAABAAAAAAAOY2VsbFRleHRJc0hUTUxib29sAQAAAAhjZWxsVGV4dFRFWFQAAAABAAAAAAAJaG9yekFsaWduZW51bQAAAA9FU2xpY2VIb3J6QWxpZ24AAAAHZGVmYXVsdAAAAAl2ZXJ0QWxpZ25lbnVtAAAAD0VTbGljZVZlcnRBbGlnbgAAAAdkZWZhdWx0AAAAC2JnQ29sb3JUeXBlZW51bQAAABFFU2xpY2VCR0NvbG9yVHlwZQAAAABOb25lAAAACXRvcE91dHNldGxvbmcAAAAAAAAACmxlZnRPdXRzZXRsb25nAAAAAAAAAAxib3R0b21PdXRzZXRsb25nAAAAAAAAAAtyaWdodE91dHNldGxvbmcAAAAAADhCSU0EKAAAAAAADAAAAAI/8AAAAAAAADhCSU0EFAAAAAAABAAAAAU4QklNBAwAAAAAA7IAAAABAAAAFQAAABgAAABAAAAGAAAAA5YAGAAB/9j/7QAMQWRvYmVfQ00AAf/uAA5BZG9iZQBkgAAAAAH/2wCEAAwICAgJCAwJCQwRCwoLERUPDAwPFRgTExUTExgRDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBDQsLDQ4NEA4OEBQODg4UFA4ODg4UEQwMDAwMEREMDAwMDAwRDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIABgAFQMBIgACEQEDEQH/3QAEAAL/xAE/AAABBQEBAQEBAQAAAAAAAAADAAECBAUGBwgJCgsBAAEFAQEBAQEBAAAAAAAAAAEAAgMEBQYHCAkKCxAAAQQBAwIEAgUHBggFAwwzAQACEQMEIRIxBUFRYRMicYEyBhSRobFCIyQVUsFiMzRygtFDByWSU/Dh8WNzNRaisoMmRJNUZEXCo3Q2F9JV4mXys4TD03Xj80YnlKSFtJXE1OT0pbXF1eX1VmZ2hpamtsbW5vY3R1dnd4eXp7fH1+f3EQACAgECBAQDBAUGBwcGBTUBAAIRAyExEgRBUWFxIhMFMoGRFKGxQiPBUtHwMyRi4XKCkkNTFWNzNPElBhaisoMHJjXC0kSTVKMXZEVVNnRl4vKzhMPTdePzRpSkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2JzdHV2d3h5ent8f/2gAMAwEAAhEDEQA/AO16j1/H6fkn7bkmi1tmz0msc9jan73VWOaxn6ay1tW/+c/Qf9ufaK1f1rwHkMxsm6y17N1jRVZa4QR6z66xTtr9Kv3Nf/Mf6WpZ31morzOo7LSWHIa19G2ADY37XXQyxzvosupo/wC3FRFPTj0Gyixt/T35A3WZQLZsNcluH6N9mPbkV37n101U+x9n6SxQSnISoVTrYOWwSwiUhLjPCPTw8I4uvy8cnrB1bG+xusOU8dPD21NyNrvVJLXmykmN/t/R/re3f/b/AFtJZIaP+bbiWwRc25rZ5eb34LmTP5rmssSUnEfwtqe0L6/znB0/d/6T/9DtOt9Buym1+lDjUXem8aODS4XtqdH0XU217sTKb/M/zduNbV6+/Dr+qWZZki+42W2NO4PsPqEFv82/Y9lLbXfuV3WUV7/5yp9S8SSUGT2+IW6vKfevYl7fDw0f739Z+kx0WOmuwtlZrdUKPQkwKwHcX7f6S61/rfaPQ/6z/hkl82JKX0/g0f1v/P8AH5/5f4b/AP/ZOEJJTQQhAAAAAABXAAAAAQEAAAAPAEEAZABvAGIAZQAgAFAAaABvAHQAbwBzAGgAbwBwAAAAFABBAGQAbwBiAGUAIABQAGgAbwB0AG8AcwBoAG8AcAAgADIAMAAyADEAAAABADhCSU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg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""
                                                    alt="""">
                                            </div>
                                            <div>
                                                <h1>Informe consumo de servicio de cotejos biométricos en línea Fijos y Móviles</h1>
                                            </div>
                                            <div class=""logo"">
                                                <p>Elaborado por:</p>
                                                <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                                                    alt="""">";

            htmlCode += String.Format(@"<div class=""text-doc"">
                                                        <p>Fecha de elaboración:</p>
                                                        <p>{0}</p>
                                            </div>", String.Format("{0:D}", DateTime.Now));

            htmlCode += @" </div>
                                        </div>
                                    </div>
                                    <div class=""wrapper-print body-flex"">
                                        <div class=""tabla-uno-contenedor"">
                                            <table cellspacing = ""0"" cellpadding=""0"" class=""tabla tabla-mes"">";
            htmlCode += String.Format(@"<h4 class=""titulo-tabla"">Reporte de Cotejos Fijos <span>{0}</span> </h4>
                                                <thead>", String.Format("{0:y}", Listar_cotejos_Total[0].Fecha));
            if (Listar_cotejos_fijos.Count > 0)
            {
                htmlCode += @"<tr>
                                                <th rowspan = ""2"">#</th>
                                                <th rowspan= ""2""> FECHA </th>
                                                <th colspan= ""3""> COTEJOS </th>
                                            </tr>
                                            <tr>
                                                <th> HIT </th>
                                                <th> NO HIT</th>
                                                <th>TOTAL</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
                int i = 1;
                int j = 0;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (NotariaGrafica.Total item in Listar_cotejos_fijos)
                {

                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td>{1}</td>
                                                <td class=""numeros"">{2}</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>", i, item.Dias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));
                    htmlCode += @"</tr>";
                    i++;
                    j++;
                }
                htmlCode += String.Format(@"</tbody>
                                        <tfoot>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio total</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>
                                                <td class=""numeros"">{5}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio día hábil</td>", decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL), decimales(totalHIT / j), decimales(totalNOHIT / j), decimales(totaTOTAL / j));
                htmlCode += String.Format(@"<td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>", decimales(listar_cotejos_Total_fijos.HIT), decimales(listar_cotejos_Total_fijos.NOHIT), decimales(listar_cotejos_Total_fijos.TOTAL));
                htmlCode += @"</tr>
                                        </tfoot>
                                    </table>
                                </div>
                                <div class=""grafica-contenedor"">";
            }

            htmlCode += String.Format(@"<img src = ""{0}"" alt="""">", imageToBase64(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "cotejos_fijos_p2 " + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".jpg"));
            if (listtotal.Count > 0)
            {
                htmlCode += @"</div>
                                <div class=""tabla-dos-contenedor"">
                                    <table class=""tabla tabla-anios"">
                                        <thead>
                                            <tr>
                                                <th>AÑO</th>
                                                <th>TOTAL COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";

                Totalesanios totalesanios = new Totalesanios
                {
                    Producto = "Fijo",
                    Total = Listar_cotejos_fijos_mes.Sum(item => item.Total),
                    Anio = 2021
                };
                listtotal.Add(totalesanios);
                foreach (Totalesanios item in listtotal.Where(x => x.Producto == "Fijo"))
                {


                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros total"">{1}</td>
                                            </tr>", decimales(item.Anio), decimales(item.Total));
                }
            }

            htmlCode += @"</tbody>
                                    </table>
    
                                    <table cellspacing = ""0"" cellpadding=""0"" class=""tabla"">
                                        <thead>
                                            <tr>
                                                <th>2.021</th>
                                                <th>COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
            if (Listar_cotejos_fijos_mes.Count > 0)
            {
                int i = 1;
                int j = 0;
                int Total = 0;
                int Promedio = 0;
                foreach (NotariaGrafica.MesTotal item in Listar_cotejos_fijos_mes)
                {
                    htmlCode += String.Format(@"<tr>
                                                <td>{0}</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", item.Mes, decimales(item.Total));

                    Total += item.Total;
                    Promedio += item.Total;
                    i++;
                    j++;
                }

                htmlCode += String.Format(@" </tbody>
                                        <tfoot>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                            </tr>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Promedio</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", decimales(Total), decimales(Promedio / j));
            }

            htmlCode += @"</tfoot>
                                            </table>
                                        </div>
                                    </div>
                                </div>";


            //tercera pagina
            htmlCode += @"<div class=""pagina"">
                                    <div>
                                        <div class=""flex-container header"">
                                            <div class=""logo-ucnc"">
                                                <img src = ""data:image/jpeg;base64,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RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAA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SU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPD94cGFja2V0IGVuZD0idyI/Pv/iDFhJQ0NfUFJPRklMRQABAQAADEhMaW5vAhAAAG1udHJSR0IgWFlaIAfOAAIACQAGADEAAGFjc3BNU0ZUAAAAAElFQyBzUkdCAAAAAAAAAAAAAAAAAAD21gABAAAAANMtSFAgIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEWNwcnQAAAFQAAAAM2Rlc2MAAAGEAAAAbHd0cHQAAAHwAAAAFGJrcHQAAAIEAAAAFHJYWVoAAAIYAAAAFGdYWVoAAAIsAAAAFGJYWVoAAAJAAAAAFGRtbmQAAAJUAAAAcGRtZGQAAALEAAAAiHZ1ZWQAAANMAAAAhnZpZXcAAAPUAAAAJGx1bWkAAAP4AAAAFG1lYXMAAAQMAAAAJHRlY2gAAAQwAAAADHJUUkMAAAQ8AAAIDGdUUkMAAAQ8AAAIDGJUUkMAAAQ8AAAIDHRleHQAAAAAQ29weXJpZ2h0IChjKSAxOTk4IEhld2xldHQtUGFja2FyZCBDb21wYW55AABkZXNjAAAAAAAAABJzUkdCIElFQzYxOTY2LTIuMQAAAAAAAAAAAAAAEnNSR0IgSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABYWVogAAAAAAAA81EAAQAAAAEWzFhZWiAAAAAAAAAAAAAAAAAAAAAAWFlaIAAAAAAAAG+iAAA49QAAA5BYWVogAAAAAAAAYpkAALeFAAAY2lhZWiAAAAAAAAAkoAAAD4QAALbPZGVzYwAAAAAAAAAWSUVDIGh0dHA6Ly93d3cuaWVjLmNoAAAAAAAAAAAAAAAWSUVDIGh0dHA6Ly93d3cuaWVjLmNoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGRlc2MAAAAAAAAALklFQyA2MTk2Ni0yLjEgRGVmYXVsdCBSR0IgY29sb3VyIHNwYWNlIC0gc1JHQgAAAAAAAAAAAAAALklFQyA2MTk2Ni0yLjEgRGVmYXVsdCBSR0IgY29sb3VyIHNwYWNlIC0gc1JHQgAAAAAAAAAAAAAAAAAAAAAAAAAAAABkZXNjAAAAAAAAACxSZWZlcmVuY2UgVmlld2luZyBDb25kaXRpb24gaW4gSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAAsUmVmZXJlbmNlIFZpZXdpbmcgQ29uZGl0aW9uIGluIElFQzYxOTY2LTIuMQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdmlldwAAAAAAE6T+ABRfLgAQzxQAA+3MAAQTCwADXJ4AAAABWFlaIAAAAAAATAlWAFAAAABXH+dtZWFzAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAACjwAAAAJzaWcgAAAAAENSVCBjdXJ2AAAAAAAABAAAAAAFAAoADwAUABkAHgAjACgALQAyADcAOwBAAEUASgBPAFQAWQBeAGMAaABtAHIAdwB8AIEAhgCLAJAAlQCaAJ8ApACpAK4AsgC3ALwAwQDGAMsA0ADVANsA4ADlAOsA8AD2APsBAQEHAQ0BEwEZAR8BJQErATIBOAE+AUUBTAFSAVkBYAFnAW4BdQF8AYMBiwGSAZoBoQGpAbEBuQHBAckB0QHZAeEB6QHyAfoCAwIMAhQCHQImAi8COAJBAksCVAJdAmcCcQJ6AoQCjgKYAqICrAK2AsECywLVAuAC6wL1AwADCwMWAyEDLQM4A0MDTwNaA2YDcgN+A4oDlgOiA64DugPHA9MD4APsA/kEBgQTBCAELQQ7BEgEVQRjBHEEfgSMBJoEqAS2BMQE0wThBPAE/gUNBRwFKwU6BUkFWAVnBXcFhgWWBaYFtQXFBdUF5QX2BgYGFgYnBjcGSAZZBmoGewaMBp0GrwbABtEG4wb1BwcHGQcrBz0HTwdhB3QHhgeZB6wHvwfSB+UH+AgLCB8IMghGCFoIbgiCCJYIqgi+CNII5wj7CRAJJQk6CU8JZAl5CY8JpAm6Cc8J5Qn7ChEKJwo9ClQKagqBCpgKrgrFCtwK8wsLCyILOQtRC2kLgAuYC7ALyAvhC/kMEgwqDEMMXAx1DI4MpwzADNkM8w0NDSYNQA1aDXQNjg2pDcMN3g34DhMOLg5JDmQOfw6bDrYO0g7uDwkPJQ9BD14Peg+WD7MPzw/sEAkQJhBDEGEQfhCbELkQ1xD1ERMRMRFPEW0RjBGqEckR6BIHEiYSRRJkEoQSoxLDEuMTAxMjE0MTYxODE6QTxRPlFAYUJxRJFGoUixStFM4U8BUSFTQVVhV4FZsVvRXgFgMWJhZJFmwWjxayFtYW+hcdF0EXZReJF64X0hf3GBsYQBhlGIoYrxjVGPoZIBlFGWsZkRm3Gd0aBBoqGlEadxqeGsUa7BsUGzsbYxuKG7Ib2hwCHCocUhx7HKMczBz1HR4dRx1wHZkdwx3sHhYeQB5qHpQevh7pHxMfPh9pH5Qfvx/qIBUgQSBsIJggxCDwIRwhSCF1IaEhziH7IiciVSKCIq8i3SMKIzgjZiOUI8Ij8CQfJE0kfCSrJNolCSU4JWgllyXHJfcmJyZXJocmtyboJxgnSSd6J6sn3CgNKD8ocSiiKNQpBik4KWspnSnQKgIqNSpoKpsqzysCKzYraSudK9EsBSw5LG4soizXLQwtQS12Last4S4WLkwugi63Lu4vJC9aL5Evxy/+MDUwbDCkMNsxEjFKMYIxujHyMioyYzKbMtQzDTNGM38zuDPxNCs0ZTSeNNg1EzVNNYc1wjX9Njc2cjauNuk3JDdgN5w31zgUOFA4jDjIOQU5Qjl/Obw5+To2OnQ6sjrvOy07azuqO+g8JzxlPKQ84z0iPWE9oT3gPiA+YD6gPuA/IT9hP6I/4kAjQGRApkDnQSlBakGsQe5CMEJyQrVC90M6Q31DwEQDREdEikTORRJFVUWaRd5GIkZnRqtG8Ec1R3tHwEgFSEtIkUjXSR1JY0mpSfBKN0p9SsRLDEtTS5pL4kwqTHJMuk0CTUpNk03cTiVObk63TwBPSU+TT91QJ1BxULtRBlFQUZtR5lIxUnxSx1MTU19TqlP2VEJUj1TbVShVdVXCVg9WXFapVvdXRFeSV+BYL1h9WMtZGllpWbhaB1pWWqZa9VtFW5Vb5Vw1XIZc1l0nXXhdyV4aXmxevV8PX2Ffs2AFYFdgqmD8YU9homH1YklinGLwY0Njl2PrZEBklGTpZT1lkmXnZj1mkmboZz1nk2fpaD9olmjsaUNpmmnxakhqn2r3a09rp2v/bFdsr20IbWBtuW4SbmtuxG8eb3hv0XArcIZw4HE6cZVx8HJLcqZzAXNdc7h0FHRwdMx1KHWFdeF2Pnabdvh3VnezeBF4bnjMeSp5iXnnekZ6pXsEe2N7wnwhfIF84X1BfaF+AX5ifsJ/I3+Ef+WAR4CogQqBa4HNgjCCkoL0g1eDuoQdhICE44VHhauGDoZyhteHO4efiASIaYjOiTOJmYn+imSKyoswi5aL/IxjjMqNMY2Yjf+OZo7OjzaPnpAGkG6Q1pE/kaiSEZJ6kuOTTZO2lCCUipT0lV+VyZY0lp+XCpd1l+CYTJi4mSSZkJn8mmia1ZtCm6+cHJyJnPedZJ3SnkCerp8dn4uf+qBpoNihR6G2oiailqMGo3aj5qRWpMelOKWpphqmi6b9p26n4KhSqMSpN6mpqhyqj6sCq3Wr6axcrNCtRK24ri2uoa8Wr4uwALB1sOqxYLHWskuywrM4s660JbSctRO1irYBtnm28Ldot+C4WbjRuUq5wro7urW7LrunvCG8m70VvY++Cr6Evv+/er/1wHDA7MFnwePCX8Lbw1jD1MRRxM7FS8XIxkbGw8dBx7/IPci8yTrJuco4yrfLNsu2zDXMtc01zbXONs62zzfPuNA50LrRPNG+0j/SwdNE08bUSdTL1U7V0dZV1tjXXNfg2GTY6Nls2fHadtr724DcBdyK3RDdlt4c3qLfKd+v4DbgveFE4cziU+Lb42Pj6+Rz5PzlhOYN5pbnH+ep6DLovOlG6dDqW+rl63Dr++yG7RHtnO4o7rTvQO/M8Fjw5fFy8f/yjPMZ86f0NPTC9VD13vZt9vv3ivgZ+Kj5OPnH+lf65/t3/Af8mP0p/br+S/7c/23////uAA5BZG9iZQBkgAAAAAH/2wCEAAwICAgJCAwJCQwRCwoLERUPDAwPFRgTExUTExgRDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBDQsLDQ4NEA4OEBQODg4UFA4ODg4UEQwMDAwMEREMDAwMDAwRDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIAGYAWwMBIgACEQEDEQH/3QAEAAb/xAE/AAABBQEBAQEBAQAAAAAAAAADAAECBAUGBwgJCgsBAAEFAQEBAQEBAAAAAAAAAAEAAgMEBQYHCAkKCxAAAQQBAwIEAgUHBggFAwwzAQACEQMEIRIxBUFRYRMicYEyBhSRobFCIyQVUsFiMzRygtFDByWSU/Dh8WNzNRaisoMmRJNUZEXCo3Q2F9JV4mXys4TD03Xj80YnlKSFtJXE1OT0pbXF1eX1VmZ2hpamtsbW5vY3R1dnd4eXp7fH1+f3EQACAgECBAQDBAUGBwcGBTUBAAIRAyExEgRBUWFxIhMFMoGRFKGxQiPBUtHwMyRi4XKCkkNTFWNzNPElBhaisoMHJjXC0kSTVKMXZEVVNnRl4vKzhMPTdePzRpSkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2JzdHV2d3h5ent8f/2gAMAwEAAhEDEQA/APVUkDMzcXBx35OXa2mln0nuMD4D9538lY4r6j9YNbxZgdIPFP0cjIH/AA352Nju/wBH/O2IE/avjC9SeGI/S/7396Tbp+sXTL+ojAqs3PJLW2gfonWN1fRXd9B9zW+7YtRZ2b03pH7OGFe1mPiNgVbSK9jh/Nvpf+Za130VVpu6/hk42yvqzKwNtosbTcAfofaK3/o3O/4RiFkb/gv9uMxcDw10yGuL+sJ/J/gu2srK6/VVe6nEpszjRrlGgAipvmf8Jb/wLPegXVdTywP2pkM6divMfZqHzY/+RZlHb/m0MWrhUYmPjtqw2sZQ36IZEfh9JKydvT+f2JEceMXP9af3Yn9WP72T9L/qasTMxs2huRi2C2p3Dm+P7p/dd/JR1jZPT8R9js/pmU3Cyt22x7CHVPd/o8mj6Dn/APgqc5v1jqaa39PpueOL2XhlZ/lOZY31WJCXcfUaolhB1hIf3chGOcf8b0TbvUupY/TcU5N8kAhrGNEve5xhtVTPz7Hfup+n9Sw+o0eviP3tna9p0cxw+lXaw+6uxqz+nYbcnMb1DqGTXmZjAfQqqP6GkHR3oMPusf8AvZDkXqHRTZf+0Om2fY+oga2ATXaB/g8uofzjf+E/na0rO/Tt1UYYx6CTxfv/AKH93h+b/DdVJZXTeti+/wCwdQq+xdSaJNDjLbAP8LiW/wCGr/8ABGfnrVRsVbH7cuLhrX8PPif/0O7+sfS7cllHUMUB+Z05xupqfrXZp+kqfWfbvez+Zt+nVYtLAzqM/CozKD+jyGh7Z5E8tP8AKajnhcJ1acfpmbhV3OxRh9U3U2MkljbR9qY1jWln51j/AM5NJ4dWbFH3QIE1wnQ/1Zb/APOek+tOPZk9HfXS31Htsqfs0khj2Wv2z+dsajU5tX2o2NY4syS1rXwBt2NLt9u7a5jPzFwOW57/AKr2PsufkOd1IE3WSHOmnuNz9qxG/P71FLLRuugdDDyHHj4TP5ZSHy/vcP8AW/qvqF4dX9YWZtv6XE+zmqtzfd6dm7c6WD3N9Vv+EWhi5OPusprqNDK4cC4BrHb5cfTh3+evJG/E/eUTtyfvKZ94o/L+LY/0MJgA5thw/J2/w30B+DZTmNzsGPTybYz8c6SGvLqsutp/wrf+mxWutev1LoF7MOaci9ntqsIY+J99Tvd7HWM9q8ydPifvKv8ASvq51bq7TZit20gwbrHFrZ/k/nP2pRy3YEd+gKM3ICHDknmA9sipSh2+WMvX6n0OrLxGV0PrxLN7Q2oNbWA6sO2tc07tvsZ/hNi0pETOi80z/q5Z0PqHTDbmC+y/IYBW0OEBrmy7V59uqPSbT9YtpybfRb1Mu9D1G+lPqfR9Dd6+78//AEKlGQ7EU0MnKQIEoZOKNGXFR112ek6nS3rvWGdNGmL0xzL8m5ujzafdRjVWD3V+z9LkbP8Ai10ELG+qjd/Trc1385nZF17j4gvNdf8Am11sW0n9L+rXIByDH0j6B/f/AHv8d//R9VXD9aYLrOpkjc13Ucdm0guBLKG7muawtdt/e9y7dxAEnQdyuF6g8O+rZz3uYxmf1F1+6wbmCtznVV+oyHb2elW32Jk9mxyvzX3MY/jxf9w5vUN3/Ny/eZd+0hMN2D+ZH0a/zGrBatzLdVZ9WbDRLq3dSAr0AJHogDbXX9H/AItUP2N1NjWusp9L1PoNscxjnR+7XY5rlWmCTp2dvlZRjE8REfUd/S12onZGbgbIGVkVY7zxWZsd/bFO9ta2ui9KwrMh9F7BlvpYH3BhJBc7+bx8fbsb/LuusUfASabkuYhjgZG5AC/SP+6+Vp/Vzog6tlu9fcMOkTaW8ucfoUsP8pdT1j6zdO6HScDGZuyambaqK42V/u+q/wDe/O2q5jXW1WUdPxMB+OzcTkW7QyprANfSdudve/8AMXG/W/6vWdLyjlU7n4eQ4kPJLix51dXY4/vf4NysUccPTqf0pOSckeb5kDMeGFXixXfF/f4f0pOVRl5OZ1rFyMqx11z8isuc4/y26NH5rVs0WB/1jfT6tbCOou/RNBNrybDtdY8+1tNc+xrFgdO/5Tw/+Pr/AOqauoxyB1k/zknqTo+j6el3/bibj/av5qgaA/QoPTfVI/5AxW/ub2H4tse0rYWL9Wz6J6h086OxMuwtH8i79ZqP/gi2lY/R+lOV/l7/AK3H/g/zj//S7/6y5VlPTHY9B/Wc5zcSj+tb7HP/AOt1epYs363YdWP9X8XDrq9WumypjKt2wHaCG77PzGfvq7mD7T9asCg6sw8e3KI/luLcat39lpsQ/roD+y63Bjrdt7P0bQCXTuaGbXstY7d/KrTJbS+xsYTU8QHU8f7IvM4OSzpvRxZs9Ng6kGPbO7YH07X+nZ/I3eyxW87EZl476HtFljCX0yYlwH73/CN/1/QrKzG2O+rWS19TqX159b30v2y1r6tjP5prK/8AoK30bMOTgsLnfpsb9G8nwHure7+z/wC7Shv9HwdIRNe6NxKj/wBy4jcx7TFbG444IY33DsW73+5dHhZt+Q6urExzg+kd2FvcQLrY/SVZZ9vquyG/Qf8AmWLO6wx2Lc3Ix2NY3IkudEuFn+EbJVXDxs3Mf6zXlrazP2h5MNI19n7zv6qisxlW7fMYZcQmagKOsvV6v60f0nrHfXjCx7KWPrtc125uQ1wAsqc07fc38/ert/W+ldXx/sGDYzKuy/Z6TmztbzZdax4/wTf/AARYGViY+Rk15F1e+yyvc69zSKvboXOr/wAJY9+1mN+Z/wBto+DndH6Rc+5zmV5bWGBtraXAe59MUta73fmb1PGctRIjhczJy2CoyxQn7oF0DceP/wBBR/WDD+r/AE7P6TgY1JblstrJsrIkM3Db68/zjrXqrQf8vkAgAdTJcBsEn1fa2zX7Q93+j9voqgWZfUuvY/UQ4ZDMnKrO9kn0/cNtNrD76tjG7fcr2G2uzr/rsrYXW9Qdtu2jcW+r9Df6u/dt+j+r+nsQBs6CtdEzjwQAlIzlwHiJ19Zeqyv1D6x4+TxT1Rn2W09hdVNuK4/8ZX61S2pWR9bKyeh33s/ncMsyqj4Opc23/qQ5Xvt1P/gP2j+yp+pH1cwk8MZddYH+X92T/9Pux7friZ/wnT/Z/Zt93/VKz9YMN+b0fJorkW7N9ZHO5nvbH3Kp9YA7DyMPrTAXNwXOZlAan7Pd7bbP+svay1bTHssY2ytwcx4DmuGoIPBCHcMhJHBMdP8ApQfOemVsyq8np5aK6ep1bMZxc47rqh61Dv0/6Rzt3q12bGenvWV0fJdh52y72NefRuafzTOhP/F2Lf690cdN6o57Iqpy3iyjIEb63NPqPxaHPiupz7P0jbLH7PQVTqOA3rQf1DAAOaJ+14zRHq7Ts+24nHqsfHv2qtIH6xdnBkgQbP6vKB6v3J/L6v5f5N0XV0urNeTUH0g7nb3Db7dB/ab+d6j6f7aq5fXMJjRXjsDwzRra9GCP+FcP/PFX/X1z9uRk3kNyLHvLIG15Oke36KXZRyyHo3cPJx3mTL+qNIt1/W8z3bW1tJO5pAMtPZ2rve9v5r7d6ynd/wAUVysdO6Rl9SeTUBXjs1uyn6VsHm78538hAXI918xjxAnSA6ltfVquzHdldX12YlZZU0GN99nsor/lbP5xX/qXh2ZPXBZfjCq3p7JyLDul1hBrqGx3srd7nv8AYo35OI3GpxsSpzsGqW4hI3DJtsGx9j2t3NdfY76GPks/mf8ARrsPq10h3S+nNbdBy7yLMgjgOja2pv8AwdLP0bFYxx1A7auPzeaozkRUsnoiP0uEfy/x2X1ne1n1e6iXcfZ7B8y3aED7Ld4H/k30v7aXX3DPyMboVXude9t2ZH5mPU4Pdv8A/DFjWU1raU3UuaRUIjqSZf4L/9T1N7A8bXag6EHUEHlrliM6d1To7yOkFuTgEz+z7nbTXPIw8j3ba/8AgbVupIEWujMxvYg7xOziuy8HrLX9I6ni2Yt1rdwovA923/CY1zC9j3VfyVy/Uvq/1TpnUKMqx77cLGAbXk0yHta2XNZds91brHfzl382uz6t0wZ9LQx5pyaXi3GyAJNdje8fnVv+haxVmj61WgMe7DxtvNrQ+0u/q1u9PZ/nJk43vfgR+1t8vn9vWBiImxPHM/Lf6WP5nl7M3EzRW7Ox6c42B7he2arAytpc5z7a/wCo/Z6le9Vcev6vZW/08bLaWN3lgtYdPztu5u521dFbT01lxq+sGJXjXH+bz6Q5lNoP7z2fzNv79dykz6v/AFUn1K8oNJM7m5A5+O5RGBJ/RPe/mb8OaxwjtmjY9Bxkyxf4PBLg/wCa4Laek0kel07fYJg5VpsG5rPtPp+jV6bd3pe/6aVWTmdTApaHZBLGvx6cVorbRZofez+ZZtn1KsixbluH9T8MN3EZlzdKqGvdfYT9ENZXW7+z7lbxqPrCKhbjMxMGvlmAWEwO3rXVOb+l/qMREDtp5QY8nMxI4uGV/oy5gyj6v6n85Nq9N6HhdDrs611d1YyGy6GA+nUXfm0M/Puse7/0krruoddzm7enYX2Jjv8AtVmkAgfvMxKy6xzv+NfWkzp/U87Lpyer+k2rFO+jEpJc028NyL3vDd3pf4Ji2QFLEaaaD8XPy5LlcqyT/wDG4fuxi0Ol9Ip6eyw73ZGVkEPycqzV9jh4/uVs/wAHUz6C0Ekk6hVMPHLi4r9T/9X1VJfKqSSn6qSXyqkkp+p7fS2O9Xb6f526NsfytyzHf81ZO77BPefSXzWkmy+n+Ez4Ov8AOf8AUv2v09hfsmT+z/s89/Q2T/4ErYXyskiNun0Y8nzH5v8Aqnzv1UkvlVJFY/VSS+VUklP/2Q==""
                                                    alt="""">
                                            </div>
                                            <div>
                                                <h1>Informe consumo de servicio de cotejos biométricos en línea Fijos y Móviles</h1>
                                            </div>
                                            <div class=""logo"">
                                                <p>Elaborado por:</p>
                                                <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                                                    alt="""">";

            htmlCode += String.Format(@"<div class=""text-doc"">
                                                        <p>Fecha de elaboración:</p>
                                                        <p>{0}</p>
                                            </div>", String.Format("{0:D}", DateTime.Now));

            htmlCode += @" </div>
                                        </div>
                                    </div>
                                    <div class=""wrapper-print body-flex"">
                                        <div class=""tabla-uno-contenedor"">
                                            <table cellspacing = ""0"" cellpadding=""0"" class=""tabla tabla-mes"">";
            htmlCode += String.Format(@"<h4 class=""titulo-tabla"">Reporte de Cotejos Móvil <span>{0}</span> </h4>
                                                <thead>", String.Format("{0:y}", Listar_cotejos_Total[0].Fecha));
            if (Listar_cotejos_movil.Count > 0)
            {
                htmlCode += @"<tr>
                                                <th rowspan = ""2"">#</th>
                                                <th rowspan= ""2""> FECHA </th>
                                                <th colspan= ""3""> COTEJOS </th>
                                            </tr>
                                            <tr>
                                                <th> HIT </th>
                                                <th> NO HIT</th>
                                                <th>TOTAL</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
                int i = 1;
                int j = 0;
                int totalHIT = 0;
                int totalNOHIT = 0;
                int totaTOTAL = 0;
                foreach (NotariaGrafica.Total item in Listar_cotejos_movil)
                {

                    totalHIT += item.HIT;
                    totalNOHIT += item.NOHIT;
                    totaTOTAL += item.TOTAL;
                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td>{1}</td>
                                                <td class=""numeros"">{2}</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>", i, item.Dias, decimales(item.HIT), decimales(item.NOHIT), decimales(item.TOTAL));
                    htmlCode += @"</tr>";
                    i++;
                    j++;
                }
                htmlCode += String.Format(@"</tbody>
                                        <tfoot>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio total</td>
                                                <td class=""numeros"">{3}</td>
                                                <td class=""numeros"">{4}</td>
                                                <td class=""numeros"">{5}</td>
                                            </tr>
                                            <tr>
                                                <td colspan = ""2"" class=""subtitulo-tabla""> Promedio día hábil</td>", decimales(totalHIT), decimales(totalNOHIT), decimales(totaTOTAL), decimales(totalHIT / j), decimales(totalNOHIT / j), decimales(totaTOTAL / j));
                htmlCode += String.Format(@"<td class=""numeros"">{0}</td>
                                                <td class=""numeros"">{1}</td>
                                                <td class=""numeros"">{2}</td>", listar_cotejos_Total_movil.HIT, listar_cotejos_Total_movil.NOHIT, listar_cotejos_Total_movil.TOTAL);
                htmlCode += @"</tr>
                                        </tfoot>
                                    </table>
                                </div>
                                <div class=""grafica-contenedor"">";
            }

            htmlCode += String.Format(@"<img src = ""{0}"" alt="""">", imageToBase64(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "cotejos_movil_p3 " + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".jpg"));
            if (listtotal.Count > 0)
            {
                htmlCode += @"</div>
                                <div class=""tabla-dos-contenedor"">
                                    <table class=""tabla tabla-anios"">
                                        <thead>
                                            <tr>
                                                <th>AÑO</th>
                                                <th>TOTAL COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";

                Totalesanios totalesanios = new Totalesanios
                {
                    Producto = "Movil",
                    Total = Listar_cotejos_movil_mes.Sum(item => item.Total),
                    Anio = 2021
                };
                listtotal.Add(totalesanios);
                foreach (Totalesanios item in listtotal.Where(x => x.Producto == "Movil"))
                {


                    htmlCode += String.Format(@"<tr>
                                                <td class=""numeros"">{0}</td>
                                                <td class=""numeros total"">{1}</td>
                                            </tr>", decimales(item.Anio), decimales(item.Total));
                }
            }

            htmlCode += @"
                                        </tbody>
                                    </table>
    
                                    <table cellspacing = ""0"" cellpadding=""0"" class=""tabla"">
                                        <thead>
                                            <tr>
                                                <th>2.021</th>
                                                <th>COTEJOS</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
            if (Listar_cotejos_movil_mes.Count > 0)
            {
                int i = 1;
                int j = 0;
                int Total = 0;
                int Promedio = 0;
                foreach (NotariaGrafica.MesTotal item in Listar_cotejos_movil_mes)
                {
                    htmlCode += String.Format(@"<tr>
                                                <td>{0}</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", item.Mes, decimales(item.Total));

                    Total += item.Total;
                    Promedio += item.Total;
                    i++;
                    j++;
                }

                htmlCode += String.Format(@" </tbody>
                                        <tfoot>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Total</td>
                                                <td class=""numeros"">{0}</td>
                                            </tr>
                                            <tr>
                                                <td class=""subtitulo-tabla""> Promedio</td>
                                                <td class=""numeros"">{1}</td>
                                            </tr>", decimales(Total), decimales(Promedio / j));
            }

            htmlCode += @"</tfoot>
                                            </table>
                                        </div>
                                    </div>
                                </div>";


            //cuarto reporte

            htmlCode += @"<div class=""pagina"">
        <div>
            <div class=""flex-container header"">
                <div class=""logo-ucnc"">
                    <img src = ""data:image/jpeg;base64,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RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAA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SU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg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""
                        alt="""">
                </div>
                <div>
                    <h1>Informe consumo de servicio de cotejos biométricos en línea Fijos y Móviles</h1>
                </div>
                <div class=""logo"">
                    <p>Elaborado por:</p>
                    <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                        alt="""">";
            htmlCode += String.Format(@"<div class=""text-doc"">
                                                        <p>Fecha de elaboración:</p>
                                                        <p>{0}</p>
                                            </div>", String.Format("{0:D}", DateTime.Now));

            htmlCode += @" </div>
                                        </div>
                                    </div>
                                    <div class=""wrapper-print body-flex"">
                                        <div class=""tabla-uno-contenedor"">
                                            <table cellspacing = ""0"" cellpadding=""0"" class=""tabla tabla-mes"">";
            htmlCode += String.Format(@"<h4 class=""titulo-tabla"">Notarías <span>{0}</span> </h4>
                                                <thead>", String.Format("{0:y}", Listar_cotejos_Total[0].Fecha));
            htmlCode += @" <tr>
                            <th rowspan = ""2"" >#</th>
                            <th rowspan=""2"">FECHA</th>
                            <th colspan = ""2""> COTEJOS EN NOTARIAS FIJO </th>
                            <th class=""total"" rowspan=""2"">TOTAL</th>
                        </tr>
                        <tr>
                            <th>MAYORES A 30</th>
                            <th>MENORES A 30</th>
                        </tr>
                    </thead>
                    <tbody>";
            if (fijos.Count > 0)
            {
                int i = 1;
                int j = 0;
                int promedioMayores = 0;
                int promedioMenores = 0;
                int promediototal = 0;
                foreach (var item in fijos)
                {
                    promedioMayores += item.MayoresA30;
                    promedioMenores += item.MenoresA30;
                    promediototal += item.Total;

                    htmlCode += String.Format(@"<tr>
                            <td class=""numeros"">{0}</td>
                            <td>{1}</td>
                            <td class=""numeros"">{2}</td>
                            <td class=""numeros"">{3}</td>
                            <td class=""numeros total"">{4}</td>
                        </tr>", i, item.Dias, decimales(item.MayoresA30), decimales(item.MenoresA30), decimales(item.Total));
                    i++;
                    j++;
                }
                htmlCode += String.Format(@" </tbody>
                    <tfoot>
                        <tr>
                            <td colspan = ""2"" class=""subtitulo-tabla"">Promedio</td>
                            <td class=""numeros"">{0}</td>
                            <td class=""numeros"">{1}</td>
                            <td class=""numeros"">{2}</td>
                        </tr>
                        <tr>
                            <td colspan = ""2"" class=""subtitulo-tabla"">Máximo</td>
                            <td class=""numeros"">{3}</td>
                            <td class=""numeros"">{4}</td>
                            <td class=""numeros"">{5}</td>
                        </tr>
                    </tfoot>", decimales(promedioMayores / j), decimales(promedioMenores / j), decimales(promediototal / j), decimales(fijos.Max(x => x.MayoresA30)), decimales(fijos.Max(x => x.MenoresA30)), decimales(fijos.Max(x => x.Total)));

            }


            htmlCode += @"</table>
            </div>
            <div class=""grafica-contenedor notarias"">";
            htmlCode += String.Format(@"<img src = ""{0}"" alt="""">", imageToBase64(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "cotejos_movil_p4 " + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".jpg"));
            htmlCode += @"</div>
        </div>
    </div>";

            //quinto reporte

            htmlCode += @"<div class=""paginas"">
        <div>
            <div class=""flex-container header"">
                <div class=""logo-ucnc"">
                    <img src = ""data:image/jpeg;base64,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RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNQeGxAcsAAAAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEAEsAAAAAQACASwAAAABAAI4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAI4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAE4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAU4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGYAAABbAAAACQBsAG8AZwBvAF8AdQBjAG4AYwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAWwAAAGYAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAA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SU0EBgAAAAAABwABAAAAAQEA/+EOVGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNi4wLWMwMDUgNzkuMTY0NTkwLCAyMDIwLzEyLzA5LTExOjU3OjQ0ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgeG1wOkNyZWF0ZURhdGU9IjIwMjAtMTItMzBUMTU6NTE6NDctMDU6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBkYzpmb3JtYXQ9ImltYWdlL2pwZWciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzg1YWM3NS0wNzRmLTQ5ODktOTk0OC03MDEwZTJjYTBjMDIiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDphZjAxOTNkNS1lOGEzLTVjNDUtYjYyMi1iODQ0YmExMDk4ZWYiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowZGExOGVhZS0wMzBlLTQyOWQtYjhlNi1iMDg3Mzg1ZDczM2EiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjBkYTE4ZWFlLTAzMGUtNDI5ZC1iOGU2LWIwODczODVkNzMzYSIgc3RFdnQ6d2hlbj0iMjAyMC0xMi0zMFQxNTo1MTo0Ny0wNTowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIDIyLjEgKE1hY2ludG9zaCkiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBpbWFnZS9wbmcgdG8gaW1hZ2UvanBlZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YTM4NWFjNzUtMDc0Zi00OTg5LTk5NDgtNzAxMGUyY2EwYzAyIiBzdEV2dDp3aGVuPSIyMDIxLTAxLTIxVDExOjI3OjEwLTA1OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMSAoTWFjaW50b3NoKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPD94cGFja2V0IGVuZD0idyI/Pv/iDFhJQ0NfUFJPRklMRQABAQAADEhMaW5vAhAAAG1udHJSR0IgWFlaIAfOAAIACQAGADEAAGFjc3BNU0ZUAAAAAElFQyBzUkdCAAAAAAAAAAAAAAAAAAD21gABAAAAANMtSFAgIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEWNwcnQAAAFQAAAAM2Rlc2MAAAGEAAAAbHd0cHQAAAHwAAAAFGJrcHQAAAIEAAAAFHJYWVoAAAIYAAAAFGdYWVoAAAIsAAAAFGJYWVoAAAJAAAAAFGRtbmQAAAJUAAAAcGRtZGQAAALEAAAAiHZ1ZWQAAANMAAAAhnZpZXcAAAPUAAAAJGx1bWkAAAP4AAAAFG1lYXMAAAQMAAAAJHRlY2gAAAQwAAAADHJUUkMAAAQ8AAAIDGdUUkMAAAQ8AAAIDGJUUkMAAAQ8AAAIDHRleHQAAAAAQ29weXJpZ2h0IChjKSAxOTk4IEhld2xldHQtUGFja2FyZCBDb21wYW55AABkZXNjAAAAAAAAABJzUkdCIElFQzYxOTY2LTIuMQAAAAAAAAAAAAAAEnNSR0IgSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABYWVogAAAAAAAA81EAAQAAAAEWzFhZWiAAAAAAAAAAAAAAAAAAAAAAWFlaIAAAAAAAAG+iAAA49QAAA5BYWVogAAAAAAAAYpkAALeFAAAY2lhZWiAAAAAAAAAkoAAAD4QAALbPZGVzYwAAAAAAAAAWSUVDIGh0dHA6Ly93d3cuaWVjLmNoAAAAAAAAAAAAAAAWSUVDIGh0dHA6Ly93d3cuaWVjLmNoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGRlc2MAAAAAAAAALklFQyA2MTk2Ni0yLjEgRGVmYXVsdCBSR0IgY29sb3VyIHNwYWNlIC0gc1JHQgAAAAAAAAAAAAAALklFQyA2MTk2Ni0yLjEgRGVmYXVsdCBSR0IgY29sb3VyIHNwYWNlIC0gc1JHQgAAAAAAAAAAAAAAAAAAAAAAAAAAAABkZXNjAAAAAAAAACxSZWZlcmVuY2UgVmlld2luZyBDb25kaXRpb24gaW4gSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAAsUmVmZXJlbmNlIFZpZXdpbmcgQ29uZGl0aW9uIGluIElFQzYxOTY2LTIuMQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdmlldwAAAAAAE6T+ABRfLgAQzxQAA+3MAAQTCwADXJ4AAAABWFlaIAAAAAAATAlWAFAAAABXH+dtZWFzAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAACjwAAAAJzaWcgAAAAAENSVCBjdXJ2AAAAAAAABAAAAAAFAAoADwAUABkAHgAjACgALQAyADcAOwBAAEUASgBPAFQAWQBeAGMAaABtAHIAdwB8AIEAhgCLAJAAlQCaAJ8ApACpAK4AsgC3ALwAwQDGAMsA0ADVANsA4ADlAOsA8AD2APsBAQEHAQ0BEwEZAR8BJQErATIBOAE+AUUBTAFSAVkBYAFnAW4BdQF8AYMBiwGSAZoBoQGpAbEBuQHBAckB0QHZAeEB6QHyAfoCAwIMAhQCHQImAi8COAJBAksCVAJdAmcCcQJ6AoQCjgKYAqICrAK2AsECywLVAuAC6wL1AwADCwMWAyEDLQM4A0MDTwNaA2YDcgN+A4oDlgOiA64DugPHA9MD4APsA/kEBgQTBCAELQQ7BEgEVQRjBHEEfgSMBJoEqAS2BMQE0wThBPAE/gUNBRwFKwU6BUkFWAVnBXcFhgWWBaYFtQXFBdUF5QX2BgYGFgYnBjcGSAZZBmoGewaMBp0GrwbABtEG4wb1BwcHGQcrBz0HTwdhB3QHhgeZB6wHvwfSB+UH+AgLCB8IMghGCFoIbgiCCJYIqgi+CNII5wj7CRAJJQk6CU8JZAl5CY8JpAm6Cc8J5Qn7ChEKJwo9ClQKagqBCpgKrgrFCtwK8wsLCyILOQtRC2kLgAuYC7ALyAvhC/kMEgwqDEMMXAx1DI4MpwzADNkM8w0NDSYNQA1aDXQNjg2pDcMN3g34DhMOLg5JDmQOfw6bDrYO0g7uDwkPJQ9BD14Peg+WD7MPzw/sEAkQJhBDEGEQfhCbELkQ1xD1ERMRMRFPEW0RjBGqEckR6BIHEiYSRRJkEoQSoxLDEuMTAxMjE0MTYxODE6QTxRPlFAYUJxRJFGoUixStFM4U8BUSFTQVVhV4FZsVvRXgFgMWJhZJFmwWjxayFtYW+hcdF0EXZReJF64X0hf3GBsYQBhlGIoYrxjVGPoZIBlFGWsZkRm3Gd0aBBoqGlEadxqeGsUa7BsUGzsbYxuKG7Ib2hwCHCocUhx7HKMczBz1HR4dRx1wHZkdwx3sHhYeQB5qHpQevh7pHxMfPh9pH5Qfvx/qIBUgQSBsIJggxCDwIRwhSCF1IaEhziH7IiciVSKCIq8i3SMKIzgjZiOUI8Ij8CQfJE0kfCSrJNolCSU4JWgllyXHJfcmJyZXJocmtyboJxgnSSd6J6sn3CgNKD8ocSiiKNQpBik4KWspnSnQKgIqNSpoKpsqzysCKzYraSudK9EsBSw5LG4soizXLQwtQS12Last4S4WLkwugi63Lu4vJC9aL5Evxy/+MDUwbDCkMNsxEjFKMYIxujHyMioyYzKbMtQzDTNGM38zuDPxNCs0ZTSeNNg1EzVNNYc1wjX9Njc2cjauNuk3JDdgN5w31zgUOFA4jDjIOQU5Qjl/Obw5+To2OnQ6sjrvOy07azuqO+g8JzxlPKQ84z0iPWE9oT3gPiA+YD6gPuA/IT9hP6I/4kAjQGRApkDnQSlBakGsQe5CMEJyQrVC90M6Q31DwEQDREdEikTORRJFVUWaRd5GIkZnRqtG8Ec1R3tHwEgFSEtIkUjXSR1JY0mpSfBKN0p9SsRLDEtTS5pL4kwqTHJMuk0CTUpNk03cTiVObk63TwBPSU+TT91QJ1BxULtRBlFQUZtR5lIxUnxSx1MTU19TqlP2VEJUj1TbVShVdVXCVg9WXFapVvdXRFeSV+BYL1h9WMtZGllpWbhaB1pWWqZa9VtFW5Vb5Vw1XIZc1l0nXXhdyV4aXmxevV8PX2Ffs2AFYFdgqmD8YU9homH1YklinGLwY0Njl2PrZEBklGTpZT1lkmXnZj1mkmboZz1nk2fpaD9olmjsaUNpmmnxakhqn2r3a09rp2v/bFdsr20IbWBtuW4SbmtuxG8eb3hv0XArcIZw4HE6cZVx8HJLcqZzAXNdc7h0FHRwdMx1KHWFdeF2Pnabdvh3VnezeBF4bnjMeSp5iXnnekZ6pXsEe2N7wnwhfIF84X1BfaF+AX5ifsJ/I3+Ef+WAR4CogQqBa4HNgjCCkoL0g1eDuoQdhICE44VHhauGDoZyhteHO4efiASIaYjOiTOJmYn+imSKyoswi5aL/IxjjMqNMY2Yjf+OZo7OjzaPnpAGkG6Q1pE/kaiSEZJ6kuOTTZO2lCCUipT0lV+VyZY0lp+XCpd1l+CYTJi4mSSZkJn8mmia1ZtCm6+cHJyJnPedZJ3SnkCerp8dn4uf+qBpoNihR6G2oiailqMGo3aj5qRWpMelOKWpphqmi6b9p26n4KhSqMSpN6mpqhyqj6sCq3Wr6axcrNCtRK24ri2uoa8Wr4uwALB1sOqxYLHWskuywrM4s660JbSctRO1irYBtnm28Ldot+C4WbjRuUq5wro7urW7LrunvCG8m70VvY++Cr6Evv+/er/1wHDA7MFnwePCX8Lbw1jD1MRRxM7FS8XIxkbGw8dBx7/IPci8yTrJuco4yrfLNsu2zDXMtc01zbXONs62zzfPuNA50LrRPNG+0j/SwdNE08bUSdTL1U7V0dZV1tjXXNfg2GTY6Nls2fHadtr724DcBdyK3RDdlt4c3qLfKd+v4DbgveFE4cziU+Lb42Pj6+Rz5PzlhOYN5pbnH+ep6DLovOlG6dDqW+rl63Dr++yG7RHtnO4o7rTvQO/M8Fjw5fFy8f/yjPMZ86f0NPTC9VD13vZt9vv3ivgZ+Kj5OPnH+lf65/t3/Af8mP0p/br+S/7c/23////uAA5BZG9iZQBkgAAAAAH/2wCEAAwICAgJCAwJCQwRCwoLERUPDAwPFRgTExUTExgRDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwBDQsLDQ4NEA4OEBQODg4UFA4ODg4UEQwMDAwMEREMDAwMDAwRDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIAGYAWwMBIgACEQEDEQH/3QAEAAb/xAE/AAABBQEBAQEBAQAAAAAAAAADAAECBAUGBwgJCgsBAAEFAQEBAQEBAAAAAAAAAAEAAgMEBQYHCAkKCxAAAQQBAwIEAgUHBggFAwwzAQACEQMEIRIxBUFRYRMicYEyBhSRobFCIyQVUsFiMzRygtFDByWSU/Dh8WNzNRaisoMmRJNUZEXCo3Q2F9JV4mXys4TD03Xj80YnlKSFtJXE1OT0pbXF1eX1VmZ2hpamtsbW5vY3R1dnd4eXp7fH1+f3EQACAgECBAQDBAUGBwcGBTUBAAIRAyExEgRBUWFxIhMFMoGRFKGxQiPBUtHwMyRi4XKCkkNTFWNzNPElBhaisoMHJjXC0kSTVKMXZEVVNnRl4vKzhMPTdePzRpSkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2JzdHV2d3h5ent8f/2gAMAwEAAhEDEQA/APVUkDMzcXBx35OXa2mln0nuMD4D9538lY4r6j9YNbxZgdIPFP0cjIH/AA352Nju/wBH/O2IE/avjC9SeGI/S/7396Tbp+sXTL+ojAqs3PJLW2gfonWN1fRXd9B9zW+7YtRZ2b03pH7OGFe1mPiNgVbSK9jh/Nvpf+Za130VVpu6/hk42yvqzKwNtosbTcAfofaK3/o3O/4RiFkb/gv9uMxcDw10yGuL+sJ/J/gu2srK6/VVe6nEpszjRrlGgAipvmf8Jb/wLPegXVdTywP2pkM6divMfZqHzY/+RZlHb/m0MWrhUYmPjtqw2sZQ36IZEfh9JKydvT+f2JEceMXP9af3Yn9WP72T9L/qasTMxs2huRi2C2p3Dm+P7p/dd/JR1jZPT8R9js/pmU3Cyt22x7CHVPd/o8mj6Dn/APgqc5v1jqaa39PpueOL2XhlZ/lOZY31WJCXcfUaolhB1hIf3chGOcf8b0TbvUupY/TcU5N8kAhrGNEve5xhtVTPz7Hfup+n9Sw+o0eviP3tna9p0cxw+lXaw+6uxqz+nYbcnMb1DqGTXmZjAfQqqP6GkHR3oMPusf8AvZDkXqHRTZf+0Om2fY+oga2ATXaB/g8uofzjf+E/na0rO/Tt1UYYx6CTxfv/AKH93h+b/DdVJZXTeti+/wCwdQq+xdSaJNDjLbAP8LiW/wCGr/8ABGfnrVRsVbH7cuLhrX8PPif/0O7+sfS7cllHUMUB+Z05xupqfrXZp+kqfWfbvez+Zt+nVYtLAzqM/CozKD+jyGh7Z5E8tP8AKajnhcJ1acfpmbhV3OxRh9U3U2MkljbR9qY1jWln51j/AM5NJ4dWbFH3QIE1wnQ/1Zb/APOek+tOPZk9HfXS31Htsqfs0khj2Wv2z+dsajU5tX2o2NY4syS1rXwBt2NLt9u7a5jPzFwOW57/AKr2PsufkOd1IE3WSHOmnuNz9qxG/P71FLLRuugdDDyHHj4TP5ZSHy/vcP8AW/qvqF4dX9YWZtv6XE+zmqtzfd6dm7c6WD3N9Vv+EWhi5OPusprqNDK4cC4BrHb5cfTh3+evJG/E/eUTtyfvKZ94o/L+LY/0MJgA5thw/J2/w30B+DZTmNzsGPTybYz8c6SGvLqsutp/wrf+mxWutev1LoF7MOaci9ntqsIY+J99Tvd7HWM9q8ydPifvKv8ASvq51bq7TZit20gwbrHFrZ/k/nP2pRy3YEd+gKM3ICHDknmA9sipSh2+WMvX6n0OrLxGV0PrxLN7Q2oNbWA6sO2tc07tvsZ/hNi0pETOi80z/q5Z0PqHTDbmC+y/IYBW0OEBrmy7V59uqPSbT9YtpybfRb1Mu9D1G+lPqfR9Dd6+78//AEKlGQ7EU0MnKQIEoZOKNGXFR112ek6nS3rvWGdNGmL0xzL8m5ujzafdRjVWD3V+z9LkbP8Ai10ELG+qjd/Trc1385nZF17j4gvNdf8Am11sW0n9L+rXIByDH0j6B/f/AHv8d//R9VXD9aYLrOpkjc13Ucdm0guBLKG7muawtdt/e9y7dxAEnQdyuF6g8O+rZz3uYxmf1F1+6wbmCtznVV+oyHb2elW32Jk9mxyvzX3MY/jxf9w5vUN3/Ny/eZd+0hMN2D+ZH0a/zGrBatzLdVZ9WbDRLq3dSAr0AJHogDbXX9H/AItUP2N1NjWusp9L1PoNscxjnR+7XY5rlWmCTp2dvlZRjE8REfUd/S12onZGbgbIGVkVY7zxWZsd/bFO9ta2ui9KwrMh9F7BlvpYH3BhJBc7+bx8fbsb/LuusUfASabkuYhjgZG5AC/SP+6+Vp/Vzog6tlu9fcMOkTaW8ucfoUsP8pdT1j6zdO6HScDGZuyambaqK42V/u+q/wDe/O2q5jXW1WUdPxMB+OzcTkW7QyprANfSdudve/8AMXG/W/6vWdLyjlU7n4eQ4kPJLix51dXY4/vf4NysUccPTqf0pOSckeb5kDMeGFXixXfF/f4f0pOVRl5OZ1rFyMqx11z8isuc4/y26NH5rVs0WB/1jfT6tbCOou/RNBNrybDtdY8+1tNc+xrFgdO/5Tw/+Pr/AOqauoxyB1k/zknqTo+j6el3/bibj/av5qgaA/QoPTfVI/5AxW/ub2H4tse0rYWL9Wz6J6h086OxMuwtH8i79ZqP/gi2lY/R+lOV/l7/AK3H/g/zj//S7/6y5VlPTHY9B/Wc5zcSj+tb7HP/AOt1epYs363YdWP9X8XDrq9WumypjKt2wHaCG77PzGfvq7mD7T9asCg6sw8e3KI/luLcat39lpsQ/roD+y63Bjrdt7P0bQCXTuaGbXstY7d/KrTJbS+xsYTU8QHU8f7IvM4OSzpvRxZs9Ng6kGPbO7YH07X+nZ/I3eyxW87EZl476HtFljCX0yYlwH73/CN/1/QrKzG2O+rWS19TqX159b30v2y1r6tjP5prK/8AoK30bMOTgsLnfpsb9G8nwHure7+z/wC7Shv9HwdIRNe6NxKj/wBy4jcx7TFbG444IY33DsW73+5dHhZt+Q6urExzg+kd2FvcQLrY/SVZZ9vquyG/Qf8AmWLO6wx2Lc3Ix2NY3IkudEuFn+EbJVXDxs3Mf6zXlrazP2h5MNI19n7zv6qisxlW7fMYZcQmagKOsvV6v60f0nrHfXjCx7KWPrtc125uQ1wAsqc07fc38/ert/W+ldXx/sGDYzKuy/Z6TmztbzZdax4/wTf/AARYGViY+Rk15F1e+yyvc69zSKvboXOr/wAJY9+1mN+Z/wBto+DndH6Rc+5zmV5bWGBtraXAe59MUta73fmb1PGctRIjhczJy2CoyxQn7oF0DceP/wBBR/WDD+r/AE7P6TgY1JblstrJsrIkM3Db68/zjrXqrQf8vkAgAdTJcBsEn1fa2zX7Q93+j9voqgWZfUuvY/UQ4ZDMnKrO9kn0/cNtNrD76tjG7fcr2G2uzr/rsrYXW9Qdtu2jcW+r9Df6u/dt+j+r+nsQBs6CtdEzjwQAlIzlwHiJ19Zeqyv1D6x4+TxT1Rn2W09hdVNuK4/8ZX61S2pWR9bKyeh33s/ncMsyqj4Opc23/qQ5Xvt1P/gP2j+yp+pH1cwk8MZddYH+X92T/9Pux7friZ/wnT/Z/Zt93/VKz9YMN+b0fJorkW7N9ZHO5nvbH3Kp9YA7DyMPrTAXNwXOZlAan7Pd7bbP+svay1bTHssY2ytwcx4DmuGoIPBCHcMhJHBMdP8ApQfOemVsyq8np5aK6ep1bMZxc47rqh61Dv0/6Rzt3q12bGenvWV0fJdh52y72NefRuafzTOhP/F2Lf690cdN6o57Iqpy3iyjIEb63NPqPxaHPiupz7P0jbLH7PQVTqOA3rQf1DAAOaJ+14zRHq7Ts+24nHqsfHv2qtIH6xdnBkgQbP6vKB6v3J/L6v5f5N0XV0urNeTUH0g7nb3Db7dB/ab+d6j6f7aq5fXMJjRXjsDwzRra9GCP+FcP/PFX/X1z9uRk3kNyLHvLIG15Oke36KXZRyyHo3cPJx3mTL+qNIt1/W8z3bW1tJO5pAMtPZ2rve9v5r7d6ynd/wAUVysdO6Rl9SeTUBXjs1uyn6VsHm78538hAXI918xjxAnSA6ltfVquzHdldX12YlZZU0GN99nsor/lbP5xX/qXh2ZPXBZfjCq3p7JyLDul1hBrqGx3srd7nv8AYo35OI3GpxsSpzsGqW4hI3DJtsGx9j2t3NdfY76GPks/mf8ARrsPq10h3S+nNbdBy7yLMgjgOja2pv8AwdLP0bFYxx1A7auPzeaozkRUsnoiP0uEfy/x2X1ne1n1e6iXcfZ7B8y3aED7Ld4H/k30v7aXX3DPyMboVXude9t2ZH5mPU4Pdv8A/DFjWU1raU3UuaRUIjqSZf4L/9T1N7A8bXag6EHUEHlrliM6d1To7yOkFuTgEz+z7nbTXPIw8j3ba/8AgbVupIEWujMxvYg7xOziuy8HrLX9I6ni2Yt1rdwovA923/CY1zC9j3VfyVy/Uvq/1TpnUKMqx77cLGAbXk0yHta2XNZds91brHfzl382uz6t0wZ9LQx5pyaXi3GyAJNdje8fnVv+haxVmj61WgMe7DxtvNrQ+0u/q1u9PZ/nJk43vfgR+1t8vn9vWBiImxPHM/Lf6WP5nl7M3EzRW7Ox6c42B7he2arAytpc5z7a/wCo/Z6le9Vcev6vZW/08bLaWN3lgtYdPztu5u521dFbT01lxq+sGJXjXH+bz6Q5lNoP7z2fzNv79dykz6v/AFUn1K8oNJM7m5A5+O5RGBJ/RPe/mb8OaxwjtmjY9Bxkyxf4PBLg/wCa4Laek0kel07fYJg5VpsG5rPtPp+jV6bd3pe/6aVWTmdTApaHZBLGvx6cVorbRZofez+ZZtn1KsixbluH9T8MN3EZlzdKqGvdfYT9ENZXW7+z7lbxqPrCKhbjMxMGvlmAWEwO3rXVOb+l/qMREDtp5QY8nMxI4uGV/oy5gyj6v6n85Nq9N6HhdDrs611d1YyGy6GA+nUXfm0M/Puse7/0krruoddzm7enYX2Jjv8AtVmkAgfvMxKy6xzv+NfWkzp/U87Lpyer+k2rFO+jEpJc028NyL3vDd3pf4Ji2QFLEaaaD8XPy5LlcqyT/wDG4fuxi0Ol9Ip6eyw73ZGVkEPycqzV9jh4/uVs/wAHUz6C0Ekk6hVMPHLi4r9T/9X1VJfKqSSn6qSXyqkkp+p7fS2O9Xb6f526NsfytyzHf81ZO77BPefSXzWkmy+n+Ez4Ov8AOf8AUv2v09hfsmT+z/s89/Q2T/4ErYXyskiNun0Y8nzH5v8Aqnzv1UkvlVJFY/VSS+VUklP/2Q==""
                        alt="""">
                </div>
                <div>
                    <h1>Informe consumo de servicio de cotejos biométricos en línea Fijos y Móviles</h1>
                </div>
                <div class=""logo"">
                    <p>Elaborado por:</p>
                    <img src = ""data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyNS4wLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHZpZXdCb3g9IjAgMCA2MTkgMTMwIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA2MTkgMTMwOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+DQo8c3R5bGUgdHlwZT0idGV4dC9jc3MiPg0KCS5zdDB7ZmlsbDojMDA4MENEO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDM3LjMsODUuNGwxLjEsMGMxNi0wLjYsMjguNy0xMy43LDI4LjctMjkuOWMwLTE2LjUtMTMuNC0yOS45LTI5LjktMjkuOWMtMTYuNSwwLTI5LjksMTMuNC0yOS45LDI5Ljl2MjkuOQ0KCQloMjguOEw0MzcuMyw4NS40eiBNMzkxLjYsNTUuNUwzOTEuNiw1NS41YzAtMjUuMiwyMC40LTQ1LjcsNDUuNy00NS43YzI1LjIsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCWgtMzAuNHY4LjJjMCw1LjEtNi40LDEwLjgtMTUuMywxMC44di0xMC4zbDAuMS0xVjgwLjZjMCwwLjUsMCwxLTAuMSwxLjV2LTIuN2MwLTAuNywwLTEuNCwwLjEtMlY1OC4xDQoJCUMzOTEuNiw1Ny4zLDM5MS42LDU2LjQsMzkxLjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yNC40LDU1LjVjMCwxNi41LDEzLjQsMjkuOSwyOS45LDI5LjljMTYuNSwwLDI5LjktMTMuNCwyOS45LTI5LjljMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjkNCgkJQzM3LjgsMjUuNiwyNC40LDM5LDI0LjQsNTUuNSBNOC42LDU1LjVMOC42LDU1LjVDOC42LDMwLjMsMjksOS44LDU0LjMsOS44YzI1LjMsMCw0NS43LDIwLjUsNDUuNyw0NS43YzAsMjUuMi0yMC41LDQ1LjctNDUuNyw0NS43DQoJCUMyOSwxMDEuMiw4LjYsODAuNyw4LjYsNTUuNSIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0xMTAuMywxMC42djQ2YzAsMTguOCwxOS41LDQzLjYsMzkuMyw0My42aDMyLjdjMC02LjktNy4xLTE1LjEtMTIuMy0xNS4xaC0xOC44Yy05LjEsMC0yNS42LTE0LjEtMjUuNi0yOA0KCQlWMjEuM0MxMjUuNiwxNi4yLDExOS4yLDEwLjYsMTEwLjMsMTAuNiIvPg0KCTxwYXRoIGNsYXNzPSJzdDAiIGQ9Ik0yMDIuOCw5LjhjLTguOSwwLTE1LjMsNS42LTE1LjMsMTAuN3YzMi42Yy0wLjEsMC43LTAuMSwxLjQtMC4xLDJ2Mi43YzAuMS0wLjUsMC4xLTEuMSwwLjEtMS42Vjg4bC0wLjEsMTMuMg0KCQljOC45LDAsMTUuNC01LjYsMTUuNC0xMC44VjU3LjljMC0wLjcsMC0xLjQsMC0ydi0yLjdjMCwwLjUsMCwxLjEsMCwxLjZWMjRsMC0xVjkuOHoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMjEzLjQsMTAwLjJ2LTQ2YzAtMTguOCwxOS41LTQzLjYsMzkuMi00My42aDMuNWg1LjdoNWMxMi4yLDAsMjQuMiw5LjQsMzEuNywyMWM3LjUtMTEuNiwxOS42LTIxLDMxLjctMjFoNC45DQoJCWgyLjNoNC41YzE5LjcsMCwzOS4yLDI0LjgsMzkuMiw0My42djQ2Yy04LjksMC0xNS4zLTUuNi0xNS4zLTEwLjdWNTMuN2MwLTEzLjktMTYuNS0yOC0yNS42LTI4aC04LjVjLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4DQoJCXYzNS43YzAsMC41LTAuMSwwLjktMC4yLDEuMXY5LjZjLTguOSwwLTE1LjMtNS42LTE1LjMtMTAuN1Y1My43YzAtMTMuOS0xNi41LTI4LTI1LjYtMjhoLTEwLjljLTkuMSwwLTI1LjYsMTQuMS0yNS42LDI4djM1LjcNCgkJQzIyOC43LDk0LjYsMjIyLjMsMTAwLjIsMjEzLjQsMTAwLjIiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNDkzLjMsMTAuNmM4LjksMCwxNS4zLDUuNiwxNS4zLDEwLjd2MzIuNmMwLDAuNywwLjEsMS40LDAuMSwydjIuN2MwLTAuNS0wLjEtMS4xLTAuMS0xLjZ2MzEuN2wwLjEsMTMuMg0KCQljLTguOCwwLTE1LjMtNS42LTE1LjMtMTAuOFY1OC42YzAtMC43LTAuMS0xLjQtMC4xLTJ2LTIuN2MwLDAuNSwwLjEsMS4xLDAuMSwxLjZWMjQuOGwtMC4xLTFWMTAuNnoiLz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNNTY0LjcsODUuNGwxLjEsMGgyOC43VjU1LjVjMC0xNi41LTEzLjQtMjkuOS0yOS45LTI5LjljLTE2LjUsMC0yOS45LDEzLjQtMjkuOSwyOS45DQoJCWMwLDE2LjEsMTIuOCwyOS4zLDI4LjcsMjkuOUw1NjQuNyw4NS40eiBNNjEwLjQsNTUuNUw2MTAuNCw1NS41YzAsMC45LDAsMS44LTAuMSwyLjZ2MTkuM2MwLjEsMC43LDAuMSwxLjMsMC4xLDJ2Mi43DQoJCWMwLTAuNS0wLjEtMS0wLjEtMS41djIwLjZoLTE1LjJoLTkuMmgtMjEuMmMtMjUuMiwwLTQ1LjctMjAuNS00NS43LTQ1LjdjMC0yNS4yLDIwLjUtNDUuNyw0NS43LTQ1LjcNCgkJQzU5MCw5LjgsNjEwLjQsMzAuMyw2MTAuNCw1NS41Ii8+DQo8L2c+DQo8L3N2Zz4NCg==""
                        alt="""">";
            htmlCode += String.Format(@"<div class=""text-doc"">
                                                        <p>Fecha de elaboración:</p>
                                                        <p>{0}</p>
                                            </div>", String.Format("{0:D}", DateTime.Now));

            htmlCode += @" </div>
                                        </div>
                                    </div>
                                    <div class=""wrapper-print body-flex"">
                                        <div class=""tabla-uno-contenedor"">
                                            <table cellspacing = ""0"" cellpadding=""0"" class=""tabla tabla-mes"">";
            htmlCode += String.Format(@"<h4 class=""titulo-tabla"">Notarías <span>{0}</span> </h4>
                                                <thead>", String.Format("{0:y}", Listar_cotejos_Total[0].Fecha));
            htmlCode += @" <tr>
                            <th rowspan = ""2"" >#</th>
                            <th rowspan=""2"">FECHA</th>
                            <th colspan = ""2""> COTEJOS EN NOTARIAS MÓVILES</th>
                            <th class=""total"" rowspan=""2"">TOTAL</th>
                        </tr>
                        <tr>
                            <th>MAYORES A 30</th>
                            <th>MENORES A 30</th>
                        </tr>
                    </thead>
                    <tbody>";
            if (movil.Count > 0)
            {
                int i = 1;
                int j = 0;
                int promedioMayores = 0;
                int promedioMenores = 0;
                int promediototal = 0;
                foreach (var item in movil)
                {
                    promedioMayores += item.MayoresA30;
                    promedioMenores += item.MenoresA30;
                    promediototal += item.Total;

                    htmlCode += String.Format(@"<tr>
                            <td class=""numeros"">{0}</td>
                            <td>{1}</td>
                            <td class=""numeros"">{2}</td>
                            <td class=""numeros"">{3}</td>
                            <td class=""numeros total"">{4}</td>
                        </tr>", i, item.Dias, decimales(item.MayoresA30), decimales(item.MenoresA30), decimales(item.Total));
                    i++;
                    j++;
                }
                htmlCode += String.Format(@" </tbody>
                    <tfoot>
                        <tr>
                            <td colspan = ""2"" class=""subtitulo-tabla"">Promedio</td>
                            <td class=""numeros"">{0}</td>
                            <td class=""numeros"">{1}</td>
                            <td class=""numeros"">{2}</td>
                        </tr>
                        <tr>
                            <td colspan = ""2"" class=""subtitulo-tabla"">Máximo</td>
                            <td class=""numeros"">{3}</td>
                            <td class=""numeros"">{4}</td>
                            <td class=""numeros"">{5}</td>
                        </tr>
                    </tfoot>", decimales(promedioMayores / j), decimales(promedioMenores / j), decimales(promediototal / j), decimales(movil.Max(x => x.MayoresA30)), decimales(movil.Max(x => x.MenoresA30)), decimales(movil.Max(x => x.Total)));

            }

            htmlCode += @"</table>
            </div>
            <div class=""grafica-contenedor notarias"">";
            htmlCode += String.Format(@"<img src = ""{0}"" alt="""">", imageToBase64(ConfigurationManager.AppSettings["RutaDocumentos"].ToString() + "cotejos_movil_p5 " + string.Format("{0:ddMMyyyy}", DateTime.Now) + ".jpg"));
            htmlCode += @"</div>
        </div>
    </div>";
            htmlCode += @" </body>
                   </html>";
            return htmlCode;
        }
        public static string imageToBase64(string _imagePath)
        {
            string _base64String = null;

            using (System.Drawing.Image _image = System.Drawing.Image.FromFile(_imagePath))
            {
                using (MemoryStream _mStream = new MemoryStream())
                {
                    _image.Save(_mStream, _image.RawFormat);
                    byte[] _imageBytes = _mStream.ToArray();
                    _base64String = Convert.ToBase64String(_imageBytes);

                    return "data:image/jpg;base64," + _base64String;
                }
            }
        }
        public static NotariaGrafica.Totales Totales(List<NotariaGrafica.Total> Listar_cotejos_Total)
        {

            List<DateTime> diasFestivos = Generacion_PDF_Notaria.CalcularDiasFestivos.FestivosColombia.DiasFestivos(Listar_cotejos_Total[0].Fecha.Year);
            int totalHIT = 0;
            int totalNOHIT = 0;
            int totaTOTAL = 0;
            int i = 0;
            foreach (NotariaGrafica.Total fechauno in Listar_cotejos_Total)
            {
                bool status = true;
                string fehchaHoy = String.Format("{0:D}", fechauno.Fecha);
                string[] fehcha = fehchaHoy.Split(',');
                if (fehcha[0] != "sábado" & fehcha[0] != "domingo")
                {
                    foreach (DateTime fecha in diasFestivos)
                    {
                        if (String.Format("{0:y}", fechauno.Fecha) == String.Format("{0:y}", fecha))
                            if (fecha == fechauno.Fecha)
                            {
                                status = false;
                                break;
                            }
                            else
                                status = true;

                    };
                }
                else
                    status = false;
                if (status == true)
                {
                    totalHIT += fechauno.HIT;
                    totalNOHIT += fechauno.NOHIT;
                    totaTOTAL += fechauno.TOTAL;
                    i++;
                }
            };
            NotariaGrafica.Totales totales = new NotariaGrafica.Totales()
            {
                HIT = totalHIT / i,
                NOHIT = totalNOHIT / i,
                TOTAL = totaTOTAL / i,
            };
            return totales;
        }
        public static bool validaciones()
        {
            if (db.PruebaConexion())
                return true;
            return false;
        }
        public static string decimales(int numero)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.NumberDecimalSeparator = ",";
            formato.CurrencyGroupSeparator = ".";
            string total = numero.ToString("N", formato);
            string[] miles = total.Split(',');
            return miles[0];
        }
        public static void eliminar(string nombreArchivo)
        {
            List<string> files = new List<string>();
            DateTime fecha = DateTime.Now;
            try
            {
                foreach (string file in Directory.GetFiles(ConfigurationManager.AppSettings["RutaDocumentos"].ToString(), nombreArchivo + string.Format("{0:ddMMyyyy}", fecha) + ".pdf"))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void eliminarimagen()
        {
            List<string> files = new List<string>();
            DateTime fecha = DateTime.Now;
            try
            {
                foreach (string file in Directory.GetFiles(ConfigurationManager.AppSettings["RutaDocumentos"].ToString(), "*" + string.Format("{0:ddMMyyyy}", fecha) + ".jpg"))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<Totalesanios> totales()
        {

            List<Totalesanios> listtotal = new List<Totalesanios>()
            {
              new Totalesanios() { Producto = "Total", Total = 1118962, Anio = 2015},
              new Totalesanios() { Producto = "Total", Total = 8958882, Anio = 2016},
              new Totalesanios() { Producto = "Total", Total = 12067044, Anio = 2017},
              new Totalesanios() { Producto = "Total", Total = 13169116, Anio = 2018},
              new Totalesanios() { Producto = "Total", Total = 14171134, Anio = 2019},
              new Totalesanios() { Producto = "Total", Total = 8512141, Anio = 2020},

              new Totalesanios() { Producto = "Fijo", Total = 8958882, Anio = 2016 },
              new Totalesanios() { Producto = "Fijo", Total = 12054952, Anio = 2017 },
              new Totalesanios() { Producto = "Fijo", Total = 13005994, Anio = 2018 },
              new Totalesanios() { Producto = "Fijo", Total = 14007536, Anio = 2019 },
              new Totalesanios() { Producto = "Fijo", Total = 8363556, Anio = 2020 },

              new Totalesanios() { Producto = "Movil", Total = 8958882, Anio = 2016 },
              new Totalesanios() { Producto = "Movil", Total = 12054952, Anio = 2017 },
              new Totalesanios() { Producto = "Movil", Total = 13005994, Anio = 2018 },
              new Totalesanios() { Producto = "Movil", Total = 14007536, Anio = 2019 },
              new Totalesanios() { Producto = "Movil", Total = 8363556, Anio = 2020 },
            };

            return listtotal;
        }
    }
}
