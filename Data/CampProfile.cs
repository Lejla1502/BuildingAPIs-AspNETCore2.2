using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CoreCodeCamp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
    public class CampProfile:Profile
    {
        private readonly IMapper mapper;

        public CampProfile(IMapper mapper)
        {
            this.mapper = mapper;

            this.CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName))
                .ForMember(d => d.Talks, opt => opt.Ignore());
                //.AfterMap(AddOrUpdateCities);
            // .ForMember(dest=>dest.Talks,o=>o.MapFrom(src=>new Collection<Talk> {   }));
            //this.CreateMap<Talk, TalkModel>(MemberList.Source)
            //    .EqualityComparison((s, d) => s.TalkId == d.TalkId);

        }
            //.ForMember(a=>a.Talks,o=>o.MapFrom(m=>new Collection<Talk> { }));

            //private void AddOrUpdateCities(Camp camp, CampModel campModel)// CountryData dto, Country country)
            //{
            //    foreach (var talkDTO in campModel.Talks)
            //    {
            //        if (talkDTO.TalkId == 0)
            //        {
            //            camp.Talks.Add(this.mapper.Map<Talk>(talkDTO));// country.Cities.Add(Mapper.Map<City>(cityDTO));
            //        }
            //        else
            //        {
            //            this.mapper.Map(talkDTO, camp.Talks.SingleOrDefault(t => t.TalkId == talkDTO.TalkId));// cityDTO, country.Cities.SingleOrDefault(c => c.Id == cityDTO.Id));
            //        }
            //    }
            //}

        }
    }
