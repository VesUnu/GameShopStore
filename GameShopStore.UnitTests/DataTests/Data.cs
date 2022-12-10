﻿using GameShopStore.Core.Dtos.ProductDtos;
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
                    Name = "The Witcher 3 Wild Hunt",
                    Price = 48.82M,
                    IsDigitalMedia = true,
                    Pegi = 18,
                    ReleaseDate = DateTime.Parse("2017-03-31"),
                    Description = "Nulla amet commodo minim esse adipisicing commodo sint esse laboris adipisicing. Officia Lorem laboris ipsum labore mollit ipsum est enim elit exercitation quis deserunt. Nostrud dolore ut sint est ut officia voluptate consequat mollit. Nulla cupidatat mollit dolore non consequat amet Lorem. Magna dolor veniam anim aliquip aliquip esse consequat velit veniam tempor in.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 1,
                        OS = "Windows 7/8/10",
                        Processor = "Intel Core i7 4790 3.6 GHz / AMD FX-9590 4.7 GHz",
                        RAM = 6,
                        GraphicsCard = "NVIDIA GeForce RTX 2080Ti 11GB / AMD Radeon RX 5700XT 8GB",
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
                            DateAdded = DateTime.Parse("2020-04-07"),
                            ProductId = 1
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 1,
                            SubCategoryId = 1
                        }
                    }
                },
                new Product()
                {
                    Id = 2,
                    Name = "Assassin’s Creed Odyssey",
                    Price = 26.81M,
                    IsDigitalMedia = false,
                    Pegi = 16,
                    ReleaseDate = DateTime.Parse("2017-07-31"),
                    Description = "Exercitation occaecat esse sunt elit adipisicing magna quis laborum. Sunt consequat nulla minim labore. Laborum ut irure cupidatat et ullamco minim occaecat id consequat officia non. Deserunt incididunt ea qui incididunt. Duis laborum proident do nulla anim laboris eiusmod incididunt velit.\r\n",
                    Category = new Category()
                    {
                        Id = 2
                    },
                    Requirements = new Requirements()
                    {
                        Id = 2,
                        OS = "None",
                        Processor = "None",
                        RAM = 0,
                        GraphicsCard = "None",
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
                            DateAdded = DateTime.Parse("2020-06-28"),
                            ProductId = 2
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 2,
                            SubCategoryId = 1
                        }
                    }
                },
                new Product()
                {
                    Id = 3,
                    Name = "Battlefield V",
                    Price = 34.82M,
                    IsDigitalMedia = true,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2017-06-01"),
                    Description = "Voluptate ut in commodo eu dolore aliquip ex. Pariatur velit laborum anim cillum et sit irure sit. Ipsum cillum officia ipsum irure consectetur irure occaecat deserunt aliquip esse consectetur eu. Cupidatat sit consequat magna sit pariatur consequat. Enim labore commodo nisi sunt commodo.\r\n",
                    Category = new Category()
                    {
                        Id = 3
                    },
                    Requirements = new Requirements()
                    {
                        Id = 3,
                        OS = "None",
                        Processor = "None",
                        RAM = 0,
                        GraphicsCard = "None",
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
                            DateAdded = DateTime.Parse("2020-03-31"),
                            ProductId = 3
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 3,
                            SubCategoryId = 2
                        }
                    }
                },
                new Product()
                {
                    Id = 4,
                    Name = "Layers of Fear",
                    Price = 42.02M,
                    IsDigitalMedia = true,
                    Pegi = 18,
                    ReleaseDate = DateTime.Parse("2017-05-16"),
                    Description = "Ex consectetur nisi id laborum laboris. Officia eu culpa nisi sint incididunt tempor consequat reprehenderit cillum proident minim laboris. Eiusmod proident nulla laboris eiusmod excepteur fugiat adipisicing voluptate aliqua sunt anim est. Non tempor duis veniam et consequat ipsum ullamco. Culpa aute dolor commodo amet proident deserunt consequat pariatur reprehenderit officia.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 4,
                        OS = "Windows 10",
                        Processor = "Intel Core i7 4790 3.6 GHz / AMD FX-9590 4.7 GHz",
                        RAM = 2,
                        GraphicsCard = "NVIDIA GeForce GTX 780 3GB / AMD Radeon R9 290X 4GB",
                        HDD = 10,
                        IsNetworkConnectionRequire =  true,
                        ProductId = 4
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 4,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2020-01-03"),
                            ProductId = 4
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 4,
                            SubCategoryId = 3
                        }
                    }
                },
                new Product()
                {
                    Id = 5,
                    Name = "The Last of Us",
                    Price = 55.15M,
                    IsDigitalMedia = true,
                    Pegi = 16,
                    ReleaseDate = DateTime.Parse("2017-07-30"),
                    Description = "Velit in ea aliqua sint veniam fugiat eiusmod. Incididunt cillum do pariatur cillum dolore occaecat ad. Minim aute laborum ex dolore. Ea exercitation minim et nostrud in elit eu esse amet eiusmod. Ad ut nostrud qui consectetur sunt consequat magna magna labore qui.\r\n",
                    Category = new Category()
                    {
                        Id = 2
                    },
                    Requirements = new Requirements()
                    {
                        Id = 5,
                        OS = "None",
                        Processor = "None",
                        RAM = 0,
                        GraphicsCard = "None",
                        HDD = 30,
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
                            DateAdded = DateTime.Parse("2020-07-01"),
                            ProductId = 5
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 5,
                            SubCategoryId = 1
                        },
                        new ProductSubCategory()
                        {
                            ProductId = 5,
                            SubCategoryId = 7
                        }
                    }
                },
                new Product()
                {
                    Id = 6,
                    Name = "Forza Horizon 4",
                    Price = 33.02M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2017-08-21"),
                    Description = "Incididunt ullamco quis eu consectetur. Elit nostrud ipsum amet minim non nostrud ipsum dolore magna magna. Ad deserunt elit velit esse aliqua quis proident in cupidatat quis. Ullamco ut in labore ad tempor aliqua aute quis amet occaecat irure. Amet deserunt velit ut ipsum ad anim mollit reprehenderit ea ipsum.\r\n",
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
                            DateAdded = DateTime.Parse("2020-01-13"),
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
                    Name = "Might & Magic: Heroes VII",
                    Price = 5.53M,
                    IsDigitalMedia = false,
                    Pegi = 12,
                    ReleaseDate = DateTime.Parse("2017-06-07"),
                    Description = "Incididunt minim excepteur adipisicing Lorem labore irure incididunt proident sint id qui. Culpa exercitation adipisicing minim sit elit magna nisi pariatur do sint minim irure. Ut do nisi in fugiat aliquip proident. Eiusmod elit et aliquip consectetur eu irure.\r\n",
                    Category = new Category()
                    {
                        Id = 1
                    },
                    Requirements = new Requirements()
                    {
                        Id = 7,
                        OS = "Windows 8/10",
                        Processor = "Intel Core i5 4690 3.3 GHz / AMD Ryzen 5 3600x 3.8 GHz",
                        RAM = 16,
                        GraphicsCard = "NVIDIA GeForce RTX 2080Ti 11GB / AMD Radeon RX 5700XT 8GB",
                        HDD = 30,
                        IsNetworkConnectionRequire =  true,
                        ProductId = 7
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture()
                        {
                            Id = 7,
                            Url = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png",
                            isMain = true,
                            DateAdded = DateTime.Parse("2020-07-17"),
                            ProductId = 7
                        }
                    },
                    SubCategories = new List<ProductSubCategory>
                    {
                        new ProductSubCategory()
                        {
                            ProductId = 7,
                            SubCategoryId = 5
                        }
                    }
                }
            };
        }
    }
}
