using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace avio_komp_konz_app
{
    abstract class Let
    {
        protected string Polaziste { get; set; }
        protected string Odrediste { get; set; }
        protected DateTime VremePolaska { get; set; }
        protected DateTime VremeDolaska { get; set; }
        protected string BrojLeta { get; set; }
        protected double BrojMesta { get; set; }
        protected string Kompanija { get; set; }
        public Let(string polaziste, string odrediste, DateTime vremePolaska, DateTime vremeDolaska, string brojLeta, double brojMesta, string kompanija)
        {
            Polaziste = polaziste;
            Odrediste = odrediste;
            VremePolaska = vremePolaska;
            VremeDolaska = vremeDolaska;
            BrojLeta = brojLeta;
            BrojMesta = brojMesta;
            Kompanija = kompanija;
        }
        public virtual void IspisiDetaljeLeta()
        {
            Console.WriteLine($"Polazak iz: {Polaziste}" +
                $"\nOdrediste: {Odrediste}" +
                $"\nVreme Polaska: {VremePolaska}" +
                $"\nVVreme Dolaska: {VremeDolaska}" +
                $"\nBroj leta: {BrojLeta}" +
                $"\nBroj mesta: {BrojMesta}" +
                $"\nKompanija: {Kompanija}" +
                $"\nTrajanje leta: {IzracunajTrajanjeLeta()}" +
                $"\nCena karte: {IzracunajCenuKarte()} eur");
        }
        public abstract double IzracunajTrajanjeLeta();
        public abstract double IzracunajCenuKarte();
        public abstract void RezervisiKartu(string imePutnika);
        public abstract void OtkaziRezervaciju(string imePutnika);
        public abstract void IzmeniLet(string novoPolaziste, string novoOdrediste, DateTime novoVremePolaska, DateTime novoVremeDolaska);

    }
    class DomaciLet : Let
    {
        private int brojRezervisanihMesta { get; set; }
        private string AerodromPolaska { get; set; }
        private string AerodromDolaska { get; set; }
        public DomaciLet(string polaziste, string odrediste, DateTime vremePolaska, DateTime vremeDolaska, string brojLeta, double brojMesta, string kompanija, string aerodromPolaska, string aerodromDolaska)
            : base(polaziste, odrediste, vremePolaska, vremeDolaska, brojLeta, brojMesta, kompanija)
        {
            AerodromPolaska = aerodromPolaska;
            AerodromDolaska = aerodromDolaska;
            brojRezervisanihMesta = 0;
        }
        public override void IspisiDetaljeLeta()
        {
            base.IspisiDetaljeLeta();
            Console.WriteLine($"Aerodrom Polaska: {AerodromPolaska}" +
                $"\nAerodrom Dolaska: {AerodromDolaska}" +
                $"\nBroj Rezervisanih Mesta: {brojRezervisanihMesta}");
        }
        public override double IzracunajTrajanjeLeta()
        {
            return (VremeDolaska - VremePolaska).TotalMinutes;
        }
        public override double IzracunajCenuKarte()
        {
            return IzracunajTrajanjeLeta() * 0.5;
        }
        public override void RezervisiKartu(string imePutnika)
        {
            if (brojRezervisanihMesta < BrojMesta)
            {
                brojRezervisanihMesta++;
                Console.WriteLine($"Karta rezervisana za {imePutnika}.");
            }
            else
            {
                Console.WriteLine("Nema slobodnih mesta.");
            }
        }
        public override void OtkaziRezervaciju(string imePutnika)
        {
            if (brojRezervisanihMesta > 0)
            {
                brojRezervisanihMesta--;
                Console.WriteLine($"Rezervacija otkazana za {imePutnika}.");
            }
            else
            {
                Console.WriteLine("Nema rezervacija za otkazivanje.");
            }
        }
        public override void IzmeniLet(string novoPolaziste, string novoOdrediste, DateTime novoVremePolaska, DateTime novoVremeDolaska)
        {
            if (brojRezervisanihMesta == BrojMesta
                || string.IsNullOrEmpty(novoPolaziste)
                || string.IsNullOrEmpty(novoOdrediste)
                || novoVremePolaska > novoVremeDolaska
                || novoVremePolaska < DateTime.Now
                || novoVremeDolaska < DateTime.Now)
            {
                Console.WriteLine("Ne mozete izmeniti let.");
                return;
            }
            Polaziste = novoPolaziste;
            Odrediste = novoOdrediste;
            VremePolaska = novoVremePolaska;
            VremeDolaska = novoVremeDolaska;
            Console.WriteLine("Detalji leta su izmenjeni.");
        }
        public void RacunajUkupnuCenuKarata()
        {
            double ukupnaCena = brojRezervisanihMesta * IzracunajCenuKarte();
            Console.WriteLine($"Ukupna cena karata za ovaj let je: {ukupnaCena} eur");
        }
    }
    class MedjunarodniLet : Let
    {
        private int brojRezervisanihMesta;
        private string DrzavaPolaska;
        private string DrzavaDolaska;
        private bool PasokaKontrola { get; set; }
        public MedjunarodniLet(string polaziste, string odrediste, DateTime vremePolaska, DateTime vremeDolaska, string brojLeta, double brojMesta, string kompanija, string drzavaPolaska, string drzavaDolaska, bool pasoskaKontrola)
            : base(polaziste, odrediste, vremePolaska, vremeDolaska, brojLeta, brojMesta, kompanija)
        {
            DrzavaPolaska = drzavaPolaska;
            DrzavaDolaska = drzavaDolaska;
            brojRezervisanihMesta = 0;
            PasokaKontrola = pasoskaKontrola;
        }
        public override void IspisiDetaljeLeta()
        {
            base.IspisiDetaljeLeta();
            Console.WriteLine($"Drzava Polaska: {DrzavaPolaska}" +
                $"\nDrzava Dolaska: {DrzavaDolaska}" +
                $"\nBroj Rezervisanih Mesta: {brojRezervisanihMesta}");
            if (PasokaKontrola)
            {
                Console.WriteLine("Pasoska kontrola: DA");
            }
            else
            {
                Console.WriteLine("Pasoska kontrola: NE");
            }
        }
        public override double IzracunajTrajanjeLeta()
        {
            return (VremeDolaska - VremePolaska).TotalMinutes;
        }
        public override double IzracunajCenuKarte()
        {
            return IzracunajTrajanjeLeta() * 2.2;
        }
        public override void RezervisiKartu(string imePutnika)
        {
            if (brojRezervisanihMesta < BrojMesta)
            {
                brojRezervisanihMesta++;
                Console.WriteLine($"Karta rezervisana za {imePutnika}.");
            }
            else
            {
                Console.WriteLine("Nema slobodnih mesta.");
            }
        }
        public override void OtkaziRezervaciju(string imePutnika)
        {
            if (brojRezervisanihMesta > 0)
            {
                brojRezervisanihMesta--;
                Console.WriteLine($"Rezervacija otkazana za {imePutnika}.");
            }
            else
            {
                Console.WriteLine("Nema rezervacija za otkazivanje.");
            }
        }
        public override void IzmeniLet(string novoPolaziste, string novoOdrediste, DateTime novoVremePolaska, DateTime novoVremeDolaska)
        {
            if (brojRezervisanihMesta == BrojMesta
                || string.IsNullOrEmpty(novoPolaziste)
                || string.IsNullOrEmpty(novoOdrediste)
                || novoVremePolaska > novoVremeDolaska
                || novoVremePolaska < DateTime.Now
                || novoVremeDolaska < DateTime.Now)
            {
                Console.WriteLine("Ne mozete izmeniti let.");
                return;
            }
            Polaziste = novoPolaziste;
            Odrediste = novoOdrediste;
            VremePolaska = novoVremePolaska;
            VremeDolaska = novoVremeDolaska;
            Console.WriteLine("Detalji leta su izmenjeni.");
        }
        public void ProveriDostupnost()
        {
            double dostupnaMesta = BrojMesta - brojRezervisanihMesta;
            Console.WriteLine($"Dostupna mesta za ovaj let: {dostupnaMesta}");
        }
    }
    class Charter : Let
    {
        private string Organizator { get; set; }
        private int BrojPutnika { get; set; }
        private Avion avion { get; set; }
        public Charter(string polaziste, string odrediste, DateTime vremePolaska, DateTime vremeDolaska, string brojLeta, double brojMesta, string kompanija, string organizator, Avion avion, int brojPutnika)
            : base(polaziste, odrediste, vremePolaska, vremeDolaska, brojLeta, brojMesta, kompanija)
        {
            Organizator = organizator;
            this.avion = avion;
            BrojPutnika = brojPutnika;
        }
        public override void IspisiDetaljeLeta()
        {
            base.IspisiDetaljeLeta();
            Console.WriteLine($"Organizator: {Organizator}" +
                $"\nBroj Putnika: {BrojPutnika}" +
                $"\nAvion:");
            avion.PrikaziDetaljeAviona();
        }
        public override double IzracunajTrajanjeLeta()
        {
            return (VremeDolaska - VremePolaska).TotalMinutes;
        }
        public override double IzracunajCenuKarte()
        {
            return IzracunajTrajanjeLeta() * 1000*BrojMesta;
        }
        public override void RezervisiKartu(string imePutnika)
        {
            BrojPutnika++;
            Console.WriteLine($"Putnik {imePutnika} je dodat na charter let.");
        }
        public override void OtkaziRezervaciju(string imePutnika)
        {
            if (BrojPutnika > 0)
            {
                BrojPutnika--;
                Console.WriteLine($"Putnik {imePutnika} je uklonjen sa charter leta.");
            }
            else
            {
                Console.WriteLine("Nema putnika za uklanjanje.");
            }
        }
        public override void IzmeniLet(string novoPolaziste, string novoOdrediste, DateTime novoVremePolaska, DateTime novoVremeDolaska)
        {
            if (string.IsNullOrEmpty(novoPolaziste)
                || string.IsNullOrEmpty(novoOdrediste)
                || novoVremePolaska > novoVremeDolaska
                || novoVremePolaska < DateTime.Now
                || novoVremeDolaska < DateTime.Now)
            {
                Console.WriteLine("Ne mozete izmeniti let.");
                return;
            }
            Polaziste = novoPolaziste;
            Odrediste = novoOdrediste;
            VremePolaska = novoVremePolaska;
            VremeDolaska = novoVremeDolaska;
            Console.WriteLine("Detalji leta su izmenjeni.");
        }
        public void PrikaziAvion()
        {
            avion.PrikaziDetaljeAviona();
        }
    }
    class Putnik
    {
        protected string Ime { get; set; }
        protected string Prezime { get; set; }
        protected string BrojPasosa { get; set; }
        public Putnik(string ime, string prezime, string brojPasosa)
        {
            Ime = ime;
            Prezime = prezime;
            BrojPasosa = brojPasosa;
        }
        public string PunoIme()
        {
            return $"{Ime} {Prezime}";
        }
    }
    class Avion
    {
        private string MarkaModel { get; set; }
        private int Kapacitet { get; set; }
        private double MaksimalnaDuzinaLeta { get; set; }
        private bool Dostupan { get; set; }
        private string MestoSkladistenja { get; set; }
        public Avion(string markaModel, int kapacitet, double maksimalnaDuzinaLeta, bool dostupan, string mestoSkladistenja)
        {
            MarkaModel = markaModel;
            Kapacitet = kapacitet;
            MaksimalnaDuzinaLeta = maksimalnaDuzinaLeta;
            Dostupan = dostupan;
            MestoSkladistenja = mestoSkladistenja;
        }
        public void PrikaziDetaljeAviona()
        {
            Console.WriteLine($"Marka i Model: {MarkaModel}" +
                $"\nKapacitet: {Kapacitet} ljudi" +
                $"\nMaksimalna Duzina Leta: {MaksimalnaDuzinaLeta} km");
        }
        public void IzmenaStanjaDostupnosti(bool novoStanje)
        {
            Dostupan = novoStanje;
        }
    }
        internal class Program
        {
        public static Dictionary<int, Let> letovi = new Dictionary<int, Let>();
        public static Dictionary<int, Putnik> putnici = new Dictionary<int, Putnik>();
        public static List<Avion> avioni = new List<Avion>();
        public static int ProveriUnosInta(string unos)
        {
            int rezultat;
            while (!int.TryParse(unos, out rezultat))
            {
                Console.Write("Pogresan unos. Pokusajte ponovo: ");
                unos = Console.ReadLine();
            }
            return rezultat;
        }
        public static string ProveriUnosStringa(string unos)
        {
            while (string.IsNullOrEmpty(unos))
            {
                Console.Write("Pogresan unos. Pokusajte ponovo: ");
                unos = Console.ReadLine();
            }
            return unos;
        }
        public static double ProveriUnosDouble(string unos)
        {
            double rezultat;
            while (!double.TryParse(unos, out rezultat))
            {
                Console.Write("Pogresan unos. Pokusajte ponovo: ");
                unos = Console.ReadLine();
            }
            return rezultat;
        }
        public static DateTime ProveriDateTime(string unos)
        {
            DateTime rezultat;
            while (!DateTime.TryParse(unos, out rezultat))
            {
                Console.Write("Pogresan unos. Pokusajte ponovo: ");
                unos = Console.ReadLine();
            }
            return rezultat;
        }
        public static bool ProveriBool(string unos)
        {
            bool rezultat;
            while (!bool.TryParse(unos, out rezultat))
            {
                Console.Write("Pogresan unos. Pokusajte ponovo: ");
                unos = Console.ReadLine();
            }
            return rezultat;
        }
        static void Main(string[] args)
        {
           
            letovi.Add(1, new DomaciLet("Beograd", "Nis", DateTime.Parse("2024-07-01 10:00"), DateTime.Parse("2024-07-01 11:00"), "BG101", 150, "Air Serbia", "Aerodrom Nikola Tesla", "Aerodrom Konstantin Veliki"));
            letovi.Add(2, new MedjunarodniLet("Beograd", "Pariz", DateTime.Parse("2024-07-02 12:00"), DateTime.Parse("2024-07-02 14:30"), "BG202", 200, "Air France", "Srbija", "Francuska", true));
            letovi.Add(3, new Charter("Beograd", "Malaga", DateTime.Parse("2024-07-03 15:00"), DateTime.Parse("2024-07-03 18:00"), "BG303", 180, "TUI", "Milan", new Avion("Boeing 737", 180, 5600, true, "Beograd"), 150));
            avioni.Add(new Avion("Boeing 737", 180, 5600, true, "Beograd"));
            avioni.Add(new Avion("Airbus A320", 150, 6100, true, "Nis"));
            avioni.Add(new Avion("Cessna 172", 4, 1280, true, "Subotica"));
            avioni.Add(new Avion("Embraer E190", 100, 4260, true, "Kopenhagen"));
            Meni();
        }
        public static void DodajPutnika()
        {
            Console.Clear();
            Console.Write("Unesite Ime: ");
            string ime = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Prezime: ");
            string prezime = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Broj Pasosa: ");
            string brojPasosa = ProveriUnosStringa(Console.ReadLine());
            int id = putnici.Count + 1;
            putnici.Add(id, new Putnik(ime, prezime, brojPasosa));
            Console.WriteLine($"ID putnika je: {id}");
        }
        public static Avion DodajAvion()
        {
            Console.WriteLine("Da li dodajete novi avion ili birate iz skladista");
            string izbor = ProveriUnosStringa(Console.ReadLine()).ToLower();
            if (izbor == "da")
            {
                Console.Write("Model i marka aviona: ");
                string Mm = ProveriUnosStringa(Console.ReadLine());
                Console.Write("Kapacitet aviona: ");
                int kapacitet = ProveriUnosInta(Console.ReadLine());
                Console.Write("Maksimalna duzina leta (km): ");
                double maxDuzinaLeta = ProveriUnosDouble(Console.ReadLine());
                Console.Write("Da li je avion dostupan: ");
                bool dostupnost = ProveriBool(Console.ReadLine());
                Console.Write("Gde se trenutno skladisti: ");
                string mestoSkladistenja = ProveriUnosStringa(Console.ReadLine());
                return new Avion(Mm, kapacitet, maxDuzinaLeta, dostupnost, mestoSkladistenja);
            }
            else
            {
                Console.WriteLine("Izaberite avion iz skladista:");
                for (int i = 0; i < avioni.Count; i++)
                {
                    Console.WriteLine($"{i + 1}:");
                    avioni[i].PrikaziDetaljeAviona();
                }
                int izborAviona = ProveriUnosInta(Console.ReadLine());
                if (izborAviona >= 1 && izborAviona <= avioni.Count)
                {
                    return avioni[izborAviona - 1];
                }
                else
                {
                    Console.WriteLine("Pogresan izbor. Dodajem novi avion.");
                    return DodajAvion();
                }
            }
        }
        public static Avion IzborAviona()
        {
            Console.WriteLine("Izaberite avion iz skladista:");
            for (int i = 0; i < avioni.Count; i++)
            {
                Console.WriteLine($"{i + 1}:");
                avioni[i].PrikaziDetaljeAviona();
            }
            int izborAviona = ProveriUnosInta(Console.ReadLine());
            if (izborAviona >= 1 && izborAviona <= avioni.Count)
            {
                return avioni[izborAviona - 1];
            }
            else
            {
                Console.WriteLine("Pogresan izbor. Pokusajte ponovo.");
                return IzborAviona();
            }
        }
        public static void DodajCharter()
        {
            Console.Clear();
            Console.Write("Unesite Polaziste: ");
            string polaziste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Odrediste: ");
            string odrediste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Vreme Polaska (yyyy-MM-dd HH:mm): ");
            DateTime vremepolaska = ProveriDateTime(Console.ReadLine());
            Console.Write("Unesite Vreme Dolaska (yyyy-MM-dd HH:mm): ");
            DateTime vremedolaska = ProveriDateTime(Console.ReadLine());
            Console.Write("Unesite Broj Leta: ");
            string brojLeta = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Maksimalan broj ljudi koji biste poveli: ");
            double brojMesta = ProveriUnosDouble(Console.ReadLine());
            Console.Write("Unesite Kompaniju: ");
            string kompanija = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Organizatora: ");
            string organizator = ProveriUnosStringa(Console.ReadLine());
            Console.WriteLine("");
            Console.Write("Unesite Broj Putnika koji sigurno idu: ");
            int brojPutnika = ProveriUnosInta(Console.ReadLine());
            Avion avion = IzborAviona();
            Charter ch = new Charter(polaziste, odrediste, vremepolaska, vremedolaska, brojLeta, brojMesta, kompanija, organizator, avion, brojPutnika);
            letovi.Add(letovi.Count + 1, ch);
            Console.WriteLine($"Cena leta: {ch.IzracunajCenuKarte()}" +
                $"\nTrajanje leta: {ch.IzracunajTrajanjeLeta()}");
        }
        
        public static void DodajLet()
        {
            Console.Clear();
            Console.Write("Unesite Polaziste: ");
            string polaziste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Odrediste: ");
            string odrediste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Vreme Polaska (yyyy-MM-dd HH:mm): ");
            DateTime vremepolaska = ProveriDateTime(Console.ReadLine());
            Console.Write("Unesite Vreme Dolaska (yyyy-MM-dd HH:mm): ");
            DateTime vremedolaska = ProveriDateTime(Console.ReadLine());
            Console.Write("Unesite Broj Leta: ");
            string brojLeta = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Broj Mesta: ");
            double brojMesta = ProveriUnosDouble(Console.ReadLine());
            Console.Write("Unesite Kompaniju: ");
            string kompanija = ProveriUnosStringa(Console.ReadLine());
            Console.WriteLine("Da li je let:");
            Console.WriteLine("1. Domaci Let");
            Console.WriteLine("2. Medjunarodni Let");
            Console.WriteLine("3. Charter");
            int izbor = ProveriUnosInta(Console.ReadLine());
            switch (izbor)
            {
                case 1:
                    Console.Write("Unesite Aerodrom Polaska: ");
                    string aerodromPolaska = ProveriUnosStringa(Console.ReadLine());
                    Console.Write("Unesite Aerodrom Dolaska: ");
                    string aerodromDolaska = ProveriUnosStringa(Console.ReadLine());
                    letovi.Add(letovi.Count + 1, new DomaciLet(polaziste, odrediste, vremepolaska, vremedolaska, brojLeta, brojMesta, kompanija, aerodromPolaska, aerodromDolaska));
                    break;
                case 2:
                    Console.Write("Unesite Drzavu Polaska: ");
                    string drzavaPolaska = ProveriUnosStringa(Console.ReadLine());
                    Console.Write("Unesite Drzavu Dolaska: ");
                    string drzavaDolaska = ProveriUnosStringa(Console.ReadLine());
                    Console.Write("Da li je potrebna pasoska kontrola? (da/ne): ");
                    string pasoskaKontrolaUnos = ProveriUnosStringa(Console.ReadLine()).ToLower();
                    bool pasoskaKontrola = pasoskaKontrolaUnos == "da";
                    letovi.Add(letovi.Count + 1, new MedjunarodniLet(polaziste, odrediste, vremepolaska, vremedolaska, brojLeta, brojMesta, kompanija, drzavaPolaska, drzavaDolaska, pasoskaKontrola));
                    break;
                case 3:
                    Console.Write("Unesite Organizatora: ");
                    string organizator = ProveriUnosStringa(Console.ReadLine());
                    Avion avion = DodajAvion();
                    Console.Write("Unesite Broj Putnika: ");
                    int brojPutnika = ProveriUnosInta(Console.ReadLine());
                    letovi.Add(letovi.Count + 1, new Charter(polaziste, odrediste, vremepolaska, vremedolaska, brojLeta, brojMesta, kompanija, organizator, avion, brojPutnika));
                    break;
                default:
                    Console.WriteLine("Pogresan izbor.");
                    break;
            }
        }
        public static void ObrisiLet()
        {
            Console.Clear();
            Console.Write("Unesite ID leta za brisanje: ");
            int idLeta = ProveriUnosInta(Console.ReadLine());
            if (letovi.ContainsKey(idLeta))
            {
                letovi.Remove(idLeta);
                Console.WriteLine("Let je obrisan.");
            }
            else
            {
                Console.WriteLine("Let sa tim ID-jem ne postoji.");
            }
        }
        public static void IspisLetova()
        {
            Console.Clear();
            foreach (var let in letovi)
            {
                Console.WriteLine($"\nID Leta: {let.Key}");
                let.Value.IspisiDetaljeLeta();
                Console.WriteLine(new string('-',50));
            }
        }
        public static void PosebneFunkcionalnosti()
        {
            Console.Clear();
            foreach (var let in letovi)
            {
                if (let.Value is DomaciLet domaciLet)
                {
                    Console.WriteLine("ID LETA: "+ let.Key);
                    domaciLet.RacunajUkupnuCenuKarata();
                }
                else if (let.Value is MedjunarodniLet medjunarodniLet)
                {
                    Console.WriteLine("ID LETA: " + let.Key);
                    medjunarodniLet.ProveriDostupnost();
                }
                else if (let.Value is Charter charter)
                {
                    Console.WriteLine("ID LETA: " + let.Key);
                    charter.PrikaziAvion();
                }
            }
        }
        public static void RezervisanjeKarte()
        {
            Console.Clear();
            Console.Write("Unesite ID Leta: ");
            int idLeta = ProveriUnosInta(Console.ReadLine());
            Console.Write("Unesite ID punika: ");
            int ID = ProveriUnosInta(Console.ReadLine());
            if (letovi.ContainsKey(idLeta) && putnici.ContainsKey(ID))
            {
                string imePutnika = putnici[ID].PunoIme();
                letovi[idLeta].RezervisiKartu(imePutnika);
            }
            else
            {
                Console.WriteLine("Ne mozete rezervisati let");
            }
        }
        public static void OtkazivanjeRezervacije()
        {
            Console.Clear();
            Console.Write("Unesite ID Leta: ");
            int idLeta = ProveriUnosInta(Console.ReadLine());
            Console.Write("Unesite ID punika: ");
            int ID = ProveriUnosInta(Console.ReadLine());
            if (letovi.ContainsKey(idLeta) && putnici.ContainsKey(ID))
            {
                string imePutnika = putnici[ID].PunoIme();
                letovi[idLeta].OtkaziRezervaciju(imePutnika);
            }
            else
            {
                Console.WriteLine("Let/Putnik sa tim ID-jem ne postoji.");
            }
        }
        public static void PrikazPutnika()
        {
            Console.Clear();
            foreach (var putnik in putnici)
            {
                Console.WriteLine($"\nID Putnika: {putnik.Key}" +
                    $"\nIme i Prezime: {putnik.Value.PunoIme()}");
            }
        }
        public static void IzmeniLet()
        {
            Console.Clear();
            Console.Write("Unesite ID leta za izmenu: ");
            int idLeta = ProveriUnosInta(Console.ReadLine());
            if (!letovi.ContainsKey(idLeta))
            {
                Console.WriteLine("Let sa tim ID-jem ne postoji.");
                return;
            }
            Console.Write("Unesite Polaziste: ");
            string polaziste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Odrediste: ");
            string odrediste = ProveriUnosStringa(Console.ReadLine());
            Console.Write("Unesite Vreme Polaska (yyyy-MM-dd HH:mm): ");
            DateTime vremepolaska = ProveriDateTime(Console.ReadLine());
            Console.Write("Unesite Vreme Dolaska (yyyy-MM-dd HH:mm): ");
            DateTime vremedolaska = ProveriDateTime(Console.ReadLine());
            letovi[idLeta].IzmeniLet(polaziste, odrediste, vremepolaska, vremedolaska);
        }
        public static void Meni()
        {
            while (true)
            {
                Console.WriteLine(new string('-',40));
                Console.WriteLine("0. Izlaz");
                Console.WriteLine("1. Dodaj Putnika");
                Console.WriteLine("2. Ispis Letova");
                Console.WriteLine("3. Rezervisi Let");
                Console.WriteLine("4. Otkazi Rezervaciju");
                Console.WriteLine("5. Posebna Funkcionalnost");
                Console.WriteLine("6. Rezervisi Charter");
                Console.Write("Izaberite opciju: ");
                int izbor = ProveriUnosInta(Console.ReadLine());
                switch(izbor)
                {
                    case 0:
                        return;
                    case 1:
                        DodajPutnika();
                        break;
                    case 2:
                        IspisLetova();
                        break;
                    case 3:
                        RezervisanjeKarte();
                        break;
                    case 4:
                        OtkazivanjeRezervacije();
                        break;
                    case 5:
                        PosebneFunkcionalnosti();
                        break;
                    case 6:
                        DodajCharter();
                        break;
                    case 7:
                        Console.Write("Unesite sifru: ");
                        string sifra = ProveriUnosStringa(Console.ReadLine());
                        if (sifra == "VolimOOP")
                        {
                            AdminMeni();
                        }
                        else
                        {
                            Console.WriteLine("Pogresna sifra.");
                        }
                        break;
                    default:
                        Console.WriteLine("Pogresan izbor. Pokusajte ponovo.");
                        break;
                }
            }
        }
        public static void AdminMeni()
        {
            while (true)
            {
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("0. Izlaz");
                Console.WriteLine("1. Dodaj Putnika");
                Console.WriteLine("2. Ispis Letova");
                Console.WriteLine("3. Rezervisi Let");
                Console.WriteLine("4. Otkazi Rezervaciju");
                Console.WriteLine("5. Posebna Funkcionalnost");
                Console.WriteLine("6. Izmeni Let");
                Console.WriteLine("7. Obrisi Let");
                Console.WriteLine("8. Dodaj Let");
                Console.WriteLine("9. Prikazi listu Putnika");
                Console.WriteLine("10. Dodaj Avion za charter");
                Console.WriteLine("11. Nazad na USER mode");
                Console.Write("Izaberite opciju: ");
                int izbor = ProveriUnosInta(Console.ReadLine());
                switch (izbor)
                {
                    case 0:
                        return;
                    case 1:
                        DodajPutnika();
                        break;
                    case 2:
                        IspisLetova();
                        break;
                    case 3:
                        RezervisanjeKarte();
                        break;
                    case 4:
                        OtkazivanjeRezervacije();
                        break;
                    case 5:
                        PosebneFunkcionalnosti();
                        break;
                    case 6:
                        IzmeniLet();
                        break;
                    case 7:
                        ObrisiLet();
                        break;
                    case 8:
                        DodajLet();
                        break;
                    case 9:
                        PrikazPutnika();
                        break;
                    case 10:
                        DodajAvion();
                        break;
                    case 11:
                        Console.Write("Da li ste sigurni? ");
                        string potvrda = ProveriUnosStringa(Console.ReadLine()).ToLower();
                        if (potvrda == "da")
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Ostajete u ADMIN modu.");
                            break;
                        }
                    default:
                        Console.WriteLine("Pogresan izbor. Pokusajte ponovo.");
                        break;
                }
            }
        }
    }
}
