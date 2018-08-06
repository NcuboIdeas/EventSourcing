using System;

namespace Puppeteer.EventSourcing.Libraries
{

	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Denominacion : Moneda
	{

		public Denominacion(Decimal monto, Monedas tipoDeMoneda) : base(monto, tipoDeMoneda)
		{
		}

		public Denominacion(Numero numero, Monedas tipoDeMoneda) : base(numero, tipoDeMoneda)
		{
		}

		internal override string SimboloMonetario()
		{
			return TipoDeMoneda().nombre();
		}

		internal override bool EstaEnLaMismaMonedaQue(Moneda otroMonto)
		{
            return this.TipoDeMoneda() == otroMonto.TipoDeMoneda();
		}

		public override string NombreDeLaMoneda()
		{
            return TipoDeMoneda().nombre();
		}

		public override Moneda CeroEnEstaMoneda()
		{
            return new Denominacion(new Decimal(0), TipoDeMoneda());
		}

		public override Moneda ObtenerNuevaMonedaEnLaMismaEconomia(double cantidadDePlata)
		{
			double cantidadRedondeada = Math.Floor(cantidadDePlata * 100.0) / 100.0;
            return new Denominacion(new Decimal(cantidadRedondeada), TipoDeMoneda());
		}

		public override double ConvertirADouble()
		{
			return monto.Valor;
		}

		public override Objeto SinDecimales()
		{
            return new Denominacion(new Numero((int)this.monto.Valor), TipoDeMoneda());
		}
	}

}