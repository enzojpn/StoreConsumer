using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace BookStoreApi.Models;

   
    public class Book
    { 
        public string? _id { get; set; }
             
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;
    } 
