using System;
using System.IO;

namespace PokemonDex.Challenge.Test.Util
{
    public class LoadJsonFor
    {
        public string ShakespeareTranslationResponseStub()
        {
            var applicationPath = Environment.CurrentDirectory + @"\Resources\Stub\";
            //Stub for API response
            var fileName = applicationPath + @"\Translation_shakespeareStub.json";
            var response = File.ReadAllText(fileName);
            return response;
        }

     
    }
}
