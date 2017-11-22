using Newtonsoft.Json;
using RomBe.Entities.Class.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using RomBe.Entities.Enums;

namespace RomBe.Logic.Google
{
    public class GoogleLogic
    {
        public async Task<CreateGoogleUserRequest> GetUserDetails(GoogleLoginRequest request)
        {
            var googleUserInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";
            var hc = new HttpClient();
            hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.GoogleToken);
            HttpResponseMessage response = await hc.GetAsync(googleUserInfoUrl);
            dynamic userInfo = await response.Content.ReadAsStringAsync();
            GoogleOAuth2ObjectResponse deserializedResponse = JsonConvert.DeserializeObject<GoogleOAuth2ObjectResponse>(userInfo);

            if (deserializedResponse.IsNull())
            {
                return null;
            }


            CreateGoogleUserRequest returnValue = new CreateGoogleUserRequest();

            returnValue.Email = deserializedResponse.email;
            returnValue.FirstName = deserializedResponse.given_name;
            returnValue.LastName = deserializedResponse.family_name;
            returnValue.GoogleUserId = deserializedResponse.id;


            if (returnValue.Email.IsNull())
            {
                returnValue.Email = returnValue.GoogleUserId + "@google.rombe.me";
            }


            if (deserializedResponse.gender == GenderTypeEnum.Male.ToString().ToLower())
            {
                returnValue.GenderType = GenderTypeEnum.Male;
            }
            else
            {
                returnValue.GenderType = GenderTypeEnum.Female;
            }

            return returnValue;

        }




        public class GoogleOAuth2ObjectResponse
        {
            public string family_name;
            public string name;
            public string gender;
            public string email;
            public string given_name;
            public string id;
        }
    }


}
