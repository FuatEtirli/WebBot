using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBot
{
    public class ConvertToDatatable
    {
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string path = string.Empty;
        static string PROJECT_NAME = "WebBot";
        public static void ConvertToDatatables(List<WebProduct> list)
        {
            try
            {
                DataTable dt = new DataTable();

                string date = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 3)
                {
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }

                dt.Columns.Add("StoreName");
                dt.Columns.Add("BudgetCategory");
                dt.Columns.Add("Category");
                dt.Columns.Add("SuperGroup");
                dt.Columns.Add("SubCategory");
                dt.Columns.Add("Brand");
                dt.Columns.Add("SKU");
                dt.Columns.Add("SKUCode");
                dt.Columns.Add("Barcode");
                dt.Columns.Add("UnitCode");
                dt.Columns.Add("Supplier");
                dt.Columns.Add("SupplierMark");
                dt.Columns.Add("Supplier2");
                dt.Columns.Add("Price");
                dt.Columns.Add("Stock");
                dt.Columns.Add("IsStock");
                dt.Columns.Add("CargoDetail");
                dt.Columns.Add("CargoPrice");
                dt.Columns.Add("URL");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("IsStar");

                string botName = "";


                foreach (var item in list.Distinct().ToList())
                {
                    var row = dt.NewRow();

                    botName = item.StoreName;

                    row["StoreName"] = item.StoreName;
                    row["BudgetCategory"] = string.Empty;
                    row["Category"] = item.Category;
                    row["SuperGroup"] = string.Empty;
                    row["SubCategory"] = item.SubCategory;
                    row["Brand"] = item.Brand;
                    row["SKU"] = item.Sku;
                    row["SKUCode"] = item.Unit;
                    row["Barcode"] = item.Barcode;
                    row["UnitCode"] = item.Unit;
                    row["Supplier"] = item.Supplier;
                    row["Price"] = Convert.ToDouble(item.Price);
                    row["Stock"] = item.StockCount;
                    row["IsStock"] = true;
                    row["CargoDetail"] = item.CargoDetail02;
                    row["CargoPrice"] = 0;
                    row["URL"] = item.NewUrl;
                    row["DateTime"] = date;
                    row["IsStar"] = 0;

                    dt.Rows.Add(row);
                }

                try
                {
                    Random r = new Random();

                    path = folderPath + PROJECT_NAME + " " + System.DateTime.Now.ToString("d MMMM yyyy dddd HH.mm.ss") + "-" + r.Next(1, 999999) + ".xlsx";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ErrorHelper error = new ErrorHelper();
                    error.ErrorWriteFile(ex, folderPath, "excelPathError.txt", PROJECT_NAME);

                }

                bool isSuccess = false;

               
                try
                {
                    GC.Collect();
                    Pum_Excel_Management.ExportToExcel(path, dt, true, false);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    ErrorHelper error = new ErrorHelper();
                    error.ErrorWriteFile(ex, folderPath, "excelError.txt", PROJECT_NAME);
                }
 

                System.Diagnostics.Process.Start(folderPath);
                Console.Clear();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
