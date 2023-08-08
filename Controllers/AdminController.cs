using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonDistributionSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Web.DynamicData;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace PersonDistributionSystem.Controllers
{
    public class AdminController : Controller
    {
        private SqlConnection sqlconnection;

        private void connection()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["ProductConn"].ToString();
            sqlconnection = new SqlConnection(connectionstring);
        }
        public ActionResult Wrong()
        {
            return View();
        }
        
        public ActionResult Details()
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ViewProducts", sqlconnection);
                command.Parameters.AddWithValue("@Action", "View");
                command.CommandType = CommandType.StoredProcedure;
                sqlconnection.Open();
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                DataTable datatable = new DataTable();
                datatable.Load(reader);
                return View(datatable);
            }
        }

        public ActionResult Create()
        {
            return View(new ProductsModel());
        }

        [HttpPost]
        public ActionResult Create(ProductsModel Product)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ProductSCRUD", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Create");
                command.Parameters.AddWithValue("@Item", Product.Item);
                command.Parameters.AddWithValue("@Quantity", Product.Quantity);
                command.Parameters.AddWithValue("@Price", Product.Price);
                sqlconnection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
            return RedirectToAction("Details");
        }

        #region Delete
        public ActionResult Delete(int id)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ProductSCRUD", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Delete");
                command.Parameters.AddWithValue("@Id", id);
                sqlconnection.Open();
                int rowsAffected = command.ExecuteNonQuery();
            }
            return RedirectToAction("Details");
        }
        #endregion

        [HttpGet]
        public ActionResult Edit(int id)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ViewProducts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Product");
                command.Parameters.AddWithValue("@Id", id);
                sqlconnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ProductsModel product = new ProductsModel();
                    product.Id = Convert.ToInt32(reader["Id"]);
                    product.Item = reader["Item"].ToString();
                    product.Quantity = Convert.ToInt32(reader["Quantity"]);
                    product.Price = (int)Convert.ToDecimal(reader["Price"]);
                    reader.Close();
                    return View(product);
                }
            }

            return View();
        }
        [HttpPost]

        public ActionResult Edit(ProductsModel product, int id)
        {
            connection();
            using (sqlconnection)
            {
                sqlconnection.Open();
                using (SqlCommand command = new SqlCommand("ProductSCRUD", sqlconnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Action", "Update");
                    command.Parameters.AddWithValue("@Item", product.Item);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Details");
        }

        public ActionResult Signup()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Signup(LoginModel admin)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("LoginDetails", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Insert");
                command.Parameters.AddWithValue("@Username", admin.Username);
                command.Parameters.AddWithValue("@Password", admin.Password);
                command.Parameters.AddWithValue("@IsAdmin", admin.Isadmin);

                SqlParameter ReturnMessage = new SqlParameter("@ReturnMessage", SqlDbType.Bit);
                ReturnMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(ReturnMessage);

                sqlconnection.Open();
                command.ExecuteNonQuery();
                bool message = (bool)ReturnMessage.Value;

                if (string.IsNullOrWhiteSpace(admin.Password) || string.IsNullOrWhiteSpace(admin.Username))
                {
                    TempData["Message"] = "Invalid username and password.";
                    return RedirectToAction("Signup");
                }

                if (admin.Password.Length < 5 || admin.Password.Length > 10)
                {
                    TempData["Message"] = "Minimum password length is 5 and maximum length is 10.";
                    return RedirectToAction("Signup");
                }

                if (message)
                {
                    TempData["Message"] = "Username already exists.";
                    return RedirectToAction("Signup");
                }

            }
            return RedirectToAction("Login");
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel admin)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("LoginDetails", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Check");
                command.Parameters.AddWithValue("@Username", admin.Username);
                string userName = admin.Username;
                command.Parameters.AddWithValue("@Password", admin.Password);

                SqlParameter IsAdminOutput = new SqlParameter("@IsAdminOutput", SqlDbType.NVarChar, 10);
                IsAdminOutput.Direction = ParameterDirection.Output;
                command.Parameters.Add(IsAdminOutput);

                SqlParameter IsAuthenticOutput = new SqlParameter("@IsAuthenticOutput", SqlDbType.Bit);
                IsAuthenticOutput.Direction = ParameterDirection.Output;
                command.Parameters.Add(IsAuthenticOutput);
                sqlconnection.Open();
                command.ExecuteNonQuery();

                bool isAuthentic = (bool)IsAuthenticOutput.Value;
                string isAdmin = IsAdminOutput.Value.ToString();

                if (isAuthentic)
                {
                    Session["userName"] = userName;

                    if (isAdmin == "Admin")
                    {
                        return RedirectToAction("Details");
                    }
                    else if (isAdmin == "NotAdmin")
                    {
                        return RedirectToAction("Userview");
                    }
                }
                TempData["Message"] = "Invalid username and password";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Userview()
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ViewProducts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "View");
                sqlconnection.Open();
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                DataTable datatable = new DataTable();
                datatable.Load(reader);

                return View(datatable);
            }
        }

        public ActionResult Mycart()
        {
            var userName = (string)Session["userName"];

            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ViewCarts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                sqlconnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable datatable = new DataTable();
                datatable.Load(reader);
                return View(datatable);
            }
        
        }
               
        public ActionResult AddingCart(int id)
        {
            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("ViewProducts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("@Action", "Product");
                sqlconnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CartModel product = new CartModel();
                    product.Id = Convert.ToInt32(reader["Id"]);
                    product.Item = reader["Item"].ToString();
                    product.Price = (int)Convert.ToDecimal(reader["Price"]);
                    reader.Close();
                    return View(product);

                }           
            }            
            return RedirectToAction("Wrong");
        }

        [HttpPost]

        public ActionResult AddingCart(CartModel product, int id)
        {
            var userName = (string)Session["userName"];

            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("AddingProductsInCarts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "Insert");
                command.Parameters.AddWithValue("@ProductId", id);
                command.Parameters.AddWithValue("@Item", product.Item);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@UserName", userName);
                decimal price = Convert.ToDecimal(product.Price) * Convert.ToInt32(product.Quantity);
                command.Parameters.AddWithValue("@Price", price);
                
                SqlParameter returnQuantityMessage = new SqlParameter("@ReturnQuantityMessage", SqlDbType.NVarChar, 10);
                returnQuantityMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnQuantityMessage);

                sqlconnection.Open();
                command.ExecuteNonQuery();
                
                string quantityMessage = returnQuantityMessage.Value.ToString();


                if (quantityMessage == "Updated")
                {
                    ViewData["Message"] = "The Quantity is greater than our stock";
                }
                else
                {
                    ViewData["Message"] = "Product added to your cart successfully";
                }

                return RedirectToAction("Userview");
            }
        }

       
        public ActionResult RemoveCart(int id)
        {
            connection();

            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("AddingProductsInCarts", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Action", "Delete");
                command.Parameters.AddWithValue("@Id", id);           

                sqlconnection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Mycart");
        }

        public ActionResult CleanCart()
        {
            var userName = (string)Session["userName"];

            connection();
            using (sqlconnection)
            {
                SqlCommand command = new SqlCommand("CheckOut", sqlconnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                sqlconnection.Open();
                command.ExecuteNonQuery();
                return RedirectToAction("Userview");

            }
        }

    }

}

