using System;
using System.Collections.Generic;
using System.Text;

namespace ReceptiAPI.DTO
{
    public class ListaSaPaginacijomDTO<T>
    {
        public List<T> Podaci { get; set; }
        public PaginacijaDTO Paginacija { get; set; }
    }
}
