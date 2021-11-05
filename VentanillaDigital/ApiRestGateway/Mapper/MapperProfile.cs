using ApiGateway.Contratos.Models.Certificado;
using ApiGateway.Helper;
using ApiGateway.Models;
using ApiGateway.Models.Transaccional;
using AutoMapper;
using Infraestructura.KeyManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiGateway.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<IFormFile, Archivo>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => Path.GetExtension(src.FileName)))
                .ForMember(dest => dest.Tamanio, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => Utilities.ObtenerBase64Documento(src)));

            CreateMap<ComparecienteCreateRequest, ComparecienteCreateModel>()
                 .PreserveReferences().ReverseMap();

            CreateMap<LoginOtpModel, LoginFuncionarioModel>();

            CreateMap<ActaCreate, ActaCreateToPlantilla>()
                 .PreserveReferences().ReverseMap();

            #region Certificado Digital
            CreateMap<SolicitudCertificadoDto, GenerateKeyRequest>()
             .ForMember(target => target.UserId, options => options.MapFrom(source => source.Correo))
             .ForMember(target => target.FullName, options => options.MapFrom(source => source.NombreCompleto))
             .ForMember(target => target.Email, options => options.MapFrom(source => source.Correo))
             .ForMember(target => target.FirstName, options => options.MapFrom(source => source.Nombres))
             .ForMember(target => target.SurName, options => options.MapFrom(source => source.Apellidos))
             .ForMember(target => target.Department, options => options.MapFrom(source => source.Departamento))
             .ForMember(target => target.City, options => options.MapFrom(source => source.Municipio))
             .ForMember(target => target.Notary, options => options.MapFrom(source => source.Notaria))
             .ForMember(target => target.Dni, options => options.MapFrom(source => source.NumeroDocumento))
             .ForMember(target => target.DniPublicNotary, options => options.MapFrom(source => source.DniNotarioPrincipal))
             .ForMember(target => target.NotaryAddress, options => options.MapFrom(source => source.Direccion))
             .ForMember(target => target.Position, options => options.MapFrom(source => source.Cargo))
             .ForMember(target => target.PhoneNumber, options => options.MapFrom(source => source.Celular))
             .ForMember(target => target.Pin, options => options.MapFrom(source => source.PinFirma))
                .PreserveReferences().ReverseMap();


            #endregion
        }
    }
}
