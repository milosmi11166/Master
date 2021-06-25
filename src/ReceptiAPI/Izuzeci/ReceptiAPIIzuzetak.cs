using System;

namespace ReceptiAPI.Izuzeci
{
    public class ReceptiAPIIzuzetak : Exception
    {
        public int HttpStatusKod { get; }
        public string Poruka { get; }

        public ReceptiAPIIzuzetak(int httpStatusKod, string poruka)
        {
            this.HttpStatusKod = httpStatusKod;
            this.Poruka = poruka;
        }
    }
}
