using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Entities;
using System.Data.Entity;
using Newtonsoft.Json;

namespace AddCountriesToDatebase
{
    class Program
    {
        static void Main(string[] args)
        {


            //int counter = 10000;
            //using (RombeEntities context = new RombeEntities())
            //{

            //    List<RealTimeLeadingQuestion> toUpdate = context.RealTimeLeadingQuestions.OrderBy(a=>a.PeriodMax).ToList();
            //    //Console.WriteLine("number of items: {0}", toUpdate.Count);
            //    foreach (RealTimeLeadingQuestion item in toUpdate)
            //    {
            //        item.UniqueId = counter;
            //        counter++;
            //    }

            //    context.SaveChanges();
            //}

            //Console.WriteLine("Counter value: {0}", counter);


            //AddCountries();
            //AddStates();
            CreateJson();
        }

        private static void AddCountries()
        {
            using (RombeEntities context = new RombeEntities())
            {
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    RegionInfo country = new RegionInfo(culture.LCID);
                    Country newCountry = new Country();
                    if (context.Countries.Where(p => p.CountryName == country.EnglishName).Count() == 0)
                    {
                        newCountry.CountryName = country.EnglishName;
                        newCountry.DateFormat = culture.DateTimeFormat.ShortDatePattern;
                        newCountry.TimeFormat = culture.DateTimeFormat.ShortTimePattern;
                        newCountry.InsertDate = DateTime.Now;
                        newCountry.UpdateDate = DateTime.Now;
                        context.Countries.Add(newCountry);
                        context.SaveChanges();
                    }
                    //Console.WriteLine("Country DateTimeFormat: {0} Country Name English: {1}", culture.DateTimeFormat.ShortDatePattern, country.EnglishName);
                }
                //Console.ReadKey();
            }
        }

        private static void AddStates()
        {
            using (RombeEntities context = new RombeEntities())
            {

                Dictionary<string, string> states = new Dictionary<string, string>();
                states.Add("AL", "Alabama");
                states.Add("AK", "Alaska");
                states.Add("AZ", "Arizona");
                states.Add("AR", "Arkansas");
                states.Add("CA", "California");
                states.Add("CO", "Colorado");
                states.Add("CT", "Connecticut");
                states.Add("DE", "Delaware");
                states.Add("DC", "District of Columbia");
                states.Add("FL", "Florida");
                states.Add("GA", "Georgia");
                states.Add("HI", "Hawaii");
                states.Add("ID", "Idaho");
                states.Add("IL", "Illinois");
                states.Add("IN", "Indiana");
                states.Add("IA", "Iowa");
                states.Add("KS", "Kansas");
                states.Add("KY", "Kentucky");
                states.Add("LA", "Louisiana");
                states.Add("ME", "Maine");
                states.Add("MD", "Maryland");
                states.Add("MA", "Massachusetts");
                states.Add("MI", "Michigan");
                states.Add("MN", "Minnesota");
                states.Add("MS", "Mississippi");
                states.Add("MO", "Missouri");
                states.Add("MT", "Montana");
                states.Add("NE", "Nebraska");
                states.Add("NV", "Nevada");
                states.Add("NH", "New Hampshire");
                states.Add("NJ", "New Jersey");
                states.Add("NM", "New Mexico");
                states.Add("NY", "New York");
                states.Add("NC", "North Carolina");
                states.Add("ND", "North Dakota");
                states.Add("OH", "Ohio");
                states.Add("OK", "Oklahoma");
                states.Add("OR", "Oregon");
                states.Add("PA", "Pennsylvania");
                states.Add("RI", "Rhode Island");
                states.Add("SC", "South Carolina");
                states.Add("SD", "South Dakota");
                states.Add("TN", "Tennessee");
                states.Add("TX", "Texas");
                states.Add("UT", "Utah");
                states.Add("VT", "Vermont");
                states.Add("VA", "Virginia");
                states.Add("WA", "Washington");
                states.Add("WV", "West Virginia");
                states.Add("WI", "Wisconsin");
                states.Add("WY", "Wyoming");


                foreach (KeyValuePair<string, string> state in states)
                {

                    State newState = new State();
                    newState.CountryId = 30;
                    newState.StateAbbreviation = state.Key;
                    newState.StateName = state.Value;
                    newState.InsertDate = DateTime.Now;
                    newState.UpdateDate = DateTime.Now;
                    context.States.Add(newState);
                    context.SaveChanges();

                    //Console.WriteLine("State Name: {0} State AB: {1}", state.Value,state.Key);
                }
                //Console.ReadKey();
            }
        }

        private static void CreateJson()
        {
            using (RombeEntities context = new RombeEntities())
            {
                var query = (from c in context.Countries
                            select new 
                            {
                                Id=c.CountryId,
                                Name=c.CountryName
                            }).ToList();

                var json = JsonConvert.SerializeObject(query, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                System.IO.File.WriteAllText(@"D:\test.json", json.ToString());
            }
        }
    }
}
