﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ProfileSampleEntities();

            ////var sources = context.ImgSources.Take(20).Select(x => x.Id);

            ////var model = new List<ImageModel>();

            ////foreach (var id in sources)
            ////{
            ////    var item = context.ImgSources.Find(id);

            ////    var obj = new ImageModel()
            ////    {
            ////        Name = item.Name,
            ////        Data = item.Data
            ////    };

            ////    model.Add(obj);
            ////}

            var model = context.ImgSources.Take(20)
                    .Select(source => new ImageModel { Name = source.Name, Data = source.Data })
                    .ToList();

            return this.View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}