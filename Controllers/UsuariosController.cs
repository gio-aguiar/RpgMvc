using System;
using System.Net.Http; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RpgMvc.Models;

namespace RpgMvc.Controllers
{
    
    public class UsuariosController : Controller
    {
        public string UriBase = "http://luizsouza.somee.com/RpgApi/Usuarios/";

        [HttpGet]
        public ActionResult Index()
        {
            return View("CadastrarUsuario");
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarAsync(UsuarioViewModel u)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient()) 
                {
                    string uriComplementar = "Registrar";

                    var content = new StringContent(JsonConvert.SerializeObject(u));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(UriBase + uriComplementar, content);

                    string serialized = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Mensagem"] = "Usuario registrado com sucesso!";
                        return View("AutenticarUsuario");
                    }
                    else
                    {
                        throw new Exception(serialized);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View("AutenticarUsuario");
        }

        [HttpPost]
        public async Task<ActionResult> AutenticarAsync(UsuarioViewModel u)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string uriComplementar = "Autenticar";

                    var content = new StringContent(JsonConvert.SerializeObject(u));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(UriBase + uriComplementar, content);

                    string serialized = await response.Content.ReadAsStringAsync();

                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UsuarioViewModel uLogado = JsonConvert.DeserializeObject<UsuarioViewModel>(serialized);
                        HttpContext.Session.SetString("SessionTokenUsuario", uLogado.Token);
                        TempData["Mensagem"] = string.Format("Bem vindo {0}", uLogado.Username);
                        return RedirectToAction("Index", "Personagem");
                    }
                    else 
                    {
                        throw new Exception(serialized);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return IndexLogin();
            }
        }
    }
}
