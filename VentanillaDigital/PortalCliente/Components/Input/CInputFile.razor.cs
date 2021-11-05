using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using BlazorInputFile;
using System.Linq;

namespace PortalCliente.Components.Input
{
    public partial class CInputFile : ComponentBase
    {
        private IFileListEntry fileSelected;

        [Parameter]
        public byte[] FileContent
        {
            get => _fileContent;
            set
            {
                if (_fileContent == value) return;
                _fileContent = value;
                FileContentChanged.InvokeAsync(value);
            }
        }
           [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Accept { get; set; }
        [Parameter]
        public EventCallback<byte[]> FileContentChanged { get; set; }

        [Parameter]
        public string Placeholder { get; set; }
        
        private byte[] _fileContent;
        // [Inject]
        // IFileReaderService fileReaderService{get;set;}
        protected async void OnInputFileChange(IFileListEntry[] files)
        {
            fileSelected = files.FirstOrDefault();
            if (fileSelected != null)
            {
                using(var ms = new MemoryStream())
                {
                    await fileSelected.Data.CopyToAsync(ms);
                    FileContent = ms.ToArray();
                }
            }
        }
    }
}