using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Configuracion
{
    public class FacturacionSettings
    {
        public string CodigoEstablecimiento { get; set;}
        public string PuntoEmision { get; set; }
        public string NumeroResolucionContribuyenteEspecial { get; set; }
        public string NumeroResolucionAgenteRetencion { get; set; }
        public bool EsObligadoLlevarContabilidad{ get; set; }
        public bool EsRegimenMicroempresas { get; set; }
        public string TipoAmbiente { get; set; }
        public string AutoridadCertifica { get; set; }
        public string TokenFirma { get; set; }
        public string NombreCertificado { get; set; }
        public string ContraseniaCertificado { get; set; }
        public string UrlPruebaComprobantesOrganismoControl { get; set; }
        public string UrlPruebaAutorizacionOrganismoControl { get; set; }
        public string UrlProduccionComprobantesOrganismoControl { get; set; }
        public string UrlProduccionAutorizacionOrganismoControl { get; set; }

    }
}
