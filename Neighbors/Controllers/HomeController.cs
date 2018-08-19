﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neighbors.Models;

namespace Neighbors.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message1"] = "Have you ever dreamt of friends who could lend you tools instead of wasting precious money to buy it ?";
            ViewData["Message2"] = "Neighbors are here to fulfil your dream.";
            ViewData["Message3"] = "Neighbors is a social platform that lets you borrow anything you wish from other members.";
            ViewData["Message4"] = "Of course, as a good neighbor one is obligated to return the tools on time and keep in a tip-top shape.";
            ViewData["Message5"] = "A user who will not follow these rules, will be fined in accordance with the terms and agreements.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Thanks for your visit!";
            ViewData["Text"] = "Please feel free to contact us with any feedback or question.";

            return View();
        }

  /*      public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}