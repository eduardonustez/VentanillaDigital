using ApiGateway.Models.Transaccional;
using FluentValidation;
using ApiGateway.Helper;

namespace ApiGateway.Validators
{
    public class NuevoComparecienteValidator : AbstractValidator<ComparecienteCreateRequest>
    {
        public NuevoComparecienteValidator()
        {
            RuleFor(c => c.TramiteId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .ScalePrecision(0, 19);

            RuleFor(c => c.TipoDocumentoId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .ScalePrecision(0, 10);

            RuleFor(c => c.NumeroDocumento)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(15);

            #region Foto

            //RuleFor(c => c.Foto.FileName)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .NotNull()
            //    .Matches(ExpresionRegularModelos.NombreDocumento);

            //RuleFor(c => c.Foto.ContentType)
            //    .Cascade(CascadeMode.Stop)
            //    .NotNull()
            //    .Must(x => x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenPng)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenBmp)
            //    || x.Equals(ExpresionRegularModelos.mimeTypePDF));

            #endregion

            #region Firma

            //RuleFor(c => c.Firma.FileName)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .NotNull()
            //    .Matches(ExpresionRegularModelos.NombreDocumento);

            //RuleFor(c => c.Firma.ContentType)
            //    .Cascade(CascadeMode.Stop)
            //    .NotNull()
            //    .Must(x => x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenPng)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenBmp)
            //    || x.Equals(ExpresionRegularModelos.mimeTypePDF));
            #endregion

            #region ImagenDocumento

            //RuleFor(c => c.ImagenDocumento.FileName)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty()
            //    .NotNull()
            //    .Matches(ExpresionRegularModelos.NombreDocumento);

            //RuleFor(c => c.ImagenDocumento.ContentType)
            //    .Cascade(CascadeMode.Stop)
            //    .NotNull()
            //    .Must(x => x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenJpeg)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenPng)
            //    || x.Equals(ExpresionRegularModelos.mimeTypeImagenBmp)
            //    || x.Equals(ExpresionRegularModelos.mimeTypePDF));
            #endregion
        }
    }
}
