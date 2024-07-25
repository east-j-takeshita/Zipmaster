using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using KadaiMVCApp.Models;
using System.IO.Compression;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using Dapper;
using KadaiMVCApp;

namespace KadaiMVCApp.Repository
{
    public class ZipRepository
    {
        private string _connectionstring;

        public ZipRepository()
        {
            _connectionstring = AppSettings.dbConnectString;

        }



        public async Task<List<Zip>> GetZipmaster(string postcode)
        {
            var zips = new List<Zip>();

            //Dapper
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionstring))//builderに対してConnectionStringを使う
            {

                connection.Open();
                Console.WriteLine("接続成功");
                using (var command = connection.CreateCommand())
                {
                    postcode = postcode+"%";
                    String sql = "SELECT TOP(100) * FROM Zipmaster WHERE PostCode LIKE @postcode";
                    
                    zips = connection.Query<Zip>(sql, new { postcode = postcode }).Take(20).ToList();//88行目の@postcodeに対して、変数を入れる
                    

                }

            }
            return zips;
        }

        //POstOrderID(主キーから該当する1件分のデータを取得する)
        public async Task<ZipViewModel> PostOrderIDGetZip(int Id)
        {
            var zip = new ZipViewModel();

            //Dapper
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionstring))//builderに対してConnectionStringを使う
            {

                connection.Open();
                Console.WriteLine("接続成功");
                using (var command = connection.CreateCommand())
                {
                    String sql = "SELECT * FROM Zipmaster WHERE PostOrderID = @OrderPostID";

                    zip = connection.QueryFirstOrDefault<ZipViewModel>(sql, new { OrderPostID = Id });//88行目の@postcodeに対して、変数を入れる


                }

            }

                return zip;
            

            
        }


        //登録
        public void CreateZipmaster(Zip zipmaster)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            //Dapper
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                Console.WriteLine("接続成功");
                String sql = @"INSERT INTO Zipmaster (GroupCode, OldPostCode, PostCode,Prefecture_Kana,City_Kana, ShipToAddress_Kana,Prefecture , City,ShipToAddress, SameShipToAddress, SubDistrictLevel, 
                ChomeName, MultiplecityNumber, UpdateDate, UpdateReason)
                VALUES (@GroupCode,@OldPostCode,@PostCode,@Prefecture_Kana,@City_Kana,@ShipToAddress_Kana,@Prefecture,@City,@ShipToAddress,@SameShipToAddress, @SubDistrictLevel,@ChomeName,
                @MultiplecityNumber,@UpdateDate,@UpdateReason)";// インスタンスの値をStringに変換
                using (SqlCommand command = connection.CreateCommand())
                {
                    var result = connection.Execute(sql, zipmaster);


                }

            }
        }

            //更新
        public void Update(int id, Zip zipmaster)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionstring))//builderに対してConnectionStringを使う
            {
                connection.Open();
                Console.WriteLine("接続成功");

                String sql = @"Update Zipmaster SET GroupCode=@GroupCode, OldPostCode=@OldPostCode, PostCode=@PostCode,Prefecture_Kana=@Prefecture_Kana,City_Kana=@City_Kana, ShipToAddress_Kana=@ShipToAddress_Kana, Prefecture=@Prefecture
                , City=@City ,ShipToAddress=@ShipToAddress, SameShipToAddress=@SameShipToAddress, SubDistrictLevel=@SubDistrictLevel, ChomeName=@ChomeName, MultiplecityNumber=@MultiplecityNumber, UpdateDate=@UpdateDate,
                UpdateReason=@UpdateReason WHERE PostOrderID=@Id";// インスタンスの値をStringに変換

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    var result = connection.Execute(sql, new
                    {
                        GroupCode = zipmaster.GroupCode,
                        OldPostCode = zipmaster.OldPostCode,
                        PostCode = zipmaster.PostCode,
                        Prefecture_Kana = zipmaster.Prefecture_Kana,
                        City_Kana = zipmaster.City_Kana,
                        ShipToAddress_Kana = zipmaster.ShipToAddress_Kana,
                        Prefecture = zipmaster.Prefecture,
                        City = zipmaster.City,
                        ShipToAddress = zipmaster.ShipToAddress,
                        SameShipToAddress = zipmaster.SameShipToAddress,
                        SubDistrictLevel = zipmaster.SubDistrictLevel,
                        ChomeName = zipmaster.ChomeName,
                        MultiplecityNumber = zipmaster.MultiplecityNumber,
                        UpdateDate = zipmaster.UpdateDate,
                        UpdateReason = zipmaster.UpdateReason,
                        Id = id,
                    });
                }
                connection.Close();

            }

        }


        //削除
        public void Delete(int id)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionstring))//builderに対してConnectionStringを使う
            {
                connection.Open();
                Console.WriteLine("接続成功");

                String sql = @"Delete FROM Zipmaster WHERE PostOrderID=@Id";// インスタンスの値をStringに変換

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    var result = connection.Execute(sql, new{Id = id});
                }
                connection.Close();
            }
        }
    }
}






