using AutoMapper;
using IoTBackend.Aplication.DTOs;
using IoTBackend.Domain.Dominio.Entities;
using IoTBackend.Infraestructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTBackend.Aplication.UseCases.Common
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<IoTDevice, IoTDeviceDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Servidor, ServidorDTO>().ReverseMap();
            CreateMap<MQTT, MQTTDTO>().ReverseMap();
        }
    }
}
