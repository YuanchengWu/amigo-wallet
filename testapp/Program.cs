using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmigoWalletDAL;

namespace testapp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dal = new AmigoWalletRepository();
            //User usr = new User();
            //usr.Name = "g";
            //usr.EmailId = "g@gmail.com";
            //usr.Password = "1234";
            //usr.MobileNumber = "1115550000";
            //Console.WriteLine(""+dal.RegisterUser(usr).ToString());

            var repobj = new AmigoWalletRepository();
            var transList = repobj.ViewUserTransaction("Justin@gmail.com", new DateTime(2017, 7, 1), new DateTime(2017, 7, 2));
            if (transList.Any())
            {
                foreach (var transaction in transList)
                {
                    Console.WriteLine(transaction.Amount.ToString());
                }
            }
            else
            {
                Console.WriteLine("nothing");
            }
        }
    }
}
