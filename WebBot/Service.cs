using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBot
{
    public class Service
    {
        public static ChromeDriver drv = new ChromeDriver();
        public static List<HtmlNode> product2 = new List<HtmlNode>();
        public static string category = "";
        public static List<WebProduct> GetProducts()
        {
            List<WebProduct> products = new List<WebProduct>();
            string mainUrl = "Buraya URL Eklenir";
            List<string> categories = new List<string>();


            categories.Add("Buraya kategori urli eklenir");
         

            foreach (var cat in categories)
            {
                var doc = new HtmlDocument();
                string page = "&currentPageNumber=3";
                if (cat == "Y")
                {
                    category = "X";
                }
               
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    drv.Navigate().GoToUrl(mainUrl + cat);
                    drv.Manage().Window.Maximize();
                    Thread.Sleep(3000);
                    IJavaScriptExecutor js = (IJavaScriptExecutor)drv;
                    //js.ExecuteScript("script to execute");
                    js.ExecuteScript("window.scrollBy(0,8000)", "");
                    Thread.Sleep(1000);
                    var p = drv.PageSource;
                    doc.LoadHtml(p);
                    int pagecount = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//span[@class='totalItemCount']").InnerText.Trim());
                    var product = doc.DocumentNode.SelectNodes("//div[@id='divProductList']//div[@class='col-lg-3 col-xs-6']").ToList();

                Again:
                    Thread.Sleep(3000);
                    if (pagecount != product2.Count())
                    {
                        js.ExecuteScript("window.scrollBy(0,8000)", "");
                        Thread.Sleep(2000);
                        var p1 = drv.PageSource;
                        doc.LoadHtml(p1);
                        product2 = doc.DocumentNode.SelectNodes("//div[@id='divProductList']//div[@class='col-lg-3 col-xs-6']").ToList();
                        goto Again;
                    }



                    foreach (var item in product2)
                    {

                        var wp = new WebProduct
                        {
                            StoreName = "Farmasi",
                            RequestTime = DateTime.Now,
                            Category = category

                        };

                        try
                        {
                            wp.NewUrl =  item.ChildNodes[1].ChildNodes[3].ChildNodes[1].Attributes[0].Value;
                        }
                        catch (Exception)
                        {
                            try
                            {
                                wp.NewUrl = item.ChildNodes[0].ChildNodes[4].ChildNodes[0].Attributes[1].Value;
                            }
                            catch (Exception)
                            {
                                wp.NewUrl = "";
                            }

                        }
                        try
                        {
                            wp.Sku = item.ChildNodes[1].ChildNodes[7].ChildNodes[1].ChildNodes[1].InnerText.Trim();
                        }
                        catch (Exception)
                        {
                            try
                            {
                                wp.Sku = item.ChildNodes[0].ChildNodes[3].ChildNodes[0].Attributes[2].Value;
                            }
                            catch (Exception)
                            {

                                wp.Sku = "";
                            }
                        }
                        try
                        {
                            wp.Price = Convert.ToDouble(item.ChildNodes[1].ChildNodes[9].ChildNodes[3].ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText.Trim().Replace("₺", ""));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                wp.Price = Convert.ToDouble(item.ChildNodes[0].ChildNodes[4].ChildNodes[1].ChildNodes[0].InnerText.Trim().Replace("₺", ""));
                            }
                            catch (Exception)
                            {
                                wp.Price = 0;
                            }
                        }
                        try
                        {
                            wp.Barcode = item.ChildNodes[1].ChildNodes[7].ChildNodes[1].ChildNodes[5].InnerText.Trim();
                        }
                        catch (Exception)
                        {
                            try
                            {
                                wp.Barcode = item.ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].InnerText.Trim();
                            }
                            catch (Exception)
                            {
                                wp.Barcode = "";
                            }

                        }
                        if (wp.Price != 0)
                        {

                            products.Add(wp);
                            Console.WriteLine(products.Count + " adet ürün bulundu");
                        }
                    }
                }

            }
            products.Distinct();
            drv.Close();
            return products;
        }
    }
}
