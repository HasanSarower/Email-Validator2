using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CheckingEmailValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailValidation emailValidation = new EmailValidation();

            Console.WriteLine("Enter an Email Address-");
            string emailInput = Console.ReadLine();
            string[] emailDomain = emailInput.Split('@');

            bool Check = emailValidation.CheckRecord(emailDomain[1]);

            if(Check == true)
            {
                emailValidation.ValidateMail(emailInput);
            }
            else
            {
                Console.WriteLine("Email Domain is Invalid!!!");
            }

            Console.ReadLine();
        }
    }
}
