using Alice.ResponseLogic.SpecialHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Alice.ResponseLogic
{
    public class AliceHandler
    {
        public string Process(AliceRequest aliceRequest)
        {
            if(aliceRequest.Request.Command == "привет")
            {
                return "Пока";
            }
            return HelloHandler.Process();
        }
    }
}
