using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CMS_Test.Models;
using Microsoft.Extensions.Configuration;
using adonet.extensions;
using System.Data;
using System.Net.Http;
using System.Net;
using CMS_Test.CModels;

namespace CMS_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase

    {
        private readonly ILogger<StockController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public StockController(ILogger<StockController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connection = new SqlConnection(_configuration["Database:ConnectionString"]);
        }

        // CORS BUG FIX
        [HttpOptions]
        [Route("/api/stock/list")]
        [Route("/api/stock/get")]
        [Route("/api/stock/get/VIN")]
        [Route("/api/stock/get/RegNo")]
        [Route("/api/stock/add")]
        [Route("/api/stock/update")]
        [Route("/api/stock/delete")]
        
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [HttpGet]
        [Route("/api/stock/total")]
        
        public int GetTotalStock()
        {
            int counter = 0;
            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select [id]  from [dbo].[stock]", _connection);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.HasRows)
                {
                    while (select_reader.Read())
                    {
                        counter += 1;
                    }
                }
                select_reader.Close();
            }

            return counter;
        }

            [HttpPost]
        [Route("/api/stock/list")]
        
        public IEnumerable<Stock> List([FromBody] PaginationModel pagination)
        {
            if(pagination.Page <= 0)
            {
                pagination.Page = 1;
            }
            List<Stock> stock_list = new List<Stock>();
            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select [id] ,[reg_no] ,[make] ,[model] ,[model_year] ,[kms] ,[colour] ,[vin] ,[retail_price] ,[cost_price], [DTCreated] ,[DTUpdated] from [dbo].[stock] order by [id] OFFSET @skip ROWS FETCH NEXT 10 ROWS ONLY", _connection);
            select_command.Parameters.AddWithValue("@skip", (pagination.Page - 1) * 10);
            //  pagination.PageNumber;
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {   
                if (select_reader.HasRows)
                {
                    while (select_reader.Read())
                    {
                        Stock newStock = new Stock()
                        {
                            ID = select_reader.GetInt32("id"),
                            RegNumber = select_reader["reg_no"].ToString(),
                            Make = select_reader.GetString("make"),
                            Model = select_reader.GetString("model"),
                            ModelYear = select_reader.GetInt32("model_year"),
                            KMS = select_reader.GetInt32("kms"),
                            Colour = select_reader["reg_no"].ToString(),
                            VIN = select_reader["reg_no"].ToString(),
                            RetailPrice = select_reader.GetInt32("retail_price"),
                            CostPrice = select_reader.GetInt32("cost_price"),
                            DTCreated = select_reader.GetDateTime("DTCreated"),
                            DTUpdated = select_reader.GetDateTime("DTUpdated"),

                            Images = new List<Image>(),
                            Accessories = new List<Accessory>(),
                        };

                        stock_list.Add(newStock);
                    }
                }
                select_reader.Close();
            }

            for (var i = 0; i < stock_list.Count(); i++)
            {

                SqlCommand select_images_command = new SqlCommand("Select [id], [name], [data], [stock_id] from [dbo].[images] WHERE stock_id like @stock_id", _connection);
                select_images_command.Parameters.AddWithValue("@stock_id", stock_list[i].ID);
                using (SqlDataReader select_images_reader = select_images_command.ExecuteReader())
                {
                    if (select_images_reader.HasRows)
                    {
                        while (select_images_reader.Read())
                        {
                            stock_list[i].Images.Add(new Image()
                            {
                                ID = select_images_reader.GetInt32("id"),
                                Name = select_images_reader["name"].ToString(),
                                Data = select_images_reader.GetString("data"),
                                StockID = select_images_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_images_reader.Close();
                }

                SqlCommand select_accessories_command = new SqlCommand("Select [id], [name], [description], [stock_id] from [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
                select_accessories_command.Parameters.AddWithValue("@stock_id", stock_list[i].ID);
                using (SqlDataReader select_accessories_reader = select_accessories_command.ExecuteReader())
                {
                    if (select_accessories_reader.HasRows)
                    {
                        while (select_accessories_reader.Read())
                        {
                            stock_list[i].Accessories.Add(new Accessory()
                            {
                                ID = select_accessories_reader.GetInt32("id"),
                                Name = select_accessories_reader.GetString("name"),
                                Description = select_accessories_reader.GetString("description"),
                                StockID = select_accessories_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_accessories_reader.Close();
                }
            }
            _connection.Close();
            return stock_list.ToArray();
        }

        [HttpPost]
        [Route("/api/stock/get")]
        
        public Stock GetByID([FromBody] GetStockModel getstock)
        {
            Stock newStock = null;

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select [id] ,[reg_no] ,[make] ,[model] ,[model_year] ,[kms] ,[colour] ,[vin] ,[retail_price] ,[cost_price], [DTCreated] ,[DTUpdated] from [dbo].[stock] WHERE id = @id", _connection);
            select_command.Parameters.AddWithValue("@id", getstock.ID);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.HasRows)
                {
                    if (select_reader.Read())
                    {
                        newStock = new Stock()
                        {
                            ID = select_reader.GetInt32("id"),
                            RegNumber = select_reader["reg_no"].ToString(),
                            Make = select_reader.GetString("make"),
                            Model = select_reader.GetString("model"),
                            ModelYear = select_reader.GetInt32("model_year"),
                            KMS = select_reader.GetInt32("kms"),
                            Colour = select_reader["reg_no"].ToString(),
                            VIN = select_reader["reg_no"].ToString(),
                            RetailPrice = select_reader.GetInt32("retail_price"),
                            CostPrice = select_reader.GetInt32("cost_price"),
                            DTCreated = select_reader.GetDateTime("DTCreated"),
                            DTUpdated = select_reader.GetDateTime("DTUpdated"),

                            Images = new List<Image>(),
                            Accessories = new List<Accessory>(),
                        };
                    }
                }
                select_reader.Close();
            }

            if (newStock != null)
            {
                SqlCommand select_images_command = new SqlCommand("Select [id], [name], [data], [stock_id] from [dbo].[images] WHERE stock_id like @stock_id", _connection);
                select_images_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_images_reader = select_images_command.ExecuteReader())
                {
                    if (select_images_reader.HasRows)
                    {
                        while (select_images_reader.Read())
                        {
                            newStock.Images.Add(new Image()
                            {
                                ID = select_images_reader.GetInt32("id"),
                                Name = select_images_reader["name"].ToString(),
                                Data = select_images_reader.GetString("data"),
                                StockID = select_images_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_images_reader.Close();
                }
            }
            if (newStock != null)
            {
                SqlCommand select_accessories_command = new SqlCommand("Select [id], [name], [description], [stock_id] from [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
                select_accessories_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_accessories_reader = select_accessories_command.ExecuteReader())
                {
                    if (select_accessories_reader.HasRows)
                    {
                        while (select_accessories_reader.Read())
                        {
                            newStock.Accessories.Add(new Accessory()
                            {
                                ID = select_accessories_reader.GetInt32("id"),
                                Name = select_accessories_reader.GetString("name"),
                                Description = select_accessories_reader.GetString("description"),
                                StockID = select_accessories_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_accessories_reader.Close();
                }
            }
            _connection.Close();
            return newStock;
        }

        [HttpGet]
        [Route("/api/stock/get/VIN")]
        
        public Stock GetByVIN( string VIN)
        {
            Stock newStock = null;

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select [id] ,[reg_no] ,[make] ,[model] ,[model_year] ,[kms] ,[colour] ,[vin] ,[retail_price] ,[cost_price], [DTCreated] ,[DTUpdated] from [dbo].[stock] WHERE vin like @vin LIMIT 1", _connection);
            select_command.Parameters.AddWithValue("@vin", VIN);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.HasRows)
                {
                    if (select_reader.Read())
                    {
                        newStock = new Stock()
                        {
                            ID = select_reader.GetInt32("id"),
                            RegNumber = select_reader["reg_no"].ToString(),
                            Make = select_reader.GetString("make"),
                            Model = select_reader.GetString("model"),
                            ModelYear = select_reader.GetInt32("model_year"),
                            KMS = select_reader.GetInt32("kms"),
                            Colour = select_reader["reg_no"].ToString(),
                            VIN = select_reader["reg_no"].ToString(),
                            RetailPrice = select_reader.GetInt32("retail_price"),
                            CostPrice = select_reader.GetInt32("cost_price"),
                            DTCreated = select_reader.GetDateTime("DTCreated"),
                            DTUpdated = select_reader.GetDateTime("DTUpdated"),

                            Images = new List<Image>(),
                            Accessories = new List<Accessory>(),
                        };
                    }
                }
                select_reader.Close();
            }

            if (newStock != null)
            {
                SqlCommand select_images_command = new SqlCommand("Select [id], [name], [data], [stock_id] from [dbo].[images] WHERE stock_id like @stock_id", _connection);
                select_images_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_images_reader = select_images_command.ExecuteReader())
                {
                    if (select_images_reader.HasRows)
                    {
                        while (select_images_reader.Read())
                        {
                            newStock.Images.Add(new Image()
                            {
                                ID = select_images_reader.GetInt32("id"),
                                Name = select_images_reader["name"].ToString(),
                                Data = select_images_reader.GetString("data"),
                                StockID = select_images_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_images_reader.Close();
                }
            }

            if (newStock != null)
            {
                SqlCommand select_accessories_command = new SqlCommand("Select [id], [name], [description], [stock_id] from [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
                select_accessories_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_accessories_reader = select_accessories_command.ExecuteReader())
                {
                    if (select_accessories_reader.HasRows)
                    {
                        while (select_accessories_reader.Read())
                        {
                            newStock.Accessories.Add(new Accessory()
                            {
                                ID = select_accessories_reader.GetInt32("id"),
                                Name = select_accessories_reader.GetString("name"),
                                Description = select_accessories_reader.GetString("description"),
                                StockID = select_accessories_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_accessories_reader.Close();
                }
            }
            _connection.Close();
            return newStock;
        }

        [HttpGet]
        [Route("/api/stock/get/RegNo")]
        
        public Stock GetByRegNo( string RegNo)
        {
            Stock newStock = null;

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select [id] ,[reg_no] ,[make] ,[model] ,[model_year] ,[kms] ,[colour] ,[vin] ,[retail_price] ,[cost_price], [DTCreated] ,[DTUpdated] from [dbo].[stock] WHERE reg_no like @reg_no LIMIT 1", _connection);
            select_command.Parameters.AddWithValue("@reg_no", RegNo);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.HasRows)
                {
                    if (select_reader.Read())
                    {
                        newStock = new Stock()
                        {
                            ID = select_reader.GetInt32("id"),
                            RegNumber = select_reader["reg_no"].ToString(),
                            Make = select_reader.GetString("make"),
                            Model = select_reader.GetString("model"),
                            ModelYear = select_reader.GetInt32("model_year"),
                            KMS = select_reader.GetInt32("kms"),
                            Colour = select_reader["reg_no"].ToString(),
                            VIN = select_reader["reg_no"].ToString(),
                            RetailPrice = select_reader.GetInt32("retail_price"),
                            CostPrice = select_reader.GetInt32("cost_price"),
                            DTCreated = select_reader.GetDateTime("DTCreated"),
                            DTUpdated = select_reader.GetDateTime("DTUpdated"),

                            Images = new List<Image>(),
                            Accessories = new List<Accessory>(),
                        };
                    }
                }
                select_reader.Close();
            }
            if (newStock != null)
            {
                SqlCommand select_images_command = new SqlCommand("Select [id], [name], [data], [stock_id] from [dbo].[images] WHERE stock_id like @stock_id", _connection);
                select_images_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_images_reader = select_images_command.ExecuteReader())
                {
                    if (select_images_reader.HasRows)
                    {
                        while (select_images_reader.Read())
                        {
                            newStock.Images.Add(new Image()
                            {
                                ID = select_images_reader.GetInt32("id"),
                                Name = select_images_reader["name"].ToString(),
                                Data = select_images_reader.GetString("data"),
                                StockID = select_images_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_images_reader.Close();
                }
            }
            if (newStock != null)
            {
                SqlCommand select_accessories_command = new SqlCommand("Select [id], [name], [description], [stock_id] from [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
                select_accessories_command.Parameters.AddWithValue("@stock_id", newStock.ID);
                using (SqlDataReader select_accessories_reader = select_accessories_command.ExecuteReader())
                {
                    if (select_accessories_reader.HasRows)
                    {
                        while (select_accessories_reader.Read())
                        {
                            newStock.Accessories.Add(new Accessory()
                            {
                                ID = select_accessories_reader.GetInt32("id"),
                                Name = select_accessories_reader.GetString("name"),
                                Description = select_accessories_reader.GetString("description"),
                                StockID = select_accessories_reader.GetInt32("stock_id"),
                            });
                        }
                    }
                    select_accessories_reader.Close();
                }
            }
            _connection.Close();
            return newStock;
        }

        [HttpPost]
        [Route("/api/stock/add")]

        public SuccessOrFailure Add([FromBody] AddStockModel addstock)
        {
            Boolean canAdd = false;
            // get user based on email and password
            if (addstock.SessionKey.Length <= 0)
            {
                return new SuccessOrFailure()
                {
                    Success = false,
                    Message = "Invalid Session Key",
                };
            }

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select id, email, session_key from [user] where session_key like @session_key", _connection);
            select_command.Parameters.AddWithValue("@session_key", addstock.SessionKey);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.Read())
                {
                    if (select_reader.HasRows)
                    {
                        canAdd = true;
                    }
                }
            }

            // check user access
            if (!canAdd)
            {
                return new SuccessOrFailure()
                {
                    Success = true,
                    Message = "You cannot add stock with this user"
                };
            }
            // store stock into DB
            SqlCommand insert_command = new SqlCommand("INSERT INTO [dbo].[stock] ([reg_no] ,[make] ,[model] ,[model_year] ,[kms] ,[colour] ,[vin] ,[retail_price] ,[cost_price] ,[DTCreated] ,[DTUpdated]) VALUES ( @reg_no, @make, @model, @model_year, @kms, @colour, @vin, @retail_price, @cost_price, @DTCreated, @DTUpdated); SELECT SCOPE_IDENTITY() AS ID; ", _connection);
            insert_command.Parameters.AddWithValue("@reg_no", addstock.Stock.RegNumber);
            insert_command.Parameters.AddWithValue("@make", addstock.Stock.Make);
            insert_command.Parameters.AddWithValue("@model", addstock.Stock.Model);
            insert_command.Parameters.AddWithValue("@model_year", addstock.Stock.ModelYear);
            insert_command.Parameters.AddWithValue("@kms", addstock.Stock.KMS);
            insert_command.Parameters.AddWithValue("@colour", addstock.Stock.Colour);
            insert_command.Parameters.AddWithValue("@vin", addstock.Stock.VIN);
            insert_command.Parameters.AddWithValue("@retail_price", addstock.Stock.RetailPrice);
            insert_command.Parameters.AddWithValue("@cost_price", addstock.Stock.CostPrice);
            insert_command.Parameters.AddWithValue("@DTCreated", DateTime.Now);
            insert_command.Parameters.AddWithValue("@DTUpdated", DateTime.Now);
            insert_command.CommandType = CommandType.Text;
            addstock.Stock.ID = Convert.ToInt32(insert_command.ExecuteScalar());

            foreach (var image in addstock.Stock.Images)
            {
                SqlCommand insert_image_command = new SqlCommand("INSERT INTO [dbo].[images] ([name] ,[data] ,[stock_id]) output INSERTED.ID VALUES (@name ,@data ,@stockID)", _connection);
                insert_image_command.Parameters.AddWithValue("@name", image.Name);
                insert_image_command.Parameters.AddWithValue("@data", image.Data);
                insert_image_command.Parameters.AddWithValue("@stockID", addstock.Stock.ID);
                insert_image_command.CommandType = CommandType.Text;
                insert_image_command.ExecuteNonQuery();
            }
            foreach (var accessory in addstock.Stock.Accessories)
            {
                SqlCommand insert_image_command = new SqlCommand("INSERT INTO [dbo].[accessory] ([name] ,[description] ,[stock_id]) VALUES (@name ,@description ,@stockID)", _connection);
                insert_image_command.Parameters.AddWithValue("@name", accessory.Name);
                insert_image_command.Parameters.AddWithValue("@description", accessory.Description);
                insert_image_command.Parameters.AddWithValue("@stockID", addstock.Stock.ID);
                insert_image_command.CommandType = CommandType.Text;
                insert_image_command.ExecuteNonQuery();
            }
            _connection.Close();
            return new SuccessOrFailure()
            {
                Success = true,
                Message = "Stock Added Suiccesfully"
            };
        }

        [HttpPost]
        [Route("/api/stock/update")]
        
        public SuccessOrFailure Update([FromBody] UpdateStockModel updatestock)
        {
            Boolean canAdd = false;
            // get user based on email and password
            if (updatestock.SessionKey.Length <= 0)
            {
                return new SuccessOrFailure()
                {
                    Success = false,
                    Message = "Invalid Session Key",
                };
            }

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select id, email, session_key from [user] where session_key like @session_key", _connection);
            select_command.Parameters.AddWithValue("@session_key", updatestock.SessionKey);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.Read())
                {
                    if (select_reader.HasRows)
                    {
                        canAdd = true;
                    }
                }
            }

            // check user access
            if (!canAdd)
            {
                return new SuccessOrFailure()
                {
                    Success = true,
                    Message = "You cannot add stock with this user"
                };
            }
            // store stock into DB
            SqlCommand update_command = new SqlCommand("UPDATE [dbo].[stock] SET [reg_no] = @reg_no ,[make] = @make,[model] = @model,[model_year] = @model_year,[kms] = @kms,[colour] = @colour,[vin] = @vin,[retail_price] = @retail_price,[cost_price] = @cost_price, [DTUpdated]= @DTUpdated WHERE id like @id", _connection);
            update_command.Parameters.AddWithValue("@id", updatestock.Stock.ID);
            update_command.Parameters.AddWithValue("@reg_no", updatestock.Stock.RegNumber);
            update_command.Parameters.AddWithValue("@make", updatestock.Stock.Make);
            update_command.Parameters.AddWithValue("@model", updatestock.Stock.Model);
            update_command.Parameters.AddWithValue("@model_year", updatestock.Stock.ModelYear);
            update_command.Parameters.AddWithValue("@kms", updatestock.Stock.KMS);
            update_command.Parameters.AddWithValue("@colour", updatestock.Stock.Colour);
            update_command.Parameters.AddWithValue("@vin", updatestock.Stock.VIN);
            update_command.Parameters.AddWithValue("@retail_price", updatestock.Stock.RetailPrice);
            update_command.Parameters.AddWithValue("@cost_price", updatestock.Stock.CostPrice);
            update_command.Parameters.AddWithValue("@DTUpdated", DateTime.Now);
            update_command.CommandType = CommandType.Text;
            update_command.ExecuteNonQuery();


            // delete existing images
            SqlCommand delete_image_command = new SqlCommand("DELETE FROM [dbo].[images] WHERE stock_id like @stock_id", _connection);
            delete_image_command.Parameters.AddWithValue("@stock_id", updatestock.Stock.ID);
            delete_image_command.ExecuteNonQuery();

            // delete existing accessories
            SqlCommand delete_accessories_command = new SqlCommand("DELETE FROM [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
            delete_accessories_command.Parameters.AddWithValue("@stock_id", updatestock.Stock.ID);
            delete_accessories_command.ExecuteNonQuery();

            foreach (var image in updatestock.Stock.Images)
            {
                SqlCommand insert_image_command = new SqlCommand("INSERT INTO [dbo].[images] ([name] ,[data] ,[stock_id]) VALUES (@name ,@data ,@stockID)", _connection);
                insert_image_command.Parameters.AddWithValue("@name", image.Name);
                insert_image_command.Parameters.AddWithValue("@data", image.Data);
                insert_image_command.Parameters.AddWithValue("@stockID", updatestock.Stock.ID);
                insert_image_command.CommandType = CommandType.Text;
                insert_image_command.ExecuteNonQuery();
            }
            foreach (var accessory in updatestock.Stock.Accessories)
            {
                SqlCommand insert_image_command = new SqlCommand("INSERT INTO [dbo].[images] ([name] ,[description] ,[stock_id]) VALUES (@name ,@description ,@stockID)", _connection);
                insert_image_command.Parameters.AddWithValue("@name", accessory.Name);
                insert_image_command.Parameters.AddWithValue("@description", accessory.Description);
                insert_image_command.Parameters.AddWithValue("@stockID", updatestock.Stock.ID);
                insert_image_command.CommandType = CommandType.Text;
                insert_image_command.ExecuteNonQuery();
            }
            _connection.Close();

            return new SuccessOrFailure()
            {
                Success = true,
                Message = "Stock updated Suiccesfully"
            };
        }

        [HttpPost]
        [Route("/api/stock/delete")]
        
        public SuccessOrFailure Delete([FromBody] DeleteStockModel deletestock)
        {
            Boolean canDelete = false;
            // get user based on email and password
            if (deletestock.SessionKey.Length <= 0)
            {
                return new SuccessOrFailure()
                {
                    Success = false,
                    Message = "Invalid Session Key",
                };
            }

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select id, email, session_key from [user] where session_key like @session_key", _connection);
            select_command.Parameters.AddWithValue("@session_key", deletestock.SessionKey);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.Read())
                {
                    if (select_reader.HasRows)
                    {
                        canDelete = true;
                    }
                }
            }

            // check user access
            if (!canDelete)
            {
                return new SuccessOrFailure()
                {
                    Success = true,
                    Message = "You cannot delete stock with this user"
                };
            }


            // delete images
            SqlCommand delete_image_command = new SqlCommand("DELETE FROM [dbo].[images] WHERE stock_id like @stock_id", _connection);
            delete_image_command.Parameters.AddWithValue("@stock_id", deletestock.ID);
            delete_image_command.ExecuteNonQuery();

            // delete accessories
            SqlCommand delete_accessories_command = new SqlCommand("DELETE FROM [dbo].[accessory] WHERE stock_id like @stock_id", _connection);
            delete_accessories_command.Parameters.AddWithValue("@stock_id", deletestock.ID);
            delete_accessories_command.ExecuteNonQuery();

            // delete stock
            SqlCommand delete_stock_command = new SqlCommand("DELETE FROM [dbo].[stock] WHERE id like @id", _connection);
            delete_stock_command.Parameters.AddWithValue("@id", deletestock.ID);
            delete_stock_command.ExecuteNonQuery();


            return new SuccessOrFailure()
            {
                Success = true,
                Message = "Stock and Associated data Deleted Successfully"
            };
        }
    }
}