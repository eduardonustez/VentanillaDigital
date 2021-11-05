using ApiGatewayAdministrador.Helper;
using ApiGateway.Models;
using ApiGateway.Models.Transaccional;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiGatewayAdministrador.Mapper
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

        }
    }
}
