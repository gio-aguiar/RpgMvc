using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RpgMvc.Models;

// Lembrete: Parei no passo 8 do pdf aula17
namespace RpgMvc.Controllers
{
    public class DisputaController : Controller
    {
        private string uriBase = "http://xyz/Disputas/"; // Corrigido o prefixo da uriBase

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string uriBuscaPersonagens = "http://xyz.somee.com/RpgApi/Personagens/GetAll";
                HttpResponseMessage response = await httpClient.GetAsync(uriBuscaPersonagens);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<PersonagemViewModel> listaPersonagens = JsonConvert.DeserializeObject<List<PersonagemViewModel>>(serialized);
                    ViewBag.ListaAtacantes = listaPersonagens;
                    ViewBag.ListaOponentes = listaPersonagens;
                    return View();
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(DisputaViewModel disputa)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Arma";

                var content = new StringContent(JsonConvert.SerializeObject(disputa));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    disputa = JsonConvert.DeserializeObject<DisputaViewModel>(serialized);
                    TempData["Mensagem"] = disputa.Narracao;
                    return RedirectToAction("Index", "Personagens");
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> IndexHabilidadeAsync()
        {
            try
            {
                string uriBuscaHabilidades = "http://xyz.somee.com/RpgApi/PersonagemHabilidades/GetHabilidades";
                HttpClient httpClient = new HttpClient(); // Instanciando o HttpClient
                HttpResponseMessage response = await httpClient.GetAsync(uriBuscaHabilidades);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<HabilidadeViewModel> listaHabilidades = JsonConvert.DeserializeObject<List<HabilidadeViewModel>>(serialized);
                    ViewBag.listaHabilidades = listaHabilidades;
                    return View("IndexHabilidades"); // Retornando a view correta
                }
                else
                
                    throw new System.Exception(serialized);
                    return View ("IndexHabilidades");
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> IndexHabilidadesAsync(DisputaViewModel disputa)
        {
            try{
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Habilidade";
                var content = new StringContent(JsonConvert.SerializeObject(disputa));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                String serialized = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    disputa = await Task.Run(()
                    => JsonConvert.DeserializeObject<DisputaViewModel>(serialized));
                    TempData["Mensagem"] = disputa.Narracao;
                    return RedirectToAction("Index", "Personagens");
                }
                else
                throw new System.Exception(serialized);

            }
            catch(System.Exception ex){
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");

            }
        }

        [HttpGet]
public async Task<ActionResult> IndexDisputasAsync()
{
try
{
string uriComplementar = "Listar";
HttpClient httpClient = new HttpClient();
string token = HttpContext.Session.GetString("SessionTokenUsuario");
httpClient.DefaultRequestHeaders.Authorization = new
AuthenticationHeaderValue("Bearer", token);
HttpResponseMessage response = await httpClient.GetAsync(uriBase +
uriComplementar);
string serialized = await response.Content.ReadAsStringAsync();
if (response.StatusCode == System.Net.HttpStatusCode.OK)
{
List<DisputaViewModel> lista = await Task.Run(() =>
JsonConvert.DeserializeObject<List<DisputaViewModel>>(serialized));
return View(lista);
}
else
throw new System.Exception(serialized);
}
catch (System.Exception ex)
{
TempData["MensagemErro"] = ex.Message;
return RedirectToAction("Index");
}
}

[HttpGet]
public async Task<ActionResult> ApagarDisputasAsync()
{
try
{
HttpClient httpClient = new HttpClient();
string token = HttpContext.Session.GetString("SessionTokenUsuario");
httpClient.DefaultRequestHeaders.Authorization = new
AuthenticationHeaderValue("Bearer", token);
string uriComplementar = "ApagarDisputas";
HttpResponseMessage response = await httpClient.DeleteAsync(uriBase +
uriComplementar);
string serialized = await response.Content.ReadAsStringAsync();
if (response.StatusCode == System.Net.HttpStatusCode.OK)
TempData["Mensagem"] = "Disputas apagadas com sucessso.";
else
throw new System.Exception(serialized);
}
catch (System.Exception ex)
{
TempData["MensagemErro"] = ex.Message;
}
return RedirectToAction("IndexDisputas", "Disputas");
}

}
}
