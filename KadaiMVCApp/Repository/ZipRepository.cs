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
using KadaiMVCApp.Interfaces;

namespace KadaiMVCApp.Repository
{
    public class ZipRepository:IZipRepository
    {
        private readonly string _connectionString;

        public ZipRepository()
        {
            _connectionString = AppSettings.dbConnectString;//AppSettingsクラスから文字列'(静的クラスなので、そのまま取得)

        }

        /// <summary>
        /// パスワードとキーワードをもとにデータベースを検索するメソッド
        /// </summary>
        /// <param name="postCode">入力された郵便番号</param>
        /// <param name="keyWord">入力されたキーワード</param>
        /// <returns>条件に一致した郵便番号一覧</returns>
        public async Task<List<Zip>> GetZips(string postCode,string keyWord)
        {
            var zips =new List<Zip>();

            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionString))//builderに対してConnectionStringを使う
            {

                connection.Open();
                Console.WriteLine("接続成功");
                using (var command = connection.CreateCommand())
                {
                    postCode = postCode + "%";
                    var partialKeyWord = String.Format("%{0}%", keyWord);

                    //キーワードが漢字だった場合
                    if (IsKanji(partialKeyWord))
                    {
                        String sql = @"SELECT TOP(100) * FROM ZipMaster WHERE (Prefecture LIKE @prefecture or City LIKE @city or ShipToAddress LIKE @shipToAddress) AND PostCode LIKE @postCode";
                        zips = connection.Query<Zip>(sql, new { prefecture = partialKeyWord, city = partialKeyWord, shipToAddress = partialKeyWord, postCode = postCode }).Take(1000).ToList();
                    }
                    //漢字以外だった場合
                    else
                    {

                        String sql = "SELECT TOP(1000) * FROM ZipMaster WHERE PostCode LIKE @postCode";
                        zips = connection.Query<Zip>(sql, new { postCode = postCode }).Take(1000).ToList();//88行目の@postcodeに対して、変数を入れる
                    }
                }

            }
            return zips;
        }


        /// <summary>
        /// キーワード検索が漢字であるかの判断するメソッド
        /// </summary>
        /// <param name="str">入力されたキーワード</param>
        /// <returns>漢字かそうでないか真偽値を返す</returns>
        private bool IsKanji(string str)
        {
            if (str == null) return false;

            foreach (char c in str)
            {
                if (!(('\u4E00' <= c && c <= '\u9FCF') || ('\uF900' <= c && c <= '\uFAFF') ||('\u3005'==c) ||('\u3400' <= c && c <= '\u4DBF'))) return false;
            }

            return true;
        }


        /// <summary>
        /// 指定されたIdから詳細データを取得するメソッド
        /// </summary>
        /// <param name="id">主キーとなるId</param>
        /// <returns>取得できた場合は、1件のデータを返す</returns>
        public async Task<Zip> GetZipDetail(int id)
        {
            var zip = new Zip();

            //Dapper
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionString))//builderに対してConnectionStringを使う
            {

                connection.Open();
                Console.WriteLine("接続成功");
                using (var command = connection.CreateCommand())
                {
                    String sql = "SELECT * FROM ZipMaster WHERE PostOrderId = @PostOrderId";

                    zip = connection.QueryFirstOrDefault<Zip>(sql, new { PostOrderId = id });//88行目の@postcodeに対して、変数を入れる


                }

            }
            return zip;
        }


        /// <summary>
        /// データベースに登録するメソッド
        /// </summary>
        /// <param name="zipMaster">入力された値</param>
        public void CreateZip(Zip zipMaster)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            //Dapper
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("接続成功");
                String sql = @"INSERT INTO ZipMaster (GroupCode, OldPostCode, PostCode,Prefecture_Kana,City_Kana, ShipToAddress_Kana,Prefecture , City,ShipToAddress, SameShipToAddress, SubDistrictLevel, 
                ExistCityBlockName, MultiplicityNumber, UpdateDate, UpdateReason)
                VALUES (@GroupCode,@OldPostCode,@PostCode,@Prefecture_Kana,@City_Kana,@ShipToAddress_Kana,@Prefecture,@City,@ShipToAddress,@SameShipToAddress, @SubDistrictLevel,@ExistCityBlockName,
                @MultiplicityNumber,@UpdateDate,@UpdateReason)";// インスタンスの値をStringに変換
                using (SqlCommand command = connection.CreateCommand())
                {
                    var result = connection.Execute(sql, zipMaster);
                }

            }
        }

        /// <summary>
        /// データを更新するメソッド
        /// </summary>
        /// <param name="id">更新される郵便番号のid</param>
        /// <param name="zipMaster">新たに入力された値</param>
        public void UpdateZip(int id, Zip zipMaster)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionString))//builderに対してConnectionStringを使う
            {
                connection.Open();
                Console.WriteLine("接続成功");

                String sql = @"Update ZipMaster SET GroupCode=@GroupCode, OldPostCode=@OldPostCode, PostCode=@PostCode,Prefecture_Kana=@Prefecture_Kana,City_Kana=@City_Kana, ShipToAddress_Kana=@ShipToAddress_Kana, Prefecture=@Prefecture
                , City=@City ,ShipToAddress=@ShipToAddress, SameShipToAddress=@SameShipToAddress, SubDistrictLevel=@SubDistrictLevel, ExistCityBlockName=@ExistCityBlockName, MultiplicityNumber=@MultiplicityNumber, UpdateDate=@UpdateDate,
                UpdateReason=@UpdateReason WHERE PostOrderId=@PostOrderId";// インスタンスの値をStringに変換

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    var result = connection.Execute(sql, new
                    {
                        GroupCode = zipMaster.GroupCode,
                        OldPostCode = zipMaster.OldPostCode,
                        PostCode = zipMaster.PostCode,
                        Prefecture_Kana = zipMaster.Prefecture_Kana,
                        City_Kana = zipMaster.City_Kana,
                        ShipToAddress_Kana = zipMaster.ShipToAddress_Kana,
                        Prefecture = zipMaster.Prefecture,
                        City = zipMaster.City,
                        ShipToAddress = zipMaster.ShipToAddress,
                        SameShipToAddress = zipMaster.SameShipToAddress,
                        SubDistrictLevel = zipMaster.SubDistrictLevel,
                        ExistCityBlockName = zipMaster.ExistCityBlockName,
                        MultiplicityNumber = zipMaster.MultiplicityNumber,
                        UpdateDate = zipMaster.UpdateDate,
                        UpdateReason = zipMaster.UpdateReason,
                        PostOrderId = id,
                    });
                }
                connection.Close();

            }

        }

        /// <summary>
        /// 特定のidのついたデータを削除するメソッド
        /// </summary>
        /// <param name="id">詳細ページで開かれているデータのid</param>
        public void DeleteZip(int id)
        {
            Console.WriteLine("SQL Serverに接続しています...");

            using (SqlConnection connection = new SqlConnection(_connectionString))//builderに対してConnectionStringを使う
            {
                connection.Open();
                Console.WriteLine("接続成功");

                String sql = @"Delete FROM ZipMaster WHERE PostOrderID=@id";// インスタンスの値をStringに変換

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    var result = connection.Execute(sql, new{id = id});
                }
                connection.Close();
            }
        }
    }
}






