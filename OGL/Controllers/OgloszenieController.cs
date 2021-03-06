﻿using System;
using System.Net;
using System.Web.Mvc;
using Repozytorium.Models;
using Repozytorium.IRepo;
using Microsoft.AspNet.Identity;

namespace OGL.Controllers
{
    public class OgloszenieController : Controller
    {
        private readonly IOgloszenieRepo _repo;

        public OgloszenieController(IOgloszenieRepo repo)
        {
            _repo = repo;
        }
        // GET: Ogloszenie
        public ActionResult Index()
        {           
            var ogloszenia = _repo.PobierzOgloszenia();            
            return View(ogloszenia);
        }

        // GET: Ogloszenie
        public ActionResult Partial()
        {
            var ogloszenia = _repo.PobierzOgloszenia();
            return PartialView("Index", ogloszenia);
        }

        // GET: Ogloszenie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Create
        [Authorize]
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Ogloszenie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                ogloszenie.UzytkownikId = User.Identity.GetUserId();
                ogloszenie.DataDodania = DateTime.Now;
                try
                {
                    _repo.Dodaj(ogloszenie);
                    _repo.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(ogloszenie);
                }                
            }            
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            else if (ogloszenie.UzytkownikId != User.Identity.GetUserId() && !(User.IsInRole("Admin") || User.IsInRole("Pracownik")))
            {                                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    _repo.Aktualizuj(ogloszenie);
                    _repo.SaveChanges();                    
                }
                catch
                {
                    ViewBag.Blad = true;
                    return View(ogloszenie);
                }
            }
            ViewBag.Blad = false;            
            return View(ogloszenie);
        }

        // GET: Ogloszenie/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, bool? blad)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
            if (ogloszenie == null)
            {
                return HttpNotFound();
            }
            else if(ogloszenie.UzytkownikId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                
            }
            if(blad != null)
            {
                ViewBag.Blad = true;
            }
            return View(ogloszenie);
        }

        // POST: Ogloszenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.UsunOgloszenie(id);
            try
            {
                _repo.SaveChanges();                
            }
            catch 
            {
                return RedirectToAction("Delete", new { id = id, blad = true });         
            }
            
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
