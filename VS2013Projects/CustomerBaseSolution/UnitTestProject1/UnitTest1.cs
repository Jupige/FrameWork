using System;
using System.Linq;
using System.ServiceModel;
using CB.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static ICBService _icbService ;

        private static  ICBService ClientService
        {
            get
            {
                if (_icbService == null)
                {

                   var channelFactory = new ChannelFactory<ICBService>("CB.Service.ICBService");
                    
                   _icbService = channelFactory.CreateChannel();
                    
                }
                return _icbService;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            ClientService.SayHello();

            //ClientService.SayHello();
        }
    }
}
