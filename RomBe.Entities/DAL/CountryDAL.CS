using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;

namespace RomBe.Entities.DAL
{
    public class CountryDAL
    {
        public int GetCountryIdByName(string countryName)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Country _userCountry = context.Countries.Where(c => c.CountryName.ToLower() == countryName.ToLower()).FirstOrDefault();
                if(_userCountry.IsNull())
                {
                    _userCountry = new Country()
                    {
                        CountryName = countryName,
                        InsertDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    context.Countries.Add(_userCountry);
                    context.SaveChanges();
                }
                return _userCountry.CountryId;
            }
        }
    }
}
