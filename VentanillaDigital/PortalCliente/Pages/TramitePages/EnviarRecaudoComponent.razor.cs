using ApiGateway.Contratos.Models.Notario;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using PortalCliente.Functions;
using PortalCliente.Services;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public class ArchivoSeleccionado
    {
        public ArchivoTramiteVirtual Archivo { get; set; }
        public bool Seleccionado { get; set; }
    }

    public partial class EnviarRecaudoComponent
    {
        [Parameter] public long TramitePortalVirtualId { get; set; }
        [Parameter] public long RecaudoTramiteVirtualId { get; set; }
        [Parameter] public List<ArchivoTramiteVirtual> Archivos { get; set; } = new List<ArchivoTramiteVirtual>();
        public List<UploadFileModel> Files { get; set; } = new List<UploadFileModel>();
        [Inject] public ITramiteVirtualService tramitesVirtualService { get; set; }
        [Inject] private NotificationService notificationService { get; set; }
        [Parameter] public EventCallback<bool> OnSend { get; set; }

        public List<ArchivoSeleccionado> ArchivosSeleccionados { get; set; } = new List<ArchivoSeleccionado>();
        public bool IsLoading { get; set; }
        public string MensajeError { get; set; }

        protected override void OnInitialized()
        {
            ArchivosSeleccionados = Archivos?.Select(m => new ArchivoSeleccionado
            {
                Archivo = m,
                Seleccionado = false
            }).ToList();
        }

        /// <summary>
        /// Seleciona un registro de la tabla
        /// </summary>
        /// <param name="archivoPortalVirtualId"></param>
        /// <param name="checkedValue"></param>
        void SeleccionarArchivo(long archivoPortalVirtualId, object checkedValue)
        {
            MensajeError = "";

            if ((bool)checkedValue)
            {
                if (!ValidarCantidadArchivos()) return;
            }

            ArchivosSeleccionados.Find(m => m.Archivo.ArchivosPortalVirtualId == archivoPortalVirtualId).Seleccionado = (bool)checkedValue;
        }

        /// <summary>
        /// Remueve un archivo de la lista
        /// </summary>
        /// <param name="item"></param>
        protected void RemoveFile(UploadFileModel item)
        {
            Files.Remove(item);
            StateHasChanged();
        }

        void ShowSuccessNotification(string title, string text)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = title, Detail = text, Duration = 4000 };
            notificationService.Notify(message);
        }

        /// <summary>
        /// Recorta el nombre del archivo subido a 30 caracteres
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string ShortName(string value)
        {
            if (value.Length > 30)
            {
                value = value.Substring(0, 30);
                return $"{value} ...";
            }
            return value;
        }

        /// <summary>
        /// Valida los archivos permitidos para enviar
        /// </summary>
        /// <returns></returns>
        private bool ValidarCantidadArchivos()
        {
            MensajeError = "";

            if (ArchivosSeleccionados.Where(m => m.Seleccionado).Count() + Files.Count >= 3)
            {
                MensajeError = "Máximo 3 archivos permitidos";

                return false;
            }

            return true;
        }

        /// <summary>
        /// Carga los datos del archivo en un listado con la info en base64
        /// </summary>
        /// <param name="eventArgs"></param>
        private async Task OnFileChange(IFileListEntry[] files)
        {
            if (files.Length > 0)
            {
                var existFiles = ExistsFiles(files);
                if (!string.IsNullOrEmpty(existFiles))
                {
                    MensajeError = existFiles;
                    return;
                }

                Files.Clear();

                if (!ValidarCantidadArchivos()) return;

                IsLoading = true;

                foreach (var file in files)
                {
                    var fileInfo = new System.IO.FileInfo(file.Name);

                    Files.Add(new UploadFileModel
                    {
                        Index = GetIndex(),
                        Nombre = file.Name.Replace(fileInfo.Extension, ""),
                        Formato = file.Type,
                        Data = Convert.ToBase64String(await file.Data.ReadToEndAsync()),
                        Type = 2
                    });

                }
            }

            IsLoading = false;
        }

        /// <summary>
        /// Obtiene el índice disponible de la lista
        /// </summary>
        /// <returns></returns>
        private int GetIndex()
        {
            int counter = -1;
            do
            {
                counter++;
            }
            while (Files.Exists(x => x.Index == counter));
            return counter;
        }

        private string ExistsFiles(IFileListEntry[] fileListEntry)
        {
            string fileNames = string.Empty;

            fileListEntry.ToList().ForEach(f =>
            {
                var fileInfo = new System.IO.FileInfo(f.Name);
                var fileName = f.Name.Replace(fileInfo.Extension, "");

                if (Files.Exists(x => x.Nombre.Equals(fileName)))
                {
                    fileNames += $" [{f.Name}]";
                }
            });
            return fileNames.Contains("[") ? $"Estos archivos ya se encuentran en la lista: {fileNames}" : null;
        }

        async Task Enviar()
        {
            try
            {
                MensajeError = "";

                if (ArchivosSeleccionados.Where(m => m.Seleccionado).Count() + Files.Count > 3)
                {
                    MensajeError = "Máximo 3 archivos permitidos";
                    return;
                }

                IsLoading = true;

                var res = await tramitesVirtualService.EnviarRecaudo(ArchivosSeleccionados.FindAll(m => m.Seleccionado).Select(m => m.Archivo.ArchivosPortalVirtualId).ToList(), Files.Any() ? Files : null, RecaudoTramiteVirtualId);

                if (res)
                {
                    ShowSuccessNotification("Enviado", "El recaudo se envió correctamente.");
                    await OnSend.InvokeAsync(true);
                }
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Limpia el listado de archivos a enviar
        /// </summary>
        void CleanFiles()
        {
            Files.Clear();
            StateHasChanged();
        }
    }
}
