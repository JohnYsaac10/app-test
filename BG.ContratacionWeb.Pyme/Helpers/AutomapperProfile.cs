using AutoMapper;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using System.Data;

namespace BG.ContratacionWeb.Pyme.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        { 
            CreateMap<SobreGiroDetalleSolicitudDto, ServicioContratacionProductos.Solicitud>()
                .ForMember(dest => dest.FechaSobregiro, opt => {
                    opt.MapFrom(src => src.FechaPago);
                })
                .ForMember(dest => dest.TotalFinanciar, opt => {
                    opt.MapFrom(src => double.Parse(src.Cantidad.Replace(",", "")));
                })
                .ForMember(dest => dest.Correspondencia, opt => {
                    opt.MapFrom(src => "");
                })
                .ForMember(dest => dest.Proposito, opt => {
                    opt.MapFrom(src => "");
                })
                .ForMember(dest => dest.TasaInteres, apt => {
                    apt.MapFrom(src => double.Parse(src.TasaInteres));
                })
                .ForMember(dest => dest.UsuarioGestor, apt => {
                    apt.MapFrom(src => "");
                })
                .ForMember(dest => dest.NoCtaCredito, apt => {
                    apt.MapFrom(src => src.Cuenta[src.Cuenta.Length - 1] == 'C' ? src.Cuenta.Remove(src.Cuenta.Length - 2) : "");
                })
                 .ForMember(dest => dest.NoCtaDebito, apt => {
                     apt.MapFrom(src => src.Cuenta[src.Cuenta.Length - 1] != 'C' ? src.Cuenta.Remove(src.Cuenta.Length - 2) : "");
                 });
                 
            CreateMap<CommonApplicationSettings, ServicioContratacionProductos.Solicitud>()
                .ForMember(dest => dest.IdProducto, opt => {
                    opt.MapFrom(src => src.ProductoSG.IdProductoNeo);
                }).ForMember(dest => dest.IdCanal, opt => {
                    opt.MapFrom(src => src.ProductoSG.idCanal);
                });

            CreateMap<Models.Producto, ServicioContratacionProductos.ProductosPyme>()
                 .ForMember(dest => dest.nombre, opt => {
                     opt.MapFrom(src => src.nombre);
                 }).ForMember(dest => dest.porcentaje, opt => {
                     opt.MapFrom(src => src.porcentajeVentas);
                 });

            CreateMap<Models.ClientesProveedores, ServicioContratacionProductos.ClientesPyme>()
                .ForMember(dest => dest.nombre, opt => {
                    opt.MapFrom(src => src.nombre);
                }).ForMember(dest => dest.porcentaje, opt => {
                    opt.MapFrom(src => src.porcentaje);
                }).ForMember(dest => dest.dias, opt => {
                    opt.MapFrom(src => src.dias);
                });
            CreateMap<int, CatalogoReduce>()
                .ForMember(dest => dest.idCodigo, opt => {
                    opt.MapFrom(src => src);
                });

            CreateMap<DataRow, FromFormSolicitudCreditoDto>()
                .ForMember(dest => dest.amortizacion, opt => {
                    opt.MapFrom(src => src["tabla_amortizacion"]);
                }).ForMember(dest => dest.destinoCredito, opt => {
                    opt.MapFrom(
                        src => Mapper.Map<int, CatalogoReduce>(int.Parse(src["id_destinocredito"].ToString())));
                        //src => new CatalogoReduce { Descripcion = src["id_destinocredito"].ToString() });
                }).ForMember(dest => dest.diaDePago, opt => {
                    opt.MapFrom(src => src["dia_pago"].ToString());
                }).ForMember(dest => dest.fechaPago, opt => {
                    opt.MapFrom(src => src["fecha_pago"].ToString());
                }).ForMember(dest => dest.monto, opt => {
                    opt.MapFrom(src => src["monto_a_financiar"]);
                }).ForMember(dest => dest.modoDePago, opt => {
                    opt.MapFrom(
                        src => Mapper.Map<int, CatalogoReduce>(int.Parse(src["id_producto"].ToString())));
                        //src => new CatalogoReduce { Descripcion = src["id_producto"].ToString() });
                }).ForMember(dest => dest.plazoCredito, opt => {
                    opt.MapFrom(
                        src => Mapper.Map<int, CatalogoReduce>(int.Parse(src["id_plazo"].ToString())));
                        //src => new CatalogoReduce { Descripcion = src["id_plazo"].ToString() });
                });
            CreateMap<FromFormInfoDirecNeg, FromFormInfoDomicilio>();
            CreateMap<DatosEstCivilCliente, EstadoCivilClienteDto>();
            CreateMap<DatosEstCivilCYG, EstadoCivilClienteDto>();
            //DatosEstCivilCliente DatosEstCivilCYG

        }


        public static void run()
        {
            Mapper.Initialize(a =>
            {
                a.AddProfile<AutomapperProfile>();
            });
        }
    }
}