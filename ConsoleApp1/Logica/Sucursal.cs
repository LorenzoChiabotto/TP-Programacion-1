﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Sucursal:Local
    {
        public float TasaInteres { get; set; }

        public Sucursal(int id, string ciudad, string direccion, int codP, float tasa)
        {
            this.ID = id;
            this.Ciudad = ciudad;
            this.Direccion = direccion;
            this.CodPostal = codP;

            this.TasaInteres = tasa;

        }
    }
}
