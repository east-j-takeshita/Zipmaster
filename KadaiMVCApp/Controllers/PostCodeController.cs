using KadaiMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using KadaiMVCApp.Repository;
using System.IO.Compression;
using System.Text.Json;
using System.Collections;
using System.Runtime.CompilerServices;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using KadaiMVCApp.Interfaces;

namespace KadaiMVCApp.Controllers
{
    public class PostCodeController : Controller
    {
        //依存性注入の利用
        private readonly IZipRepository _zipRepository;
        public PostCodeController(IZipRepository zipRepository)
        {
            _zipRepository = zipRepository;
        }


        /// <summary>
        /// Indexビューアクション
        /// </summary>
        /// <returns>値が空のIndexページ</returns>
        public IActionResult Index()
        {
            var zipViewModel = new ZipViewModel();
            zipViewModel.InputtedKeyValue=new InputtedKeyValue();
            return View("index",zipViewModel);
        }

        /// <summary>
        /// データの詳細ビューアクション
        /// </summary>
        /// <returns></returns>
        public IActionResult PostCodeDetail()
        {
            return View("index");
        }

        /// <summary>
        /// 新規追加ビューアクション
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()//これをまず書かないとビューとして見れない
        {
            return View();
        }

        /// <summary>
        /// 新規追加アクション
        /// </summary>
        /// <param name="zip">新規追加するデータ</param>
        /// <returns>値のないindexページ</returns>
        [HttpPost("Create")]
        public ActionResult<List<Zip>> Create(Zip zip)
        {
            _zipRepository.CreateZip(zip);
            var zipViewModel = new ZipViewModel();
            zipViewModel.InputtedKeyValue = new InputtedKeyValue();
            return View("index", zipViewModel);
        }

        /// <summary>
        /// 検索結果indexページを表示するAPI
        /// </summary>
        /// <param name="postCode">検索のために入力された郵便番号</param>
        /// <param name="keyWord">検索のために入力されたキーワード</param>
        /// <returns>条件に合致した郵便番号検索一覧</returns>
        [HttpPost("Index/{postCode?}/{keyWord?}")]
        public async Task<ActionResult<ZipViewModel>> Index(string postCode, string keyWord)
        {


            //if (postcode.Length != 7)//前方一致のためコメントアウト
            //{
            //    //ContentResultはActionResultを継承しているから、使える
            //    ViewData["Message"] = "郵便番号が7文字ではありません。";
            //    return View("Index");
            //}
            var zips = new List<Zip>();
            zips = await _zipRepository.GetZips(postCode,keyWord);
            
            var zipViewModel = new ZipViewModel();
            zipViewModel.InputtedKeyValue = new InputtedKeyValue();
            zipViewModel.InputtedKeyValue.InputtedPostCode = postCode;
            zipViewModel.InputtedKeyValue.InputtedKeyWord= keyWord;
            // データが見つからなかった場合
            if (zips.Count == 0)
            {
                return zipViewModel;
            }
            zipViewModel.ZipsData = zips;//zipsをZipViewModelのZipsDataに格納
            return zipViewModel;
        }


        /// <summary>
        /// zipの詳細ページを表示するAPI
        /// </summary>
        /// <param name="id">詳細ボタンが押されたid情報</param>
        /// <returns>idのzipデータを保持したPostCodeDetailページ</returns>
        [HttpGet("PostCodeDetail/{id?}")]
        public async Task<ActionResult<Zip>> PostCodeDetail(int id)
        {
            var zip = new Zip();
            zip = await _zipRepository.GetZipDetail(id);
            //データが見つからなかった場合
            if (zip == null)
            {
                return View("index", new ZipViewModel());
            }
            //該当するデータが見つかった場合
            else
            {
                return View(zip);
            }
        }


        /// <summary>
        /// 更新ページAPI
        /// </summary>
        /// <param name="id">更新されるデータのid情報</param>
        /// <param name="zipMaster">更新のためのデータ</param>
        /// <returns></returns>
        [HttpPost("Update/{id?}")]
        public IActionResult Update(int id, Zip zipMaster)
        {
            zipMaster.PostOrderId = id;
            _zipRepository.UpdateZip(id, zipMaster);//id入力をPostOrderIDに入れれば、良い
            var zipViewModel = new ZipViewModel();
            zipViewModel.InputtedKeyValue = new InputtedKeyValue();
            return Index();

        }

        /// <summary>
        /// 削除API
        /// </summary>
        /// <param name="id">削除されるデータのid情報</param>
        /// <returns></returns>
        [HttpPost("Delete/{id?}")]//?は?id を省略可能、外せない。
        public IActionResult Delete(int id)
        {
            _zipRepository.DeleteZip(id);
            var zipViewModel = new ZipViewModel();
            zipViewModel.InputtedKeyValue = new InputtedKeyValue();
            return View("index", zipViewModel);
        }

    }
}
