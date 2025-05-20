using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitabhChauta.Model
{

    // Enum representing the category of a book

    public enum Category
    {
        BestSeller,
        LatestBooks
    }

    // Author class representing an author entity
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Author_id { get; set; }

        [MaxLength(50)]
        public string Author_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();


    }

    // Genre class representing a genre entity
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Genre_id { get; set; }
        [MaxLength(50)]
        public string Genre_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }


    // Publisher class representing a publisher entity
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Publisher_id { get; set; }
        [MaxLength(50)]
        public string Publisher_Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }




    // Book class representing a book entity
    public class Book
     {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

            [Required(ErrorMessage = "Title is required")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "ISBN is required")]
            public string ISBN { get; set; } = string.Empty;

            [Required(ErrorMessage = "Price is required")]
            public decimal Price { get; set; }

            public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

            public int Pages { get; set; }

            public int StockCount { get; set; }

            public string Synopsis { get; set; } = string.Empty;

            public string CoverImageUrl { get; set; } = string.Empty;

            public bool IsOnSale { get; set; } = false;

            [Range(0, 1, ErrorMessage = "Discount must be between 0 (0%) and 1 (100%)")]
            public decimal? DiscountPercentage { get; set; }

            public Category? Category { get; set; }

            // Foreign Keys
            public int Author_id { get; set; }
            [ForeignKey("Author_id")]
        public Author? Author { get; set; }

        public int Genre_id { get; set; }
            [ForeignKey("Genre_id")]
            public Genre? Genre { get; set; }

            public int Publisher_id { get; set; }
            [ForeignKey("Publisher_id")]
            public Publisher? Publisher { get; set; }
        }


    }
