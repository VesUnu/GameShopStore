using GameShopStore.Core.Dtos.ProductDtos;
using GameShopStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.UnitTests.DataTests
{
    public class Data
    {
        public static List<ProductSearchDto> ProductSearchDto()
        {
            return new List<ProductSearchDto>()
            {
                new ProductSearchDto()
                {
                    Id = 1,
                    Name = "Cyberpunk 2077",
                    Price = 59.99M,
                    CategoryName = "PS4",
                    Picture = new Picture()
                    {
                        Id = 1,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("\"2022-04-14")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 2,
                    Name = "Anno 1800",
                    Price = 59.99M,
                    CategoryName = "PC",
                    Picture = new Picture()
                    {
                        Id = 2,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-04-24")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 3,
                    Name = "Resident Evil Village",
                    Price = 39.99M,
                    CategoryName = "PS4",
                    Picture = new Picture()
                    {
                        Id = 3,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-04-27")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 4,
                    Name = "FIFA 23",
                    Price = 69.99M,
                    CategoryName = "XONE",
                    Picture = new Picture()
                    {
                        Id = 4,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-05-15")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 5,
                    Name = "Red Dead Redemption 2",
                    Price = 55.15M,
                    CategoryName = "XONE",
                    Picture = new Picture()
                    {
                        Id = 5,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-07-25")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 6,
                    Name = "Forza Horizon 4",
                    Price = 31.99M,
                    CategoryName = "PS4",
                    Picture = new Picture()
                    {
                        Id = 6,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-08-21")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 7,
                    Name = "Final Fantasy VII Remake Integrade",
                    Price = 40.59M,
                    CategoryName = "PC",
                    Picture = new Picture()
                    {
                        Id = 7,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-09-05")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 8,
                    Name = "Tom Clancy's Splinter Cell",
                    Price = 6.99M,
                    CategoryName = "PC",
                    Picture = new Picture()
                    {
                        Id = 8,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-09-26")
                    }
                },
                new ProductSearchDto()
                {
                    Id = 9,
                    Name = "Grand Theft Auto V",
                    Price = 6.99M,
                    CategoryName = "PS4",
                    Picture = new Picture()
                    {
                        Id = 9,
                        Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                        isMain = true,
                        DateAdded = DateTime.Parse("2022-10-28")
                    }
                }
            };
        }
        public static List<ProductModerationDto> ProductModerationDto()
        {
            return new List<ProductModerationDto>()
            {
                new ProductModerationDto()
                {
                    Id = 1,
                    Name = "Cyberpunk 2077",
                    Price = 59.99M,
                    CategoryName = "PS4",
                },
                new ProductModerationDto()
                {
                    Id = 2,
                    Name = "Anno 1800",
                    Price = 59.99M,
                    CategoryName = "PC",

                },
                new ProductModerationDto()
                {
                    Id = 3,
                    Name = "Resident Evil Village",
                    Price = 39.99M,
                    CategoryName = "PS4",

                },
                new ProductModerationDto()
                {
                    Id = 4,
                    Name = "FIFA 23",
                    Price = 69.99M,
                    CategoryName = "XONE",

                },
                new ProductModerationDto()
                {
                    Id = 5,
                    Name = "Red Dead Redemption 2",
                    Price = 55.15M,
                    CategoryName = "XONE",

                },
                new ProductModerationDto()
                {
                    Id = 6,
                    Name = "Forza Horizon 4",
                    Price = 33.02M,
                    CategoryName = "XONE",

                },
                new ProductModerationDto()
                { 
                    Id = 7,
                    Name = "Final Fantasy VII Remake Integrade",
                    Price = 40.59M,
                    CategoryName = "PC",

                },
                new ProductModerationDto()
                {
                    Id = 8,
                    Name = "Tom Clancy's Splinter Cell",
                    Price = 6.99M,
                    CategoryName = "PC",

                },
                new ProductModerationDto()
                {
                    Id = 9,
                    Name = "Grand Theft Auto V",
                    Price = 6.99M,
                    CategoryName = "PS4",

                }
            };
        }

        public static IEnumerable<Product> Product()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Cyberpunk 2077",
                    Price = 59.99M,
                    IsDigitalMedia = true,
                    Pegi = 18,
                    ReleaseDate = DateTime.Parse("2020-12-10"),
                    Description = "Cyberpunk 2077 is an open-world, action-adventure RPG set in the dark future of Night City - a dangerous megalopolis obsessed with power, glamor, and ceaseless body modification.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 1,
                        OS = "Windows 7/8/10",
                        Processor = "Intel Core i5-3570K / AMD FX-8310",
                        RAM = 6,
                        GraphicsCard = "Nvidia GeForce GTX 780 3GB / AMD Radeon RX 470",
                        HDD = 50,
                        IsNetworkConnectionRequire = true,
                        ProductId = 1
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 1,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-04-14"),
                            ProductId = 1
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                           ProductId = 1,
                           SubCategoryId = 7
                        }
                    }
                },
                new Product()
                {
                    Id = 2,
                    Name = "Anno 1800",
                    Price = 59.99M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2019-04-01"),
                    Description = "Lead the Industrial Revolution! Welcome to the dawn of the Industrial Age. The path you choose will define your world. Are you an innovator or an exploiter? A conqueror or a liberator? How the world remembers your name is up to you.\r\n",
                    Category = new Category()
                    {
                        Id = 2
                    },
                    Requirements = new Requirements()
                    {
                        Id = 2,
                        OS = "Windows 7/8/10",
                        Processor = "Intel Core i5-3470 / AMD FX 6350",
                        RAM = 8,
                        GraphicsCard = "NVIDIA GeForce 670 GTX / AMD Radeon R9 285",
                        HDD = 30,
                        IsNetworkConnectionRequire = true,
                        ProductId = 2
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 2,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-04-24"),
                            ProductId = 2
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 2,
                            SubCategoryId = 8
                        }
                    }
                },
                new Product()
                {
                    Id = 3,
                    Name = "Resident Evil Village",
                    Price = 39.99M,
                    IsDigitalMedia = true,
                    Pegi = 18,
                    ReleaseDate = DateTime.Parse("2017-06-01"),
                    Description = "Experience survival horror like never before in the 8th major installment in the Resident Evil franchise - Resident Evil Village. With detailed graphics, intense first-person action and masterful storytelling, the terror has never felt more realistic.\r\n",
                    Category = new Category()
                    {
                        Id = 3
                    },
                    Requirements = new Requirements()
                    {
                        Id = 3,
                        OS = "Windows 10",
                        Processor = "AMD Ryzen 3 1200 / Intel Core i5-7500",
                        RAM = 8,
                        GraphicsCard = "AMD Radeon RX 560 4GB / Nvidia GeForce GTX 1050 Ti 4GB",
                        HDD = 10,
                        IsNetworkConnectionRequire = true,
                        ProductId = 3
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 3,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-04-27"),
                            ProductId = 3
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 3,
                            SubCategoryId = 3
                        }
                    }
                },
                new Product()
                {
                    Id = 4,
                    Name = "FIFA 23",
                    Price = 42.02M,
                    IsDigitalMedia = true,
                    Pegi = 3,
                    ReleaseDate = DateTime.Parse("2022-09-30"),
                    Description = "Experience the excitement of the biggest tournament in football with EA SPORTS FIFA 23 and the men's FIFA World Cup update, available on November 9 at no additiona cost!\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 4,
                        OS = "Windows 10",
                        Processor = "AMD Ryzen 7 2700X / Intel Core i7 6700",
                        RAM = 12,
                        GraphicsCard = "AMD Radeon RX 5600 XT / NVIDIA GeForce GTX 1660",
                        HDD = 50,
                        IsNetworkConnectionRequire = true,
                        ProductId = 4
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 4,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-05-15"),
                            ProductId = 4
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 4,
                            SubCategoryId = 8
                        }
                    }
                },
                new Product()
                {
                    Id = 5,
                    Name = "Red Dead Redemption 2",
                    Price = 55.15M,
                    IsDigitalMedia = true,
                    Pegi = 18,
                    ReleaseDate = DateTime.Parse("2019-12-05"),
                    Description = "Winner of over 175 Game of the Year Awards and recipient of over 250 perfect scores, RDR2 is the epic tale of outlaw Arthur Morgan and the infamous Van der Linde gang, on the run across America at the dawn of the modern age. Also includes access to the shared living world of Red Dead Online.\r\n",
                    Category = new Category()
                    {
                        Id = 2
                    },
                    Requirements = new Requirements()
                    {
                        Id = 5,
                        OS = "Windows 7/8/10",
                        Processor = "Intel Core i5-2500K / AMD FX-6300",
                        RAM = 8,
                        GraphicsCard = "Nvidia GeForce GTX 770 2GB / AMD Radeon R9 280",
                        HDD = 50,
                        IsNetworkConnectionRequire = true,
                        ProductId = 5
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 5,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-07-25"),
                            ProductId = 5
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 5,
                            SubCategoryId = 4
                        }
                    }
                },
                new Product()
                {
                    Id = 6,
                    Name = "Forza Horizon 4",
                    Price = 31.99M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2017-08-21"),
                    Description = "Dynamic seasons change everything at the world’s greatest automotive festival. Go it alone or team up with others to explore beautiful and historic Britain in a shared open world.\r\n",
                    Category = new Category()
                    {
                        Id = 3
                    },
                    Requirements = new Requirements()
                    {
                        Id = 6,
                        OS = "None",
                        Processor = "None",
                        RAM = 0,
                        GraphicsCard = "None",
                        HDD = 30,
                        IsNetworkConnectionRequire = true,
                        ProductId = 6
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 6,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-08-21"),
                            ProductId = 6
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 6,
                            SubCategoryId = 6
                        }
                    }
                },
                new Product()
                {
                    Id = 7,
                    Name = "Final Fantasy VII Remake Integrade",
                    Price = 40.59M,
                    IsDigitalMedia = true,
                    Pegi = 16,
                    ReleaseDate = DateTime.Parse("2022-06-17"),
                    Description = "Cloud Strife, an ex-SOLDIER operative, descends on the mako-powered city of Midgar. The world of the timeless classic FINAL FANTASY VII is reborn, using cutting-edge graphics technology, a new battle system and an additional adventure featuring Yuffie Kisaragi.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                       Id = 7,
                       OS = "Windows 10",
                       Processor = "AMD FX-8350 / Intel Core™ i5-3330",
                       RAM = 8,
                       GraphicsCard = "AMD Radeon™ RX 480 / NVIDIA GeForce GTX 780",
                       HDD = 30,
                       IsNetworkConnectionRequire = true,
                       ProductId = 7
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 7,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-09-05"),
                            ProductId = 7
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 7,
                            SubCategoryId = 7
                        }
                    }
                },
                new Product()
                {
                    Id = 8,
                    Name = "Tom Clancy's Splinter Cell",
                    Price = 6.99M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2003-02-18"),
                    Description = "Infiltrate terrorists' positions, acquire critical intelligence by any means necessary, execute with extreme prejudice, and exit without a trace! You are Sam Fisher, a highly trained secret operative of the NSA's secret arm: Third Echelon.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 8,
                        OS = "None",
                        Processor = "None",
                        RAM = 0,
                        GraphicsCard = "None",
                        HDD = 30,
                        IsNetworkConnectionRequire = true,
                        ProductId = 8
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 8,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-09-26"),
                            ProductId = 8
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                             ProductId = 8,
                             SubCategoryId = 1
                        }
                    }
                },
                new Product()
                {
                    Id = 9,
                    Name = "Grand Theft Auto V",
                    Price = 6.99M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2015-04-15"),
                    Description = "Grand Theft Auto V for PC offers players the option to explore the award-winning world of Los Santos and Blaine County in resolutions of up to 4k and beyond, as well as the chance to experience the game running at 60 frames per second.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                         Id = 9,
                         OS = "Windows 7/8/10",
                         Processor = "Intel Core i5 3470 3.2GHz / AMD X8 FX-8350 4GHz",
                         RAM = 0,
                         GraphicsCard = "NVIDIA GTX 660 2GB / AMD HD 7870 2GB",
                         HDD = 60,
                         IsNetworkConnectionRequire = true,
                         ProductId = 9
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 9,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2022-10-28"),
                            ProductId = 7
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 9,
                            SubCategoryId = 4
                        }
                    }
                }
            };
        }
    }
}
