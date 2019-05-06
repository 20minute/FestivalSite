using Festival.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace Festival.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Validation"];
            HttpCookie cookie2 = Request.Cookies["Nom"];
            HttpCookie cookie3 = Request.Cookies["Login"];
            if (cookie2 == null && cookie3 == null)
            {
                if (cookie != null)
                {
                    ViewData["Message"] = cookie.Value;

                    // Destruction du cookie
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(cookie);

                    return View();
                }
                else
                {
                    ViewData["Message"] = null;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index2");
            }
        }

        public ActionResult Index2()
        {
            HttpCookie cookie = Request.Cookies["Nom"];
            HttpCookie cookie1 = Request.Cookies["Login"];
            String value;
            if (cookie != null || cookie1 != null)
            {
                if (cookie != null)
                {
                    value = cookie.Value;
                    ViewBag.message = "Bienvenue " + value + " !";
                    return View();
                }
                else
                {
                    value = cookie1.Value;
                    ViewBag.message = "Bienvenue " + value + " !";
                    return View();
                }

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Deconnection()
        {
            if (Request.Cookies["Nom"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Nom");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            if (Request.Cookies["Login"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Login");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Inscrire()
        {
            ViewBag.Message = "ça marche !!!";
            return View();
        }

        public ActionResult Connection()
        {
            ViewBag.Message = "ça marche !!!";
            return View();
        }
        [HttpPost]
        public ActionResult Connection(ConnectionFestivalier log)
        {
            IEnumerable<Festivalier> festivaliers = null;

            if(log.Mdp == null)
            {
                ModelState.AddModelError(string.Empty, "Champ vide");
                return View();
            }

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:52837/api/Festivaliers");
                //client.BaseAddress = new Uri("http://localhost:49356/api/Festivaliers");
                client.BaseAddress = new Uri("http://localhost:5575/api/Festivaliers/");

                var responseTask = client.GetAsync("org");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    
                    var readTask = result.Content.ReadAsAsync<IList<Festivalier>>();
                    readTask.Wait();

                    festivaliers = readTask.Result;
                    
                    foreach (Festivalier f in festivaliers)
                    {
                        Md5toString a = new Md5toString();
                        if (f.Email == log.Email && f.Mdp == a.getMd5String(log.Mdp))
                        {
                            // cookie pour retenir le festivalier connecter
                            HttpCookie cookie = new HttpCookie("Nom");
                            cookie.Value = f.Prenom + " " + f.Nom;
                            Response.Cookies.Add(cookie);

                            return RedirectToAction("Index2");
                        }
                    }
                    ViewData["Message"] = "0";
                }
                else
                {
                    
                    festivaliers = Enumerable.Empty<Festivalier>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Inscription()
        {
            var vm = new FestivalFestivalier();

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/Festivals");
                client.BaseAddress = new Uri("http://localhost:5575/api/Festivals/");
                var responseTask = client.GetAsync("org");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Models.Festival>>();
                    readTask.Wait();
                    vm.Festivals = readTask.Result;

                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
            
        }
        [HttpPost]
        public ActionResult Inscription(FestivalFestivalier ctrl)
        {
            DateTime age = DateTime.Now;
            age = age.AddYears(-18);

            DateTime age2 = DateTime.Now;
            age2 = age.AddYears(-150);

            // convertie le controle en festivalier pour la base de donnée
            Festivalier festivalier = ctrl.ControleFestivaliers.ConvertToFestivalier();
            if(festivalier.Genre == null)
            {
                return RedirectToAction("Inscription");
            }

            if (festivalier.Mdp == null)
            {
                ModelState.AddModelError(string.Empty, "Champ vide");
                return View();
            }

            if (festivalier.Naissance <= age && festivalier.Naissance > age2)
            {
                if (festivalier.Email == ctrl.ControleFestivaliers.Emailv)
                {
                    if (festivalier.Mdp == ctrl.ControleFestivaliers.Mdpv)
                    {
                        Md5toString a = new Md5toString();
                        festivalier.setHashMdp(a.getMd5String(festivalier.Mdp));
                        using (var client = new HttpClient())
                        {
                            //client.BaseAddress = new Uri("http://localhost:52837/api/Festivaliers");
                            //client.BaseAddress = new Uri("http://localhost:49356/api/Festivaliers");
                            client.BaseAddress = new Uri("http://localhost:5575/api/Festivaliers/org");
                            var postTask = client.PostAsJsonAsync<Festivalier>("Festivaliers", festivalier);
                            postTask.Wait();

                            var result = postTask.Result;

                            if (result.IsSuccessStatusCode)
                            {
                                HttpCookie cookie = new HttpCookie("Validation");
                                cookie.Value = "1";
                                Response.Cookies.Add(cookie);

                                return RedirectToAction("Index");
                            }
                            ModelState.AddModelError(string.Empty, "Des champs non pas été rempli.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Mot de passe pas valide.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email pas valide.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Vous n'avez pas 18 ans. Ou vous êtes mort");
                return View();
            }
            return View(festivalier);

            //return RedirectToAction("Index");
        }
        
        
        public ActionResult Festivalls()
        {
            var vm = new FestivalsViewModel();
            IEnumerable<Festivalier> festivaliers = null;
            List<string> nbPlaces = new List<string>();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:52837/api/Festivaliers");
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("festivals/org");
                responseTask.Wait();
                var result = responseTask.Result;

                var responseTask2 = client.GetAsync("programmations/org");
                responseTask2.Wait();
                var result2 = responseTask2.Result;

                var responseTask3 = client.GetAsync("artistes/org");
                responseTask3.Wait();
                var result3 = responseTask3.Result;

                var responseTask4 = client.GetAsync("scenes/org");
                responseTask4.Wait();
                var result4 = responseTask4.Result;

                var responseTask5 = client.GetAsync("festivaliers/org");
                responseTask5.Wait();
                var result5 = responseTask5.Result;

                if (result.IsSuccessStatusCode && result2.IsSuccessStatusCode && result3.IsSuccessStatusCode && result4.IsSuccessStatusCode && result5.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Festival.Models.Festival>>();
                    readTask.Wait();
                    vm.Festivals = readTask.Result;

                    var readTask2 = result2.Content.ReadAsAsync<List<Programmation>>();
                    readTask2.Wait();
                    vm.Programmations = readTask2.Result;

                    var readTask3 = result3.Content.ReadAsAsync<List<Artiste>>();
                    readTask3.Wait();
                    vm.Artistes = readTask3.Result;

                    var readTask4 = result4.Content.ReadAsAsync<List<Scene>>();
                    readTask4.Wait();
                    vm.Scenes = readTask4.Result;

                    var readTask5 = result5.Content.ReadAsAsync<List<Festivalier>>();
                    readTask5.Wait();
                    festivaliers = readTask5.Result;

                    foreach (var fest in vm.Festivals)
                    {
                        int place = 0, nbFestivaliers = 0;

                        // requete pour avoir toutes les scenes lier au festival
                        var Sql = from prog in vm.Programmations
                                  where prog.FestivalID == fest.Id
                                  select prog;

                        List<int> list = new List<int>();
                        foreach (var p in Sql)
                        {
                            bool test = false;
                            foreach (var ilist in list)
                            {
                                if (ilist == p.SceneID)
                                {
                                    test = true;
                                }
                            }
                            if (!test)
                            {
                                place = place + p.Scene.Capacite;
                                list.Add(p.SceneID);
                            }
                        }

                        // requete pour avoir tous les festivaliers lier a un festival
                        var Sql2 = from festo in festivaliers
                                   where festo.FestivalId == fest.Id
                                   select festo;

                        foreach (var p in Sql2)
                        {
                            nbFestivaliers++;
                        }

                        nbPlaces.Add(fest.Id+" "+place+" "+(place - nbFestivaliers));
                    }
                    
                    ViewData["listPlace"] = nbPlaces;

                    return View(vm);
                }
                else
                {
                    festivaliers = Enumerable.Empty<Festivalier>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
            
        }
        
        public ActionResult ConnectionOrganisateur()
        {
            ViewBag.Message = "ça marche !!!";
            return View();
        }
        [HttpPost]
        public ActionResult ConnectionOrganisateur(Organisateur log)
        {
            IEnumerable<Organisateur> organisateurs = null;

            if (log.Mdp == null)
            {
                ModelState.AddModelError(string.Empty, "Champ vide");
                return View();
            }

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:52837/api/Festivaliers");
               //client.BaseAddress = new Uri("http://localhost:49356/api/Organisateurs");
                client.BaseAddress = new Uri("http://localhost:5575/api/Organisateurs/org");

                var responseTask = client.GetAsync("organisateurs");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Organisateur>>();
                    readTask.Wait();

                    organisateurs = readTask.Result;

                    foreach (Organisateur f in organisateurs)
                    {
                        Md5toString a = new Md5toString();
                        if (f.Login == log.Login && f.Mdp == a.getMd5String(log.Mdp))
                        {
                            // cookie pour retenir le festivalier connecter
                            HttpCookie cookie = new HttpCookie("Login");
                            cookie.Value = f.Nom + " " + f.Prenom;
                            Response.Cookies.Add(cookie);

                            return RedirectToAction("Index2");
                        }
                    }
                    ViewData["Message"] = "0";
                }
                else
                {
                    organisateurs = Enumerable.Empty<Organisateur>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        
        public ActionResult ConsultanChiffre()
        {
            var vm = new FestivalsViewModel();
            IEnumerable<Organisateur> organisateurs = null;
            IEnumerable<Festivalier> festivaliers = null;
            HttpCookie cookie = Request.Cookies["Login"];
            int nbScene = 0, place = 0, idorga = 0, idfestival = 0, nbArtiste = 0, nbFestivaliers = 0;
            double prix = 0;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("festivaliers/org");
                responseTask.Wait();
                var result = responseTask.Result;

                var responseTask2 = client.GetAsync("programmations/org");
                responseTask2.Wait();
                var result2 = responseTask2.Result;

                var responseTask5 = client.GetAsync("organisateurs/org");
                responseTask5.Wait();
                var result5 = responseTask5.Result;

                if (result.IsSuccessStatusCode && result2.IsSuccessStatusCode && result5.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Festivalier>>();
                    readTask.Wait();
                    festivaliers = readTask.Result;

                    var readTask2 = result2.Content.ReadAsAsync<List<Programmation>>();
                    readTask2.Wait();
                    vm.Programmations = readTask2.Result;

                    var readTask5 = result5.Content.ReadAsAsync<List<Organisateur>>();
                    readTask5.Wait();
                    organisateurs = readTask5.Result;

                    // recuperer l'id de l'organisateur
                    foreach (Organisateur o in organisateurs)
                    {
                        if((o.Nom+" "+o.Prenom).Equals(cookie.Value))
                        {
                           idorga = o.Id;
                        }
                    }

                    // requete pour avoir toutes les scenes lier au festival de l'organisateur
                    var Sql = from prog in vm.Programmations
                              where prog.OrganisateurID == idorga
                              select prog;

                    foreach (var o in Sql)
                    {
                        idfestival = o.FestivalID;
                        prix = o.Festival.Prix;
                    }

                    List<int> list = new List<int>();
                    List<int> list2 = new List<int>();
                    foreach (var p in Sql)
                    {
                        bool test = false;
                        bool test2 = false;
                        foreach (var ilist in list)
                        {
                            if(ilist == p.SceneID)
                            {
                                test = true;
                            }
                        }
                        if(!test)
                        {
                            place = place + p.Scene.Capacite;
                            nbScene++;
                            list.Add(p.SceneID);
                        }

                        foreach (var ilist2 in list2)
                        {
                            if (ilist2 == p.ArtisteID)
                            {
                                test2 = true;
                            }
                        }
                        if (!test2)
                        {
                            nbArtiste++;
                            list2.Add(p.ArtisteID);
                        }
                    }
                    ViewData["place"] = place;
                    ViewData["nbscene"] = nbScene;
                    ViewData["nbArtiste"] = nbArtiste;

                    // requete pour avoir tous les festivaliers lier a un festival
                    var Sql2 = from fest in festivaliers
                               where fest.FestivalId == idfestival
                               select fest;

                    foreach (var p in Sql2)
                    {
                        nbFestivaliers++;
                    }
                    ViewData["nbFestivaliers"] = nbFestivaliers;
                    ViewData["prix"] = (nbFestivaliers*prix)+" €";

                    return View(vm);
                }
                else
                {
                    organisateurs = Enumerable.Empty<Organisateur>();
                    festivaliers = Enumerable.Empty<Festivalier>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
        }

        
        public ActionResult Freq(int idFestival)
        {
            IEnumerable<Interet> interets = null;
            bool testVide = true;
            Interet a = new Interet();

           
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("interets/org");
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Interet>>();
                        readTask.Wait();

                        interets = readTask.Result;

                        foreach (var inte in interets)
                        { 
                            if (idFestival == inte.FestivalId)
                            {
                                testVide = false;
                                a.Id = inte.Id;
                                a.Interesser = inte.Interesser + 1;
                                a.FestivalId = inte.FestivalId;
                            }
                        }

                        if(!testVide)
                        {
                            
                            var postTask2 = client.PutAsJsonAsync("Interets/"+ a.Id, a);
                            postTask2.Wait();

                            var result2 = postTask2.Result;

                            if (result2.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Festivalls");
                            }
                        }
                        else
                        {
                            a.FestivalId = idFestival;
                            a.Interesser = 1;

                            var postTask2 = client.PostAsJsonAsync<Interet>("Interets", a);
                            postTask2.Wait();

                            var result2 = postTask2.Result;

                            if (result2.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Festivalls");
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        interets = Enumerable.Empty<Interet>();

                        ModelState.AddModelError(string.Empty, "Server error");
                        return RedirectToAction("Index");
                    }
                }
        }

        public ActionResult FreqAtt()
        {
            IEnumerable<Organisateur> organisateurs = null;
            IEnumerable<Interet> interets = null;
            IEnumerable<Programmation> programmations = null;
            IEnumerable<Festivalier> festivaliers = null;
            HttpCookie cookie = Request.Cookies["Login"];
            int idorga=0, idFestival=0, nbFestivaliers=0;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("interets/org");
                responseTask.Wait();
                var result = responseTask.Result;

                var responseTask5 = client.GetAsync("organisateurs/org");
                responseTask5.Wait();
                var result5 = responseTask5.Result;

                var responseTask4 = client.GetAsync("programmations/org");
                responseTask4.Wait();
                var result4 = responseTask4.Result;

                var responseTask3 = client.GetAsync("festivaliers/org");
                responseTask3.Wait();
                var result3 = responseTask3.Result;

                if (result.IsSuccessStatusCode && result5.IsSuccessStatusCode && result4.IsSuccessStatusCode && result3.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Interet>>();
                    readTask.Wait();
                    interets = readTask.Result;

                    var readTask5 = result5.Content.ReadAsAsync<List<Organisateur>>();
                    readTask5.Wait();
                    organisateurs = readTask5.Result;

                    var readTask4 = result4.Content.ReadAsAsync<List<Programmation>>();
                    readTask4.Wait();
                    programmations = readTask4.Result;

                    var readTask3 = result3.Content.ReadAsAsync<List<Festivalier>>();
                    readTask3.Wait();
                    festivaliers = readTask3.Result;

                    // recuperer l'id de l'organisateur
                    foreach (Organisateur o in organisateurs)
                    {
                        if ((o.Nom + " " + o.Prenom).Equals(cookie.Value))
                        {
                            idorga = o.Id;
                        }
                    }

                    // recuperer l'id du festival
                    foreach (Programmation p in programmations)
                    {
                        if (p.OrganisateurID == idorga)
                        {
                            idFestival = p.FestivalID;
                        }
                    }

                    // requete pour avoir tous les festivaliers lier a un festival
                    var Sql2 = from fest in festivaliers
                               where fest.FestivalId == idFestival
                               select fest;

                    foreach (var p in Sql2)
                    {
                        nbFestivaliers++;
                    }
                    ViewData["nbFestivaliers"] = nbFestivaliers;

                    foreach (Interet inte in interets)
                    {
                        if(inte.FestivalId == idFestival)
                        {
                            return View(inte);
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    organisateurs = Enumerable.Empty<Organisateur>();
                    interets = Enumerable.Empty<Interet>();
                    programmations = Enumerable.Empty<Programmation>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult ChiffreSelection()
        {
            IEnumerable<Organisateur> organisateurs = null;
            IEnumerable<Selection> selections = null;
            IEnumerable<Programmation> programmations = null;
            IEnumerable<Festivalier> festivaliers = null;
            List<ChiffreSelections> chiffreSelections = new List<ChiffreSelections>();
            HttpCookie cookie = Request.Cookies["Login"];
            int idorga=0, i=0, j=0;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("selections");
                responseTask.Wait();
                var result = responseTask.Result;

                var responseTask5 = client.GetAsync("organisateurs/org");
                responseTask5.Wait();
                var result5 = responseTask5.Result;

                var responseTask4 = client.GetAsync("programmations/org");
                responseTask4.Wait();
                var result4 = responseTask4.Result;

                var responseTask3 = client.GetAsync("festivaliers/org");
                responseTask3.Wait();
                var result3 = responseTask3.Result;

                if (result.IsSuccessStatusCode && result5.IsSuccessStatusCode && result4.IsSuccessStatusCode && result3.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Selection>>();
                    readTask.Wait();
                    selections = readTask.Result;

                    var readTask5 = result5.Content.ReadAsAsync<List<Organisateur>>();
                    readTask5.Wait();
                    organisateurs = readTask5.Result;

                    var readTask4 = result4.Content.ReadAsAsync<List<Programmation>>();
                    readTask4.Wait();
                    programmations = readTask4.Result;

                    var readTask3 = result3.Content.ReadAsAsync<List<Festivalier>>();
                    readTask3.Wait();
                    festivaliers = readTask3.Result;

                    // recuperer l'id de l'organisateur
                    foreach (Organisateur o in organisateurs)
                    {   
                        if ((o.Nom + " " + o.Prenom).Equals(cookie.Value))
                        {
                            idorga = o.Id;
                        }
                    }

                    var Sql = from sel in selections
                              where sel.Programmation.OrganisateurID == idorga
                              select sel;

                    var Sql2 = from prog in programmations
                               where prog.OrganisateurID == idorga
                               select prog;
                    
                    foreach (var p in Sql2)
                    {
                        i = 0;
                        j = 0;
                        foreach(var o in Sql)
                        {
                            if(p.ProgrammationId == o.ProgrammationId && o.PrimaireSecondaire == 1)
                            {
                                i++;
                            }
                            if (p.ProgrammationId == o.ProgrammationId && o.PrimaireSecondaire == 2)
                            {
                                j++;
                            }
                        }
                        chiffreSelections.Add(new ChiffreSelections(p, i, j));
                    }

                    return View(chiffreSelections);
                }
                else
                {
                    organisateurs = Enumerable.Empty<Organisateur>();
                    selections = Enumerable.Empty<Selection>();
                    programmations = Enumerable.Empty<Programmation>();

                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Selections()
        {
            var vm = new FestivalsViewModel();

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("festivals/org");
                responseTask.Wait();
                var result = responseTask.Result;

                var responseTask2 = client.GetAsync("programmations/org");
                responseTask2.Wait();
                var result2 = responseTask2.Result;

                var responseTask3 = client.GetAsync("artistes/org");
                responseTask3.Wait();
                var result3 = responseTask3.Result;

                var responseTask4 = client.GetAsync("scenes/org");
                responseTask4.Wait();
                var result4 = responseTask4.Result;

                var responseTask5 = client.GetAsync("festivaliers/org");
                responseTask4.Wait();
                var result5 = responseTask5.Result;

                var responseTask6 = client.GetAsync("selections");
                responseTask4.Wait();
                var result6 = responseTask6.Result;

                if (result.IsSuccessStatusCode && result2.IsSuccessStatusCode && result3.IsSuccessStatusCode && result4.IsSuccessStatusCode && result5.IsSuccessStatusCode && result6.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Models.Festival>>();
                    readTask.Wait();
                    vm.Festivals = readTask.Result;

                    var readTask2 = result2.Content.ReadAsAsync<List<Programmation>>();
                    readTask2.Wait();
                    vm.Programmations = readTask2.Result;

                    var readTask3 = result3.Content.ReadAsAsync<List<Artiste>>();
                    readTask3.Wait();
                    vm.Artistes = readTask3.Result;

                    var readTask4 = result4.Content.ReadAsAsync<List<Scene>>();
                    readTask4.Wait();
                    vm.Scenes = readTask4.Result;

                    var readTask5 = result5.Content.ReadAsAsync<List<Festivalier>>();
                    readTask5.Wait();
                    vm.Festivaliers = readTask5.Result;

                    var readTask6 = result6.Content.ReadAsAsync<List<Selection>>();
                    readTask6.Wait();
                    vm.Selections = readTask6.Result;

                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult ArtisteDetail(int id)
        {
            ViewBag.Id = id;
            List<Artiste> listeArtiste = new List<Artiste>();
            Artiste artiste = new Artiste();
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("artistes/org");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Artiste>>();
                    readTask.Wait();
                    listeArtiste = readTask.Result;

                    foreach (var i in listeArtiste)
                    {
                        if (i.ArtisteID.Equals(ViewBag.Id))
                        {
                            artiste = i;
                        }
                    }
                    return View(artiste);
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Festivalls");
                }
            }
        }

        public ActionResult Ajouter(int progId, int primSec)
        {
            HttpCookie cookie = Request.Cookies["Nom"];
            IEnumerable<Festivalier> festivaliers = null;

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:49356/api/");
                client.BaseAddress = new Uri("http://localhost:5575/api/");
                var responseTask = client.GetAsync("festivaliers/org");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Festivalier>>();
                    readTask.Wait();
                    festivaliers = readTask.Result;

                    foreach (var festivalier in festivaliers)
                    {
                        if (cookie.Value.Equals(festivalier.Prenom + " " + festivalier.Nom))
                        {
                            Selection s = new Selection();
                            s.FestivalierId = festivalier.ID;
                            s.ProgrammationId = progId;
                            s.PrimaireSecondaire = primSec;
                            var postTask = client.PostAsJsonAsync<Selection>("selections", s);
                            postTask.Wait();

                            var result3 = postTask.Result;

                            if (result3.IsSuccessStatusCode)
                            {

                                return RedirectToAction("Selections");
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Server error");
                    return RedirectToAction("Festivalls");
                }
            }
        }
    }
}