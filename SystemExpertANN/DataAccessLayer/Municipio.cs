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
    
    public partial class Municipio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Municipio()
        {
            this.Becarios = new HashSet<Becarios>();
        }
    
        public int Id_Municipio { get; set; }
        public string Estado { get; set; }
        public string NombreMunicipio { get; set; }
        public bool Prioridad { get; set; }
        public Nullable<System.DateTime> FechaModificacionValores { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Becarios> Becarios { get; set; }

        public override string ToString()
        {
            return this.NombreMunicipio.ToString();
        }
    }
}
