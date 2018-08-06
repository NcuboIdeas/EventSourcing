using Puppeteer.EventSourcing.Interprete.Libraries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Puppeteer.EventSourcing.Interprete
{
    using TablaDeSimbolos = DB.TablaDeSimbolos;
	using TokenType = Puppeteer.EventSourcing.Interprete.Token.TokenType;
	using Linea = Puppeteer.EventSourcing.Interprete.Libraries.Linea;
	using LineaComentada = Puppeteer.EventSourcing.Interprete.Libraries.LineaComentada;
	using Comando = Puppeteer.EventSourcing.Interprete.Libraries.Comando;
	using ComandoBloque = Puppeteer.EventSourcing.Interprete.Libraries.ComandoBloque;
	using ComandoCall = Puppeteer.EventSourcing.Interprete.Libraries.ComandoCall;
	using ComandoIf = Puppeteer.EventSourcing.Interprete.Libraries.ComandoIf;
	using ComandoNuevaInstancia = Puppeteer.EventSourcing.Interprete.Libraries.ComandoNuevaInstancia;
	using ComandoNulo = Puppeteer.EventSourcing.Interprete.Libraries.ComandoNulo;
	using ComandoPrint = Puppeteer.EventSourcing.Interprete.Libraries.ComandoPrint;
	using Declaracion = Puppeteer.EventSourcing.Interprete.Libraries.Declaracion;
	using DeclaracionDeParametro = Puppeteer.EventSourcing.Interprete.Libraries.DeclaracionDeParametro;
	using DeclaracionProcedure = Puppeteer.EventSourcing.Interprete.Libraries.DeclaracionProcedure;
	using Expresion = Puppeteer.EventSourcing.Interprete.Libraries.Expresion;
	using Id = Puppeteer.EventSourcing.Interprete.Libraries.Id;
	using IdConPunto = Puppeteer.EventSourcing.Interprete.Libraries.IdConPunto;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;
	using LineaDePrograma = Puppeteer.EventSourcing.Interprete.Libraries.LineaDePrograma;
	using LineaEnBlanco = Puppeteer.EventSourcing.Interprete.Libraries.LineaEnBlanco;
	using LiteralBoolean = Puppeteer.EventSourcing.Interprete.Libraries.LiteralBoolean;
	using LiteralDecimal = Puppeteer.EventSourcing.Interprete.Libraries.LiteralDecimal;
	using LiteralFecha = Puppeteer.EventSourcing.Interprete.Libraries.LiteralFecha;
	using LiteralFechaHora = Puppeteer.EventSourcing.Interprete.Libraries.LiteralFechaHora;
	using LiteralHilera = Puppeteer.EventSourcing.Interprete.Libraries.LiteralHilera;
	using LiteralMes = Puppeteer.EventSourcing.Interprete.Libraries.LiteralMes;
	using LiteralMoneda = Puppeteer.EventSourcing.Interprete.Libraries.LiteralMoneda;
	using LiteralNull = Puppeteer.EventSourcing.Interprete.Libraries.LiteralNull;
	using LiteralNumero = Puppeteer.EventSourcing.Interprete.Libraries.LiteralNumero;
	using NuevaInstancia = Puppeteer.EventSourcing.Interprete.Libraries.NuevaInstancia;
	using OpAnd = Puppeteer.EventSourcing.Interprete.Libraries.OpAnd;
	using OpDividir = Puppeteer.EventSourcing.Interprete.Libraries.OpDividir;
	using OpIgualQue = Puppeteer.EventSourcing.Interprete.Libraries.OpIgualQue;
	using OpMayorOIgualQue = Puppeteer.EventSourcing.Interprete.Libraries.OpMayorOIgualQue;
	using OpMayorQue = Puppeteer.EventSourcing.Interprete.Libraries.OpMayorQue;
	using OpMenorOIgualQue = Puppeteer.EventSourcing.Interprete.Libraries.OpMenorOIgualQue;
	using OpMenorQue = Puppeteer.EventSourcing.Interprete.Libraries.OpMenorQue;
	using OpMenos = Puppeteer.EventSourcing.Interprete.Libraries.OpMenos;
	using OpMultiplicar = Puppeteer.EventSourcing.Interprete.Libraries.OpMultiplicar;
	using OpNoIgualQue = Puppeteer.EventSourcing.Interprete.Libraries.OpNoIgualQue;
	using OpNot = Puppeteer.EventSourcing.Interprete.Libraries.OpNot;
	using OpOr = Puppeteer.EventSourcing.Interprete.Libraries.OpOr;
	using OpRestar = Puppeteer.EventSourcing.Interprete.Libraries.OpRestar;
	using OpSumar = Puppeteer.EventSourcing.Interprete.Libraries.OpSumar;
	using ParserValidation = Puppeteer.EventSourcing.Interprete.Libraries.ParserValidation;
	using Programa = Puppeteer.EventSourcing.Interprete.Libraries.Programa;
	using Punto = Puppeteer.EventSourcing.Interprete.Libraries.Punto;
	using PuntoConPunto = Puppeteer.EventSourcing.Interprete.Libraries.PuntoConPunto;
	using Fecha = Puppeteer.EventSourcing.Libraries.Fecha;
	using FechaHora = Puppeteer.EventSourcing.Libraries.FechaHora;
	using Hilera = Puppeteer.EventSourcing.Libraries.Hilera;
	using Meses = Puppeteer.EventSourcing.Libraries.Meses;
	using Monedas = Puppeteer.EventSourcing.Libraries.Monedas;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class Parser
	{
		private Comando ultimoComandoValido = new ComandoNulo("Es el inicio del archivo y no hay un comando anterior.");
		private readonly TablaDeSimbolos tablaDeSimbolos;
		private readonly Lexer lexer;
		private readonly Salida salida;
        private readonly Assembly assembly;

        private readonly List<Type> libraries;

        private static readonly Dictionary<string, Type> tiposPrimitivos = new Dictionary<string, Type>();

		static Parser()
		{
			foreach (Type tipoPrimitivo in NuevaInstancia.TiposPrimitivos())
			{
				string nombre = tipoPrimitivo.Name.ToLower();
				tiposPrimitivos[nombre] = tipoPrimitivo;
			}
		}

        public Parser(Assembly assembly, TablaDeSimbolos tablaDeSimbolos, Salida salida)
		{
            libraries = LoadTypesFromLibrary(assembly);
			this.tablaDeSimbolos = tablaDeSimbolos;
			this.salida = salida;
            this.assembly = assembly;
            lexer = new Lexer(" \f");
        }

        private List<Type> LoadTypesFromLibrary(Assembly assembly)
        {
            List<Type> result = new List<Type>();
            foreach (Type t in assembly.GetTypes())
            {
                if (isPuppet(t) && t.IsSubclassOf(typeof(Objeto)))
                {
                    result.Add(t);
                }
            }
            return result;
        }

        private bool isPuppet(Type t)
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);
            foreach (System.Attribute attr in attrs)
            {
                if (attr is Puppet) return true;
            }
            return false;
        }


        public Programa Procesar()
		{
			Programa result = ParsearPrograma();
			return result;
		}

		public virtual void EstablecerComando(string comando)
		{
			this.lexer.Comando = comando;
		}

		private Programa ParsearPrograma()
		{
			LineaDePrograma linea2 = null;
			List<Linea> lineas = new List<Linea>();
			while (lexer.tokenActual.Type != TokenType.eof)
			{
				if (lexer.tokenActual.Type == TokenType.eol)
				{
					Linea linea = ParsearEspaciosEnBlanco();
					lineas.Add(linea);
					if (linea == null)
					{
						linea = linea2;
					}
				}
				else if (lexer.tokenActual.Type == TokenType.comentarioDeLinea)
				{
					Linea linea = ParsearComentariosDeLinea();
					lineas.Add(linea);
					if (linea == null)
					{
						linea = linea2;
					}
				}
				else
				{
					IList<Comando> comandosDeLaLinea = new List<Comando>();
					while (lexer.tokenActual.Type != TokenType.eof && lexer.tokenActual.Type != TokenType.eol)
					{
						comandosDeLaLinea.Add(ParsearComando());
					}
					LineaDePrograma linea = new LineaDePrograma(comandosDeLaLinea);
					lineas.Add(linea);
					if (lexer.tokenActual.Type == TokenType.eol)
					{
						lexer.Aceptar(TokenType.eol);
					}
				}
			}
			lexer.Aceptar(TokenType.eof);
			Linea[] lineasArr = lineas.ToArray();
			return new Programa(salida, lineasArr);
		}

		private Linea ParsearEspaciosEnBlanco()
		{
			lexer.Aceptar(TokenType.eol);
			return new LineaEnBlanco();
		}

		private Linea ParsearComentariosDeLinea()
		{
			string comentario = lexer.tokenActual.Valor;
			lexer.Aceptar(TokenType.comentarioDeLinea);
			return new LineaComentada(comentario);
		}

		private Comando ParsearComando()
		{
			Comando result = null;

			TokenType tipo = lexer.tokenActual.Type;
			switch (tipo)
			{
				case TokenType.print:
					result = ParsearComandoPrint();
					break;
                case TokenType.procedure:
					Declaracion declaracion = ParsearDeclaracionDeProcedimiento();
					result = new ComandoNulo(declaracion);
					break;
                case TokenType.IF:
					result = ParsearComandoIf();
					break;
                case TokenType.FOR:
                    result = ParsearComandoFor();
                    break;
                case TokenType.begin:
					result = ParsearBloque();
					break;
                case TokenType.id:
					result = ParserComandoCreateOCall();
					break;
                case TokenType.EVAL:
                    result = ParsearComandoEval();
                    break;
                case TokenType.comentarioDeLinea:
					result = ParserComentarioDeLinea();
					break;
				default:
					string hileraConProblemas = lexer.tokenActual.Valor;
					throw new LanguageException("Se encontró con un '" + hileraConProblemas + "' donde se esperaba que inicie un comando.");
			}
			ultimoComandoValido = result;
			return result;
		}

		private DeclaracionProcedure ParsearDeclaracionDeProcedimiento()
		{
			lexer.Aceptar();
			Id id = (Id) ParsearId();
			DeclaracionDeParametro[] parametros = ParsearParametros();
			ComandoBloque comandoBloque = (ComandoBloque) ParsearBloque();
			DeclaracionProcedure declaracion = new DeclaracionProcedure(tablaDeSimbolos, id,parametros,comandoBloque);
			declaracion.guardar();
			return declaracion;
		}

		private DeclaracionDeParametro[] ParsearParametros()
		{
			List<DeclaracionDeParametro> parametros = new List<DeclaracionDeParametro>();
			lexer.Aceptar(TokenType.lParentesis);
			bool salir = lexer.tokenActual.Type == TokenType.rParentesis;
			while (!salir)
			{
				Id nombre = (Id) ParsearId();
				lexer.Aceptar(TokenType.@as);
				Type tipo = ParsearTipo();
				parametros.Add(new DeclaracionDeParametro(nombre, tipo));
				bool siguienteEsUnaComa = lexer.tokenActual.Type == TokenType.coma;
				bool siguienteEsUnCerrarParentesis = lexer.tokenActual.Type == TokenType.rParentesis;
				if (siguienteEsUnCerrarParentesis)
				{
					salir = true;
				}
				else if (siguienteEsUnaComa)
				{
					lexer.Aceptar();
				}
				else
				{
					string hileraConProblemas = lexer.tokenActual.Valor;
					throw new LanguageException("Se esperaba un argumento o un paréntesis, pero se encontró '" + hileraConProblemas + "'",hileraConProblemas, 1, 1);
				}
			}
			lexer.Aceptar(TokenType.rParentesis);
            DeclaracionDeParametro[] parametrosArr = parametros.ToArray();
			return parametrosArr;
		}

		private Type ParsearTipo()
		{
			string nombreDelTipo = lexer.tokenActual.Valor.ToLower();
			Type tipo = tiposPrimitivos[nombreDelTipo];
			if (tipo == null)
			{
				throw new LanguageException("Se encontró un tipo no válido en los parámetros del procedimiento: '" + nombreDelTipo + "'", nombreDelTipo, 1, 1);
			}
			lexer.Aceptar();
			return (Type) tipo;
		}

		private Comando ParsearComandoIf()
		{
			Comando resultado;
			lexer.Aceptar();
			lexer.Aceptar(TokenType.lParentesis);
			Expresion exp = ParseExpresionLogica();
			lexer.Aceptar(TokenType.rParentesis);

			Comando comandosDelIF = ParsearComando();

			if (lexer.tokenActual.Type == TokenType.ELSE)
			{
				lexer.Aceptar();
				Comando comandosDelElse = ParsearComando();
				resultado = new ComandoIf(exp, comandosDelIF, comandosDelElse);
			}
			else
			{
				resultado = new ComandoIf(exp, comandosDelIF);
			}
			return resultado;
		}

        private Comando ParsearComandoFor()
        {
            Comando resultado;
            lexer.Aceptar(TokenType.FOR);
            lexer.Aceptar(TokenType.lParentesis);
            Id id = (Id) ParsearId();
            string variable = id.Valor;
            lexer.Aceptar(TokenType.dosPuntos);
            Expresion exp = ParsearExpresion();
            lexer.Aceptar(TokenType.rParentesis);

            Comando comandosDelFOR = ParsearComando();

            resultado = new ComandoFor(salida, tablaDeSimbolos, variable, exp, comandosDelFOR);
            return resultado;
        }

        private Comando ParserComentarioDeLinea()
		{
			string comentario = lexer.tokenActual.Valor;
			lexer.Aceptar(TokenType.comentarioDeLinea);
			return new ComandoNulo(comentario);
		}

		private Comando ParserComandoCreateOCall()
		{
			Comando resultado;
			Expresion punto = ParsearPunto();
			bool esUnComandoCreate = lexer.tokenActual.Type == TokenType.igual;
			if (esUnComandoCreate)
			{
				resultado = ParsearComandoCreate(punto);
			}
			else
			{
				resultado = ParsearComandoCall(punto);
			}
			lexer.Aceptar(TokenType.puntoComa);
			return resultado;
		}

		private Comando ParsearBloque()
		{
			lexer.Aceptar(TokenType.begin);
			List<Comando> comandosDelBloque = new List<Comando>();
			while (lexer.tokenActual.Type != TokenType.end && lexer.tokenActual.Type != TokenType.eof)
			{
					comandosDelBloque.Add(ParsearComando());
			}
			lexer.Aceptar(TokenType.end);
			if (lexer.tokenActual.Type == TokenType.puntoComa)
			{
				lexer.Aceptar();
			}
            Comando[] comandos = comandosDelBloque.ToArray();
			return new ComandoBloque(tablaDeSimbolos, comandos);
		}

		private Comando ParsearComandoCall(Expresion punto)
		{
			return new ComandoCall(tablaDeSimbolos, punto);
		}

		private Comando ParsearComandoCreate(Expresion lValue)
		{
			lexer.Aceptar(TokenType.igual);
			Expresion rValue = ParseExpresionLogica();
			return new ComandoNuevaInstancia(tablaDeSimbolos, lValue, rValue);
		}

		private Comando ParsearComandoPrint()
		{
			lexer.Aceptar();
			Expresion exp = ParsearExpresion();
            string alias = null;
            if (lexer.tokenActual.Type == TokenType.id)
            {
                alias = lexer.tokenActual.Valor;
                lexer.Aceptar(TokenType.id);
            }
            else if (lexer.tokenActual.Type == TokenType.hilera)
            {
                alias = lexer.tokenActual.Valor;
                lexer.Aceptar(TokenType.hilera);
            }
            else
            {
                throw new LanguageException($"Se esperaba el alias para la expresion '{exp.ToString()}' del Print");
            }
            lexer.Aceptar(TokenType.puntoComa);
			return new ComandoPrint(salida, exp, alias);
		}

        private Comando ParsearComandoEval()
        {
            lexer.Aceptar(TokenType.EVAL);
            lexer.Aceptar(TokenType.lParentesis);
            Expresion exp = ParsearExpresion();
            lexer.Aceptar(TokenType.rParentesis);
            lexer.Aceptar(TokenType.puntoComa);
            return new ComandoEval(assembly, tablaDeSimbolos, salida, exp);
        }

        private Expresion ParsearPunto()
		{
			Expresion resultado = ParsearId();
			TokenType tipo = lexer.tokenActual.Type;
			switch (tipo)
			{
                case TokenType.punto:
					lexer.Aceptar();
					string metodo = lexer.tokenActual.Valor;
					lexer.Aceptar(TokenType.id);

					if (lexer.tokenActual.Type != TokenType.lParentesis)
					{
						resultado = new IdConPunto(tablaDeSimbolos, (Id) resultado, metodo);
                        bool siguienteEsUnPunto = lexer.tokenActual.Type == TokenType.punto;
                        while (siguienteEsUnPunto)
                        {
                            lexer.Aceptar();
                            metodo = lexer.tokenActual.Valor;
                            lexer.Aceptar(TokenType.id);

                            if (lexer.tokenActual.Type != TokenType.lParentesis)
                            {
                                resultado = new PuntoConPunto((Punto)resultado, metodo);
                            }
                            else
                            {
                                lexer.Aceptar(TokenType.lParentesis);
                                resultado = new PuntoConPunto((Punto)resultado, metodo, ParsearArgumentos());
                                lexer.Aceptar(TokenType.rParentesis);
                            }
                            siguienteEsUnPunto = lexer.tokenActual.Type == TokenType.punto;
                        }
                    }
                    else
					{
						lexer.Aceptar(TokenType.lParentesis);
						resultado = new IdConPunto(tablaDeSimbolos, (Id) resultado, metodo, ParsearArgumentos());
						lexer.Aceptar(TokenType.rParentesis);

						bool siguienteEsUnPunto = lexer.tokenActual.Type == TokenType.punto;
						while (siguienteEsUnPunto)
						{
							lexer.Aceptar();
							metodo = lexer.tokenActual.Valor;
							lexer.Aceptar(TokenType.id);

							if (lexer.tokenActual.Type != TokenType.lParentesis)
							{
								resultado = new PuntoConPunto((Punto) resultado, metodo);
							}
							else
							{
								lexer.Aceptar(TokenType.lParentesis);
								resultado = new PuntoConPunto((Punto) resultado, metodo, ParsearArgumentos());
								lexer.Aceptar(TokenType.rParentesis);
							}
							siguienteEsUnPunto = lexer.tokenActual.Type == TokenType.punto;
						}
					}
					break;
                case TokenType.lParentesis:
					lexer.Aceptar();
					resultado = new NuevaInstancia(libraries, tablaDeSimbolos, (Id) resultado, ParsearArgumentos());
					lexer.Aceptar(TokenType.rParentesis);
					break;
			}
			return resultado;
		}

		private Expresion[] ParsearArgumentos()
		{
			List<Expresion> argumentos = new List<Expresion>();
			bool salir = false;
			bool siguienteCierraParentesis = lexer.tokenActual.Type == TokenType.rParentesis;
			if (siguienteCierraParentesis)
			{
				salir = true;
			}
			while (!salir)
			{
				Expresion argumento = ParseExpresionLogica();
				argumentos.Add(argumento);
				bool siguienteEsUnaComa = lexer.tokenActual.Type == TokenType.coma;
				siguienteCierraParentesis = lexer.tokenActual.Type == TokenType.rParentesis;
				if (siguienteCierraParentesis)
				{
					salir = true;
				}
				else if (siguienteEsUnaComa)
				{
					lexer.Aceptar();
				}
				else
				{
					string hileraConProblemas = lexer.tokenActual.Valor;
					throw new LanguageException("Se esperaba un argumento o un paréntesis para abrir la función pero se encontró, '" + hileraConProblemas + "'",hileraConProblemas,1,1);
				}
			}
            Expresion[] argumentosArr = argumentos.ToArray();
			return argumentosArr;
		}

		private Expresion ParsearId()
		{
			string id = lexer.tokenActual.Valor;
			lexer.Aceptar(TokenType.id);
			return new Id(tablaDeSimbolos, id);
		}

		private Expresion ParsearExpresion()
		{
			Expresion resultado = ParseExpresionRelacional();
			return resultado;
		}

		private Expresion ParseFecha()
		{
			Fecha fecha = ParserValidation.parseFechaValidacion(lexer);
			Expresion resultado = new LiteralFecha(fecha);
			if (lexer.tokenActual.Type == Token.TokenType.hora)
			{
				resultado = ParseFechaHora(fecha);
			}
			return resultado;
		}

		private Expresion ParseFechaHora(Fecha fecha)
		{
			FechaHora fechaHora = ParserValidation.parseFechaHoraValidacion(fecha, lexer);

			Expresion resultado = new LiteralFechaHora(fechaHora);
			return resultado;
		}


		private bool EsOperadorRelacional()
		{
			TokenType tipo = lexer.tokenActual.Type;

			return tipo == TokenType.igualdad || tipo == TokenType.desigualdad || tipo == TokenType.menorIgual || tipo == TokenType.mayorIgual || tipo == TokenType.menor || tipo == TokenType.mayor;
		}

		private bool EsOperadorLogica()
		{
			TokenType tipo = lexer.tokenActual.Type;

			return tipo == TokenType.yLogico || tipo == TokenType.oLogico;
		}

		private Expresion ParseExpresionLogica()
		{
			Expresion resultado = ParsearExpresion();

			bool siguienteOperadorLogico = EsOperadorLogica();

			while (siguienteOperadorLogico)
			{
				TokenType tipo = lexer.tokenActual.Type;
				lexer.Aceptar();
				Expresion segundoObjeto = ParsearExpresion();
				switch (tipo)
				{
                    case TokenType.yLogico:
						resultado = new OpAnd(resultado, segundoObjeto);
						break;
                    case TokenType.oLogico:
						resultado = new OpOr(resultado, segundoObjeto);
						break;
					default:
						break;
				}
                siguienteOperadorLogico = EsOperadorLogica();
			}
			return resultado;
		}

		private Expresion ParseExpresionRelacional()
		{
			Expresion resultado = ParseExpresionMultiplicativa();

			bool siguienteOperadorRelacional = EsOperadorRelacional();
			if (siguienteOperadorRelacional)
			{
				TokenType tipo = lexer.tokenActual.Type;
				lexer.Aceptar();
				Expresion segundoObjeto = ParsearExpresion();

				switch (tipo)
				{
                    case TokenType.igualdad:
						resultado = new OpIgualQue(resultado, segundoObjeto);
						break;

                    case TokenType.desigualdad:
						resultado = new OpNoIgualQue(resultado, segundoObjeto);
						break;

                    case TokenType.menor:
						resultado = new OpMenorQue(resultado, segundoObjeto);
						break;

                    case TokenType.mayor:
						resultado = new OpMayorQue(resultado, segundoObjeto);
						break;

                    case TokenType.menorIgual:
						resultado = new OpMenorOIgualQue(resultado, segundoObjeto);
						break;

                    case TokenType.mayorIgual:
						resultado = new OpMayorOIgualQue(resultado, segundoObjeto);
						break;
					default:
						break;
				}
			}
			return resultado;
		}

		private Expresion ParseExpresionMultiplicativa()
		{
			Expresion resultado = ParseExpresionAditiva();
			TokenType tipo = lexer.tokenActual.Type;
			Expresion segundoObjeto;
			switch (tipo)
			{
                case TokenType.multiplicacion:
					lexer.Aceptar();
					segundoObjeto = ParseExpresionMultiplicativa();
					resultado = new OpMultiplicar(resultado, segundoObjeto);
					break;
                case TokenType.division:
					lexer.Aceptar();
					segundoObjeto = ParseExpresionMultiplicativa();
					resultado = new OpDividir(resultado, segundoObjeto);
					break;
				default:
					break;
			}
			return resultado;
		}

		private Expresion ParseExpresionAditiva()
		{
			Expresion resultado = ParseExpresionAtomica();
			TokenType tipo = lexer.tokenActual.Type;
			Expresion segundoObjeto;
			switch (tipo)
			{
                case TokenType.suma:
					lexer.Aceptar();
					segundoObjeto = ParseExpresionMultiplicativa();
					resultado = new OpSumar(resultado, segundoObjeto);
					break;
                case TokenType.resta:
					lexer.Aceptar();
					segundoObjeto = ParseExpresionMultiplicativa();
					resultado = new OpRestar(resultado, segundoObjeto);
					break;
				default:
					break;
			}
			return resultado;
		}

		private Expresion ParseExpresionAtomica()
		{
			Expresion resultado;
			TokenType tipo = lexer.tokenActual.Type;
			switch (tipo)
			{
                case TokenType.lParentesis:
					lexer.Aceptar();
                    resultado = ParseExpresionLogica();
					lexer.Aceptar(Token.TokenType.rParentesis);
					break;
                case TokenType.id:
					resultado = ParsearPunto();
					break;
                case TokenType.resta:
					lexer.Aceptar();
					resultado = ParseExpresionMultiplicativa();
					resultado = new OpMenos(resultado);
					break;
                case TokenType.suma:
					lexer.Aceptar();
					resultado = ParseExpresionMultiplicativa();
					break;
                case TokenType.negacionLogica:
					lexer.Aceptar();
					resultado = ParsearExpresion();
					resultado = new OpNot(resultado);
					break;
				default :
					resultado = ParseLiteral();
					break;
			}
			return resultado;
		}

		private Expresion ParseLiteral()
		{
			Expresion resultado = null;
			TokenType tipoDelLiteral = lexer.tokenActual.Type;
			switch (tipoDelLiteral)
			{
                case TokenType.numero:
					resultado = ParseNumero();
					break;

                case TokenType.hilera:
					resultado = ParseHilera();
					break;

                case TokenType.@decimal:
					resultado = ParseDecimal();
					break;

                case TokenType.nulo:
					resultado = ParseNull();
					break;

                case TokenType.fecha:
					resultado = ParseFecha();
					break;

                case TokenType.mes:
					resultado = ParseMes();
					break;

                case TokenType.monto:
					resultado = ParseMonto();
					break;

                case TokenType.boolFalse:
                case TokenType.boolTrue:
					resultado = ParseBoolean();
					break;

				default:
					string hileraConProblemas = lexer.tokenActual.Valor;
					throw new LanguageException("El símbolo '" + hileraConProblemas + "' es desconocido", hileraConProblemas, 1, 1);
			}
			return resultado;
		}

		private Expresion ParseMes()
		{
			const char SEPARADOR = '/';
			string hilera = lexer.tokenActual.Valor;
			int index = hilera.IndexOf(SEPARADOR);
			Meses mes = Meses.valueOf(hilera.Substring(0, index).ToUpper());
			int anno = int.Parse(hilera.Substring(index + 1));

			lexer.Aceptar(TokenType.mes);

			return new LiteralMes(mes, anno);
		}

		private Expresion ParseBoolean()
		{
			Expresion resultado = new LiteralBoolean(bool.Parse(lexer.tokenActual.Valor.ToLower()));
			lexer.Aceptar();
			return resultado;
		}

		private Expresion ParseHilera()
		{
			string literal = lexer.tokenActual.Valor;
			lexer.Aceptar();
			return new LiteralHilera(literal);
		}

		private Expresion ParseDecimal()
		{
            double valor = double.Parse(lexer.tokenActual.Valor, CultureInfo.GetCultureInfo("en-US"));
            Expresion decimalLiteral = new LiteralDecimal(valor);
			lexer.Aceptar();
			return decimalLiteral;
		}

		private Expresion ParseNull()
		{
			Expresion resultado = new LiteralNull();
			lexer.Aceptar();
			return resultado;
		}

		private Expresion ParseNumero()
		{
			Expresion resultado = new LiteralNumero(int.Parse(lexer.tokenActual.Valor));
			lexer.Aceptar();
			return resultado;
		}

		private Expresion ParseMonto()
		{
			Monedas tipoActual = Monedas.obtenerTipoDeMonto(new Hilera(lexer.tokenActual.Valor));

			Hilera unidadMonetaria;
			double cantidad;
			string simbolo = tipoActual.nombre();
			unidadMonetaria = new Hilera(simbolo);
			cantidad = double.Parse(lexer.tokenActual.Valor.Substring(simbolo.Length));

			Expresion resultado = new LiteralMoneda(ParserValidation.validacionMoneda(unidadMonetaria, cantidad, lexer));

			lexer.Aceptar();
			return resultado;
		}

		public virtual string ComandoActual()
		{
			return ultimoComandoValido.ToString();
		}

		public virtual int Fila()
		{
			return lexer.Fila();
		}

		public virtual int Columna()
		{
			return lexer.Columna();
		}
	}
}