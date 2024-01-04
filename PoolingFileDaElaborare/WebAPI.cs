using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    static public class WebAPI
    {
        static HttpClient client = new HttpClient();


        //static async Task<VociOrdine> GetProductAsync(string path)
        //{
        //    VociOrdine products = null;
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        products = await response.Content.RunAsync(ConveertiInVoceOrdine());
        //    }
        //    return products;
        //}

        //private static object ConveertiInVoceOrdine()
        //{
        //    throw new NotImplementedException();
        //}

        //static async Task RunAsync()
        //{
        //    // Update port # in the following line.
        //    client.BaseAddress = new Uri("http://localhost:64195/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //    try
        //    {
        //        // Create a new product
        //        VociOrdine product = new VociOrdine
        //        {
        //            CodProdotto = "1234",
        //            DescrizioneProdotto = "prodotto test",
        //            Note = "questo è un test"
        //        };

        //        var url = await GetProductAsync(product);
        //        Console.WriteLine($"Created at {url}");

        //        // Get the product
        //        product = await GetProductAsync(url.PathAndQuery);
        //        ShowProduct(product);

        //        // Update the product
        //        Console.WriteLine("Updating price...");
        //        product.Price = 80;
        //        await UpdateProductAsync(product);

        //        // Get the updated product
        //        product = await GetProductAsync(url.PathAndQuery);
        //        ShowProduct(product);

        //        // Delete the product
        //        var statusCode = await DeleteProductAsync(product.Id);
        //        Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    Console.ReadLine();
        //}
    }
}
