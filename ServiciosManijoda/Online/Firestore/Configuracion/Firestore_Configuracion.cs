//using Firebase.Auth;
using Google.Cloud.Firestore;
using ManijodaServicios.AppSettings;

namespace ManijodaServicios.Online.Firestore.Configuracion
{
    public class Firestore_Configuracion
    {
        string webAPIKey = SettingsOnline.IdProyecto;
        FirestoreDb db;

        public Firestore_Configuracion()
        {
            db = null; //FirestoreDb.Create(webAPIKey);
        }
        public async void ConsultaSecuencial(string tipoDocumento)
        {
            DocumentSnapshot snap = await db.Collection("Secuenciales").Document("Secuencial").GetSnapshotAsync();
            snap.GetValue<int>(tipoDocumento);
            //snap.SetValues<>
        }
    }
}
