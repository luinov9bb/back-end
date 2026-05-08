using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace bookStore.BusinessLogic.Mapping
{
    public static class MapperConfig
    {
        private static IMapper? _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BookMappingProfile>();
                    });

                    _mapper = config.CreateMapper();
                }

                return _mapper;
            }
        }
    }
}
