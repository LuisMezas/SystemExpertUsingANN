//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Becarios
    {
        public int Id_Becario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad { get; set; }
        public double Promedio { get; set; }
        public bool EsRegular { get; set; }
        public float IngresoMensual { get; set; }
        public bool EsBecadoProspera { get; set; }
        public bool Discapacidad { get; set; }
        public int Id_Municipio { get; set; }
        public Nullable<bool> EsBecado { get; set; }
        public Nullable<System.DateTime> FechaEvaluacionbeca { get; set; }
    
        public virtual Municipio Municipio { get; set; }
    }
}
