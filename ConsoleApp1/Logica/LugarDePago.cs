﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LugarDePago:Comercio
    {
        public bool EsSucursal { get; set; }

        public LugarDePago()
        {

        }
        public LugarDePago(int id, string ciudad, string direccion, int codP, string razonSocial, bool esSucur)
        {

            this.ID = id;
            this.Ciudad = ciudad.ToUpper();
            this.Direccion = direccion.ToUpper();
            this.CodPostal = codP;
            this.RazonSocial = razonSocial.ToUpper();

            this.EsSucursal = esSucur;
        }
    }
}
