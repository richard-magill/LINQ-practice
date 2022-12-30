using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LINQSamples
{
  public class SamplesViewModel
  {
    #region Constructor
    public SamplesViewModel()
    {
      // Load all Product Data
      Products = ProductRepository.GetAll();
    }
    #endregion

    #region Properties
    public bool UseQuerySyntax { get; set; }
    public List<Product> Products { get; set; }
    public string ResultText { get; set; }
    #endregion

    #region GetAllLooping
    /// <summary>
    /// Put all products into a collection via looping, no LINQ
    /// </summary>
    public void GetAllLooping()
    {
      List<Product> list = new List<Product>();
        foreach (var item in Products)
            {
                list.Add(item);
            }
     
      ResultText = $"Total Products: {list.Count}";
    }
    #endregion

    #region GetAll
    /// <summary>
    /// Put all products into a collection using LINQ
    /// </summary>
    public void GetAll()
    {
      List<Product> list = new List<Product>();

            if (UseQuerySyntax)
            {
                // Query Syntax

                list = (from prod in Products select prod).ToList();
                
            }
            else
            {
                // Method Syntax
                list = Products.Select(prod => prod).ToList();

            }

      ResultText = $"Total Products: {list.Count}";
    }
    #endregion

    #region GetSingleColumn
    /// <summary>
    /// Select a single column
    /// </summary>
    public void GetSingleColumn()
    {
      StringBuilder sb = new StringBuilder(1024);
      List<string> list = new List<string>();

    if (UseQuerySyntax) {
        // Query Syntax
        list.AddRange(from prod in Products 
                        select prod.Name);
      }
      else {
        // Method Syntax
        list.AddRange(Products.Select(prod => prod.Name));
      }

      foreach (string item in list) {
        sb.AppendLine(item);
      }

      ResultText = $"Total Products: {list.Count}" + Environment.NewLine + sb.ToString();
      Products.Clear();
    }
    #endregion

    #region GetSpecificColumns
    /// <summary>
    /// Select a few specific properties from products and create new Product objects
    /// </summary>
    public void GetSpecificColumns()
    {
      if (UseQuerySyntax) {
        // Query Syntax
        Products = (from prod in Products
                    select new Product
                    {
                        ProductID = prod.ProductID,
                        Name = prod.Name,   
                        Size = prod.Size,
                    }).ToList();
      }
      else {
        // Method Syntax
       Products = Products.Select(prod => new Product
       {
           ProductID = prod.ProductID,
           Name = prod.Name,
           Size = prod.Size,
       }).ToList();
      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region AnonymousClass
    /// <summary>
    /// Create an anonymous class from selected product properties
    /// </summary>
    public void AnonymousClass()
    {
      StringBuilder sb = new StringBuilder(2048);

      if (UseQuerySyntax) {
                // Query Syntax
                var products = (from product in Products
                    select new
                    {
                        ProductIdentifier = product.ProductID,
                        ProductName = product.Name,
                        ProductSize = product.Size,

                    });
                // Loop through anonymous class
                foreach (var prod in products)
                {
                    sb.AppendLine($"Product ID: {prod.ProductIdentifier}");
                    sb.AppendLine($"   Product Name: {prod.ProductName}");
                    sb.AppendLine($"   Product Size: {prod.ProductSize}");
                }
            }
      else {
                // Method Syntax
                var products = Products.Select(product => new 
                {
                    Identifier = product.ProductID,
                    ProductName = product.Name,
                    ProductSize = product.Size
                });
                // Loop through anonymous class
                foreach (var prod in products)
                {
                    sb.AppendLine($"Product ID: {prod.Identifier}");
                    sb.AppendLine($"   Product Name: {prod.ProductName}");
                    sb.AppendLine($"   Product Size: {prod.ProductSize}");
                }
            }

      ResultText = sb.ToString();
      Products.Clear();
    }
    #endregion

    #region OrderBy
    /// <summary>
    /// Order products by Name
    /// </summary>
    public void OrderBy()
    {
      if (UseQuerySyntax) {
        // Query Syntax
        Products = (from product in Products
                    orderby product.Name descending
                    select product).ToList();
      }
      else {
                // Method Syntax
                Products = Products.OrderByDescending(product => product.Name).ToList();
      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region OrderByDescending Method
    /// <summary>
    /// Order products by name in descending order
    /// </summary>
    public void OrderByDescending()
    {
      if (UseQuerySyntax) {
        // Query Syntax

      }
      else {
        // Method Syntax

      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

    #region OrderByTwoFields Method
    /// <summary>
    /// Order products by Color descending, then Name
    /// </summary>
    public void OrderByTwoFields()
    {
      if (UseQuerySyntax) {
        // Query Syntax
        Products  = (from product in Products
                     orderby product.Name descending, product.Size
                     select product).ToList();
      }
      else {
        // Method Syntax
        Products = Products.OrderByDescending(product => product.Name).ThenBy(product => product.Size).ToList();
      }

      ResultText = $"Total Products: {Products.Count}";
    }
    #endregion

        ///<summary>WhereExpression</summary>
        public void WhereExpression()
        {
            var search = "L";
            var cost = 100;
            if (UseQuerySyntax)
            {
                //query syntax
                Products = (from product in Products
                            where product.Name.StartsWith(search)
                            && product.ListPrice > cost
                            select product).ToList();
            }
            else
            {
                //method syntax
                Products = Products.Where(product => product.Name.StartsWith(search) && product.ListPrice > cost).ToList();
            }
            ResultText = $"Total Products: {Products.Count}";
        }

        ///<summary>WhereExpression</summary>
        public void First()
        {
            var search = "Red";
            Product value;
            try
            {
                if (UseQuerySyntax)
                {
                    //query syntax
                    value = (from product in Products
                             select product)
                             .Last(prod => prod.Color == search);
                }
                else
                {
                    //method syntax
                    value = Products.Last(product => product.Color == search );
                }
                ResultText = $"Found: {value}";
            }
            catch(Exception ex)
            {
                ResultText = ex.Message;
            }

            
        }

        public void AssignValue()
        {


                if (UseQuerySyntax)
                {
                    //query syntax
                    Products = (from product in Products
                                let tmp = product.NameLength = product.Name.Length
                                select product).ToList();
                }
                else
                {
                    //method syntax
                    Products.ForEach(product => product.NameLength = product.Name.Length);
                }
                ResultText = $"Total Products: {Products.Count}";




        }
    }
}
