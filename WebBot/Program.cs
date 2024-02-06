using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebBot 
{
   class Program
    {
        static void Main(string[] args)
        {
            List<WebProduct> webProducts = Service.GetProducts();
            ConvertToDatatable.ConvertToDatatables(webProducts);

        }
    }
}