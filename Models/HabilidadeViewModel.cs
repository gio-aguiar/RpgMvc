using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace RpgMvc.Models
{
    public class HabilidadeViewModel
    {
        public int Id{get;set;}
        public int Dano{get;set;}
        public string Nome{get;set;} = string.Empty;
        public List<PersonagemHabilidadeViewModel>PersonagemHabilidade {get;set;}
    }
}